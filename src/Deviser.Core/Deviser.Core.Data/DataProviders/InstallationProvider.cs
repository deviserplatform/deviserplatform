using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data;
using Deviser.Core.Data.DataProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Deviser.Core.Data.DataProviders
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
        DbContextOptions GetDbContextOptions();
        DbContextOptionsBuilder GetDbContextOptionsBuilder();
        DbContextOptionsBuilder GetDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder);
    }

    public class InstallationProvider : IInstallationProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private DbContextOptions _dbContextOptions;
        private DbContextOptionsBuilder _dbContextOptionsBuilder;

        private InstallModel _installModel;
        private bool _isPlarformInstalled;
        private bool _isDbExist;

        public InstallationProvider(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public bool IsPlatformInstalled
        {
            get
            {
                if (!_isPlarformInstalled)
                {
                    var installModel = GetInstallationModel();
                    _isPlarformInstalled = installModel != null ? true : false;
                }
                return _isPlarformInstalled;
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
                return _isPlarformInstalled;
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
            //TODO: Insert data
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
            else
            {
                //SQLite
                return $"Data Source={model.DatabaseName}.db";
            }
        }

        public DbContextOptions GetDbContextOptions()
        {
            if (_dbContextOptions == null)
            {
                InstallModel installModel = GetInstallationModel();
                _dbContextOptions = GetDbContextOptions(installModel);
            }

            if (_dbContextOptions == null)
                throw new NullReferenceException("Platform is not installed properly. Kindly install it properly");

            return _dbContextOptions;
        }

        public DbContextOptionsBuilder GetDbContextOptionsBuilder()
        {
            if (_dbContextOptionsBuilder == null)
            {
                InstallModel installModel = GetInstallationModel();
                _dbContextOptionsBuilder = GetDbContextOptionsBuilder(installModel);
            }

            if (_dbContextOptionsBuilder == null)
                throw new NullReferenceException("Platform is not installed properly. Kindly install it properly");

            return _dbContextOptionsBuilder;
        }

        public DbContextOptionsBuilder GetDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {   
            InstallModel installModel = GetInstallationModel();
            _dbContextOptionsBuilder = GetDbContextOptionsBuilder(installModel, optionsBuilder);

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

        private DbContextOptionsBuilder GetDbContextOptionsBuilder(InstallModel installModel, DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = GetConnectionString(installModel);
            if (installModel.DatabaseProvider == DatabaseProvider.SQLServer)
            {
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
            }
            else if (installModel.DatabaseProvider == DatabaseProvider.SQLLocalDb)
            {
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
            }
            else if (installModel.DatabaseProvider == DatabaseProvider.PostgreSQL)
            {
                optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
            }
            else
            {
                optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly("Deviser.WI"));
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
    }
}
