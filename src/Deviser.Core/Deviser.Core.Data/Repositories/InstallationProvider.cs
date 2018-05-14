using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common.Internal;
using Deviser.Core.Data;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Data.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Deviser.Core.Data.Installation;

namespace Deviser.Core.Data.Repositories
{

    public interface IInstallationProvider
    {
        //bool IsPlatformInstalled();
        //bool IsDatabaseExist();

        bool IsPlatformInstalled { get; }

        bool IsDatabaseExist { get; }

        void InstallPlatform(InstallModel installModel);
        void InsertData(DbContextOptions dbOption);
        string GetConnectionString(InstallModel model);
        DbContextOptionsBuilder GetDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, string moduleAssembly=null);
    }

    public class InstallationProvider : IInstallationProvider
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        private DbContextOptions _dbContextOptions;
        private DbContextOptionsBuilder _dbContextOptionsBuilder;
        private InstallModel _installModel;
        private bool _isPlatformInstalled;
        private bool _isDbExist;

        public InstallationProvider(IHostingEnvironment hostingEnvironment, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public bool IsPlatformInstalled
        {
            get
            {
                if (!_isPlatformInstalled)
                {
                    var installModel = GetInstallationModel();
                    _isPlatformInstalled = installModel != null ? true : false;
                }
                return _isPlatformInstalled;
            }
        }

        public bool IsDatabaseExist
        {
            get
            {
                if (!IsPlatformInstalled)
                    return false;

                if (!_isDbExist)
                {
                    var installModel = GetInstallationModel();
                    var connectionString = GetConnectionString(installModel);
                    _isDbExist = IsDatabaseExistsFor(connectionString);
                }
                return _isPlatformInstalled;
            }
        }

        public void InstallPlatform(InstallModel installModel)
        {
            string connectionString = GetConnectionString(installModel);
            string settingFile = Path.Combine(_hostingEnvironment.ContentRootPath, $"appsettings.{_hostingEnvironment.EnvironmentName}.json");
            DbContextOptionsBuilder dbContextOptionsBuilder = GetDbContextOptionsBuilder(installModel);
            DbContextOptions dbOption = dbContextOptionsBuilder.Options;

            if (!IsDatabaseExistsFor(connectionString))
            {
                //Creating database                        
                using (var context = new DeviserDbContext(dbOption))
                {
                    context.Database.Migrate();
                }

                //Insert data
                InsertData(dbOption);

                //Migrate module
                MigrateModuleContexts(installModel);
            }

            //Write intall settings
            WriteInstallSettings(installModel);

            //Update connection string 
            //Writing to appsettings.json file
            string json = File.ReadAllText(settingFile);
            JObject jsonObj = JObject.Parse(json);
            jsonObj["ConnectionStrings"]["DefaultConnection"] = connectionString;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(settingFile, output);

            //Updating it in cache
            //_configuration["ConnectionStrings:DefaultConnection"] = connectionString;

            //Success no exceptions were thrown
            _dbContextOptions = dbOption;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public void InsertData(DbContextOptions dbOption)
        {
            using (var context = new DeviserDbContext(dbOption))
            {
                var dataSeeder = new DataSeeder(context);
                dataSeeder.InsertData();
            }
        }

        public string GetConnectionString(InstallModel model)
        {
            if (model.DatabaseProvider == DatabaseProvider.SQLServer)
            {
                if (model.IsIntegratedSecurity)
                    return $"Server={model.ServerName};Database={model.DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
                return $"Server={model.ServerName};Database={model.DatabaseName};User Id={model.DBUserName};Password={model.DBPassword}";
            }
            else if (model.DatabaseProvider == DatabaseProvider.SQLLocalDb)
            {
                return $"Server={model.ServerName};Database={model.DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
            }
            else if (model.DatabaseProvider == DatabaseProvider.PostgreSQL)
            {
                return $"Server={model.ServerName};Database={model.DatabaseName};Username={model.DBUserName};Password={model.DBPassword}";
            }
            else if (model.DatabaseProvider == DatabaseProvider.MySQL)
            {
                return $"server={model.ServerName};database={model.DatabaseName};user={model.DBUserName};password={model.DBPassword}";
            }
            else
            {
                //SQLite
                return $"Data Source={model.DatabaseName}.db";
            }
        }
        
        public DbContextOptionsBuilder GetDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, string moduleAssembly=null)
        {   
            InstallModel installModel = GetInstallationModel();
            _dbContextOptionsBuilder = GetDbContextOptionsBuilder(installModel, optionsBuilder, moduleAssembly);

            if (_dbContextOptionsBuilder == null)
                throw new NullReferenceException("Platform is not installed properly. Kindly install it properly");


            return _dbContextOptionsBuilder;
        }

        public DbContextOptions GetDbContextOptions(InstallModel installModel)
        {
            return GetDbContextOptionsBuilder(installModel).Options;
        }

        private DbContextOptionsBuilder GetDbContextOptionsBuilder(InstallModel installModel)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeviserDbContext>();
            return GetDbContextOptionsBuilder(installModel, optionsBuilder);
        }

        private DbContextOptionsBuilder GetDbContextOptionsBuilder(InstallModel installModel, DbContextOptionsBuilder optionsBuilder, string moduleAssembly=null)
        {
            string connectionString = IsPlatformInstalled? _configuration.GetConnectionString("DefaultConnection"): GetConnectionString(installModel);


            if (installModel.DatabaseProvider == DatabaseProvider.SQLServer || installModel.DatabaseProvider == DatabaseProvider.SQLLocalDb)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(Globals.PlatformAssembly));
                }
                else
                {
                    optionsBuilder.UseSqlServer(connectionString, (x) =>
                    {
                        x.MigrationsAssembly(moduleAssembly);
                        x.MigrationsHistoryTable(Globals.ModuleMigrationTableName);
                    })
                    .ReplaceService<IHistoryRepository, SqlServerModuleHistoryRepository>();
                }
                
            }
            else if (installModel.DatabaseProvider == DatabaseProvider.PostgreSQL)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(Globals.PlatformAssembly));
                }
                else
                {
                    optionsBuilder.UseNpgsql(connectionString, (x) =>
                    {
                        x.MigrationsAssembly(moduleAssembly);
                        x.MigrationsHistoryTable(Globals.ModuleMigrationTableName);
                    })
                    .ReplaceService<IHistoryRepository, NpgsqlModuleHistoryRepository>();
                }
            }
            else if (installModel.DatabaseProvider == DatabaseProvider.MySQL)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseMySQL(connectionString, b => b.MigrationsAssembly(Globals.PlatformAssembly));
                }
                else
                {
                    optionsBuilder.UseMySQL(connectionString, (x) =>
                    {
                        x.MigrationsAssembly(moduleAssembly);
                        x.MigrationsHistoryTable(Globals.ModuleMigrationTableName);
                    })
                    .ReplaceService<IHistoryRepository, NpgsqlModuleHistoryRepository>();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly(Globals.PlatformAssembly));
                }
                else
                {
                    optionsBuilder.UseSqlite(connectionString, (x) =>
                    {
                        x.MigrationsAssembly(moduleAssembly);
                        x.MigrationsHistoryTable(Globals.ModuleMigrationTableName);
                    })
                    .ReplaceService<IHistoryRepository, SqliteModuleHistoryRepository>();
                }
            }

            return optionsBuilder;
        }

        private bool IsDatabaseExistsFor(string connectionString)
        {
            try
            {
                //just try to connect
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get InstallModel from installed config file
        /// </summary>
        /// <returns>InstallModel if the platform has been installed</returns>
        private InstallModel GetInstallationModel()
        {
            if (_installModel != null)
                return _installModel;

            string installationFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, Globals.InstallConfigFile);
            if (File.Exists(installationFilePath))
            {
                string strInstallation = File.ReadAllText(installationFilePath);
                _installModel = !string.IsNullOrEmpty(strInstallation) ? SDJsonConvert.DeserializeObject<InstallModel>(strInstallation) : null;
                return _installModel;
            }
            return null;
        }

        private void WriteInstallSettings(InstallModel model)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(contentRootPath, Globals.InstallConfigFile);
            //Save setting
            File.WriteAllText(filePath, JsonConvert.SerializeObject(model));
        }

        private void MigrateModuleContexts(InstallModel installModel)
        {
            var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.PlatformAssembly);
            List<TypeInfo> moduleDbContextTypes = new List<TypeInfo>();

            Type moduleDbContextBaseType = typeof(ModuleDbContext);
            PropertyInfo databaseField = moduleDbContextBaseType.GetProperty("Database");
            MethodInfo registerServiceMethodInfo = typeof(Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions).GetMethod("Migrate");
            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.DefinedTypes.Where(t => moduleDbContextBaseType.IsAssignableFrom(t)).ToList();

                if (controllerTypes != null && controllerTypes.Count > 0)
                    moduleDbContextTypes.AddRange(controllerTypes);
            }

            if (moduleDbContextTypes.Count > 0)
            {
                foreach (var moduleDbContextType in moduleDbContextTypes)
                {
                    var moduleDbOptionBuilderGType = typeof(DbContextOptionsBuilder<>);
                    Type[] typeArgs = { moduleDbContextType };
                    var moduleDbOptionBuilderType = moduleDbOptionBuilderGType.MakeGenericType(typeArgs);
                    var moduleDbOptionBuilder = Activator.CreateInstance(moduleDbOptionBuilderType); //var optionsBuilder = new DbContextOptionsBuilder<DeviserDbContext>();
                    var dbContextOptionBuilder = GetDbContextOptionsBuilder(installModel, (DbContextOptionsBuilder)moduleDbOptionBuilder);

                    var moduleDbContextObj = Activator.CreateInstance(moduleDbContextType, dbContextOptionBuilder.Options);

                    var databaseObj = databaseField.GetValue(moduleDbContextObj);

                    registerServiceMethodInfo.Invoke(databaseObj, new object[] { databaseObj });
                    //registerServiceMethodInfo.Invoke(obj, new object[] { serviceCollection });

                }
            }
        }
    }
}
