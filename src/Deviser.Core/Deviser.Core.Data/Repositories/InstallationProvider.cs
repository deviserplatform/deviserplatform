using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common.Internal;
using Deviser.Core.Data.Extension;
using Deviser.Core.Data.Installation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Hubs;
using Deviser.Core.Data.Installation.Contexts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Deviser.Core.Data.Repositories
{
    public class InstallationProvider : IInstallationProvider
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ApplicationHub> _hubContext;
        private readonly ILogger<InstallationProvider> _logger;

        private UserManager<Entities.User> _userManager;
        private IServiceCollection _services;


        private DbContextOptions _dbContextOptions;
        private DbContextOptionsBuilder _dbContextOptionsBuilder;
        private static InstallModel _installModel;
        private static Dictionary<DatabaseProvider, string> ConnectionStringKeys = Globals.ConnectionStringKeys;
        private static bool _isInstallInProgress;
        private static bool _isPlatformInstalled;
        private static bool _isDbExist;


        public InstallationProvider(IWebHostEnvironment hostingEnvironment,
            IHubContext<ApplicationHub> hubContext,
            IConfiguration configuration,
            ILogger<InstallationProvider> logger,
            IServiceProvider serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _hubContext = hubContext;
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public bool IsPlatformInstalled
        {
            get
            {
                if (_isInstallInProgress)
                    return false;

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

        public async Task InstallPlatform(InstallModel installModel)
        {
            var connectionString = GetConnectionString(installModel);
            var settingFile = Path.Combine(_hostingEnvironment.ContentRootPath, $"appsettings.{_hostingEnvironment.EnvironmentName}.json");
            var dbContextOptionsBuilder = GetDbContextOptionsBuilder<DeviserDbContext>(installModel);
            var dbOption = dbContextOptionsBuilder.Options;
            _isInstallInProgress = true;
            _installModel = installModel;
            await UpdateInstallLog($"Deviser Platform Installation begins");
            await UpdateInstallLog($"Verifying whether the database exist");
            //if (!IsDatabaseExistsFor(connectionString))
            //{
            //Creating database                        
            await UpdateInstallLog($"Creating database");
            var deviserDbContextType = typeof(DeviserDbContext);
            var contextType = this.GetType().Assembly.DefinedTypes
                .FirstOrDefault(t =>
                    deviserDbContextType.IsAssignableFrom(t) &&
                    t.Name.ToLower().Contains(installModel.DatabaseProvider.ToString().ToLower()));

            await using var context = GetDbContext(contextType, dbOption);
            await UpdateInstallLog($"Creating platform database objects");
            await context.Database.MigrateAsync();

            //Insert data
            await UpdateInstallLog($"Inserting platform data");
            InsertData(dbOption);

            //Migrate module
            await UpdateInstallLog($"Creating module database objects");
            await MigrateModuleContexts(installModel);

            IServiceCollection services = new ServiceCollection();
            var logger = Logger.GetLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(logger, true));
            services.AddDbContext<DeviserDbContext>(
                (internalServiceProvider, dbContextOptionBuilder) =>
                {
                    GetDbContextOptionsBuilder<DeviserDbContext>(dbContextOptionBuilder);
                });
            services.AddIdentity<Entities.User, Entities.Role>()
                .AddEntityFrameworkStores<DeviserDbContext>()
                .AddDefaultTokenProviders();
            var sp = services.BuildServiceProvider();
            //Create user account
            await UpdateInstallLog($"Creating admin user account");
            _userManager = sp.GetService<UserManager<Entities.User>>();

            var user = new Entities.User { UserName = installModel.AdminEmail, Email = installModel.AdminEmail };
            var result = _userManager.CreateAsync(user, installModel.AdminPassword).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                //Assign user to admin role
                await _userManager.AddToRoleAsync(user, "Administrators");
            }
            //}

            //Write install settings
            await UpdateInstallLog($"Creating the config files");
            WriteInstallSettings(installModel);

            //Update connection string 
            //Writing to appsettings.json file
            if (!File.Exists(settingFile))
            {
                var fs = File.Create(settingFile);
                fs.Close();
            }

            await UpdateInstallLog($"Finalizing the installation");
            var json = File.ReadAllText(settingFile);
            if (string.IsNullOrEmpty(json))
            {
                //var jsonObj = JObject.FromObject(new
                //{
                //    ConnectionStrings = new
                //    {
                //        DefaultConnection = connectionString
                //    }
                //});
                //var jsonObj = JObject.Parse(@$"{{ ConnectionStrings: {{ ""{ConnectionStringKeys[installModel.DatabaseProvider]}"":""{connectionString}"" }} }}");
                var jsonObj = JObject.Parse(@"{'ConnectionStrings': {}}");
                jsonObj["ConnectionStrings"][ConnectionStringKeys[installModel.DatabaseProvider]] = connectionString;
                var output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                await File.WriteAllTextAsync(settingFile, output);
            }
            else
            {
                var jsonObj = JObject.Parse(json);
                if (jsonObj["ConnectionStrings"] == null)
                {
                    jsonObj["ConnectionStrings"] = JObject.Parse(@"{}");
                }
                jsonObj["ConnectionStrings"][ConnectionStringKeys[installModel.DatabaseProvider]] = connectionString;
                var output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                await File.WriteAllTextAsync(settingFile, output);
            }


            //Updating it in cache
            //_configuration["ConnectionStrings:DefaultConnection"] = connectionString;

            //Success no exceptions were thrown
            _dbContextOptions = dbOption;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _isInstallInProgress = false;
            await UpdateInstallLog($"Deviser Platform has been installed successfully!");
        }

        public DbContext GetDbContext(Type dbContextType, DbContextOptions options)
        {
            var dbContext = Activator.CreateInstance(dbContextType, options) as DbContext;
            return dbContext;
        }

        public void InsertData(DbContextOptions<DeviserDbContext> dbOption)
        {
            using var context = new DeviserDbContext(dbOption);
            var dataSeeder = new DataSeeder(context);
            dataSeeder.InsertData();
        }

        public string GetConnectionString(InstallModel model)
        {
            if (model.DatabaseProvider == DatabaseProvider.SqlServer)
            {
                if (model.IsIntegratedSecurity)
                    return $"Server={model.ServerName};Database={model.DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
                return $"Server={model.ServerName};Database={model.DatabaseName};User Id={model.DBUserName};Password={model.DBPassword}";
            }
            else if (model.DatabaseProvider == DatabaseProvider.PostgreSql)
            {
                var host = model.ServerName;
                var port = "";

                if (host.Contains(":"))
                {
                    var hostAndPort = host.Split(':');
                    host = hostAndPort[0];
                    port = hostAndPort[1];
                }

                return $"Host={host};{((!string.IsNullOrEmpty(port)) ? $"Port={port};" : "")}Database={model.DatabaseName};User ID={model.DBUserName};Password={model.DBPassword};";
            }
            else if (model.DatabaseProvider == DatabaseProvider.MySql)
            {
                var host = model.ServerName;
                var port = "";

                if (host.Contains(":"))
                {
                    var hostAndPort = host.Split(':');
                    host = hostAndPort[0];
                    port = hostAndPort[1];
                }

                return $"server={host};{((!string.IsNullOrEmpty(port)) ? $"port={port};" : "")}database={model.DatabaseName};user={model.DBUserName};password={model.DBPassword};CharSet=utf8mb4;";
            }
            else
            {
                //SQLite
                return $"Data Source={model.DatabaseName}.db";
            }
        }

        public DbContextOptionsBuilder GetDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, string moduleAssembly = null)
        {
            //DbContextOptionsBuilder<DeviserDbContext>

            var installModel = GetInstallationModel();
            if (installModel == null)
                return null;

            _dbContextOptionsBuilder = GetDbContextOptionsBuilder(installModel, optionsBuilder, moduleAssembly);

            if (_dbContextOptionsBuilder == null)
                throw new NullReferenceException("Platform is not installed properly. Kindly install it properly");


            return _dbContextOptionsBuilder;
        }

        public DbContextOptionsBuilder<TContext> GetDbContextOptionsBuilder<TContext>(DbContextOptionsBuilder optionsBuilder, string moduleAssembly = null)
            where TContext : DbContext
        {
            //DbContextOptionsBuilder<DeviserDbContext>

            var installModel = GetInstallationModel();
            if (installModel == null)
                return null;

            _dbContextOptionsBuilder = GetDbContextOptionsBuilder(installModel, optionsBuilder, moduleAssembly);

            if (_dbContextOptionsBuilder == null)
                throw new NullReferenceException("Platform is not installed properly. Kindly install it properly");


            return (DbContextOptionsBuilder<TContext>)_dbContextOptionsBuilder;
        }


        public DbContextOptions GetDbContextOptions(InstallModel installModel)
        {
            return GetDbContextOptionsBuilder(installModel).Options;
        }

        private DbContextOptionsBuilder GetDbContextOptionsBuilder(InstallModel installModel)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            return GetDbContextOptionsBuilder(installModel, optionsBuilder);
        }

        private DbContextOptionsBuilder<TContext> GetDbContextOptionsBuilder<TContext>(InstallModel installModel)
            where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            return GetDbContextOptionsBuilder<TContext>(installModel, optionsBuilder);
        }

        private DbContextOptionsBuilder GetDbContextOptionsBuilder(InstallModel installModel, DbContextOptionsBuilder optionsBuilder, string moduleAssembly = null)
        {
            var connectionString = IsPlatformInstalled ? _configuration.GetConnectionString(ConnectionStringKeys[installModel.DatabaseProvider]) : GetConnectionString(installModel);


            if (installModel.DatabaseProvider == DatabaseProvider.SqlServer)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(Globals.MigrationAssembly));
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
            else if (installModel.DatabaseProvider == DatabaseProvider.PostgreSql)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(Globals.MigrationAssembly));
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
            else if (installModel.DatabaseProvider == DatabaseProvider.MySql)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly(Globals.MigrationAssembly));
                }
                else
                {
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),(x) =>
                    {
                        x.MigrationsAssembly(moduleAssembly);
                        x.MigrationsHistoryTable(Globals.ModuleMigrationTableName);
                    })
                    .ReplaceService<IHistoryRepository, MySQLModuleHistoryRepository>();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly(Globals.MigrationAssembly));
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

        private DbContextOptionsBuilder<TContext> GetDbContextOptionsBuilder<TContext>(InstallModel installModel, DbContextOptionsBuilder<TContext> optionsBuilder, string moduleAssembly = null)
            where TContext : DbContext
        {
            var connectionString = IsPlatformInstalled ? _configuration.GetConnectionString(ConnectionStringKeys[installModel.DatabaseProvider]) : GetConnectionString(installModel);


            if (installModel.DatabaseProvider == DatabaseProvider.SqlServer)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(Globals.MigrationAssembly));
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
            else if (installModel.DatabaseProvider == DatabaseProvider.PostgreSql)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(Globals.MigrationAssembly));
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
            else if (installModel.DatabaseProvider == DatabaseProvider.MySql)
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly(Globals.MigrationAssembly));
                }
                else
                {
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), (x) =>
                    {
                        x.MigrationsAssembly(moduleAssembly);
                        x.MigrationsHistoryTable(Globals.ModuleMigrationTableName);
                    })
                    .ReplaceService<IHistoryRepository, MySQLModuleHistoryRepository>();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(moduleAssembly))
                {
                    optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly(Globals.MigrationAssembly));
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

            var installationFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, Globals.InstallConfigFile);

            if (!File.Exists(installationFilePath)) return null;

            var strInstallation = File.ReadAllText(installationFilePath);
            _installModel = !string.IsNullOrEmpty(strInstallation) ? SDJsonConvert.DeserializeObject<InstallModel>(strInstallation) : null;
            return _installModel;
        }

        private void WriteInstallSettings(InstallModel model)
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var filePath = Path.Combine(contentRootPath, Globals.InstallConfigFile);
            //Save setting
            File.WriteAllText(filePath, JsonConvert.SerializeObject(model));
        }

        private async Task MigrateModuleContexts(InstallModel installModel)
        {
            var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.EntryPointAssembly);
            var moduleDbContextTypes = new List<TypeInfo>();

            var moduleDbContextBaseType = typeof(ModuleDbContext);
            //var databaseField = moduleDbContextBaseType.GetProperty("Database");
            //var registerServiceMethodInfo = typeof(Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions).GetMethod("MigrateAsync");
            foreach (var assembly in assemblies)
            {
                var moduleDbContextTypesOnAssembly = assembly.DefinedTypes.Where(t => t.IsSubclassOf(moduleDbContextBaseType) && t.BaseType == moduleDbContextBaseType).ToList();

                if (moduleDbContextTypesOnAssembly.Count > 0)
                    moduleDbContextTypes.AddRange(moduleDbContextTypesOnAssembly);
            }

            if (moduleDbContextTypes.Count <= 0) return;

            foreach (var moduleDbContextType in moduleDbContextTypes)
            {
                var assembly = moduleDbContextType.Assembly.GetName().Name;
                var moduleDbOptionBuilderGType = typeof(DbContextOptionsBuilder<>);
                Type[] typeArgs = { moduleDbContextType };
                var moduleDbOptionBuilderType = moduleDbOptionBuilderGType.MakeGenericType(typeArgs);
                var moduleDbOptionBuilder = Activator.CreateInstance(moduleDbOptionBuilderType); //var optionsBuilder = new DbContextOptionsBuilder<DeviserDbContext>();
                var dbContextOptionBuilder = GetDbContextOptionsBuilder(installModel, (DbContextOptionsBuilder)moduleDbOptionBuilder, assembly);

                var contextType = moduleDbContextType.Assembly.DefinedTypes
                    .FirstOrDefault(t =>
                        moduleDbContextType.IsAssignableFrom(t) &&
                        t.Name.ToLower().Contains(installModel.DatabaseProvider.ToString().ToLower()));

                await using var moduleDbContext = GetDbContext(contextType, dbContextOptionBuilder.Options);
                await moduleDbContext.Database.MigrateAsync();
            }
        }

        private async Task UpdateInstallLog(string message)
        {
            _logger.LogInformation(message);
            await _hubContext.Clients.All.SendAsync("OnUpdateInstallLog", message);
        }
        private static void CallGenericMethod(string methodName, Type genericType, object[] parameters)
        {
            var getItemMethodInfo = typeof(InstallationProvider).GetMethods(BindingFlags.NonPublic | BindingFlags.Static).FirstOrDefault(m => m.Name == methodName && m.IsGenericMethod);

            if (getItemMethodInfo == null) return;

            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericType);
            var result = getItemMethod.Invoke(null, parameters);
        }
    }
}
