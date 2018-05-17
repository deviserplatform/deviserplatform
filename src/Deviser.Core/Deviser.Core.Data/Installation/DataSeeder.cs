using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Deviser.Core.Data.DataMigration;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Deviser.Core.Data.Installation
{
    public class DataSeeder
    {
        private readonly DeviserDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DataSeeder(DeviserDbContext applicationDbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public void InsertData()
        {
            string jsonfile = Path.Combine(_hostingEnvironment.ContentRootPath, "DataSeed.json");            
            using (StreamReader r = new StreamReader(jsonfile))
            {
                string json = r.ReadToEnd();
                var entityManager = new EntityManager(_dbContext);
                entityManager.ImportData(json);
            }
        }
    }
}
