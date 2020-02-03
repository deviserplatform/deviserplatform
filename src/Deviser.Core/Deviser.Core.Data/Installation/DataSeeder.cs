using Deviser.Core.Data.DataMigration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Deviser.Core.Data.Installation
{
    public class DataSeeder
    {
        private readonly DeviserDbContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DataSeeder(DeviserDbContext applicationDbContext, IWebHostEnvironment hostingEnvironment)
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
