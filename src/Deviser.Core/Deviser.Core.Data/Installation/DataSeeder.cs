using Deviser.Core.Data.DataMigration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Deviser.Core.Common.FileProviders;
using Microsoft.Extensions.FileProviders;

namespace Deviser.Core.Data.Installation
{
    public class DataSeeder
    {
        private readonly DeviserDbContext _dbContext;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        public DataSeeder(DeviserDbContext applicationDbContext/*, IWebHostEnvironment hostingEnvironment*/)
        {
            _dbContext = applicationDbContext;
            //_hostingEnvironment = hostingEnvironment;
        }

        public void InsertData()
        {
            //var manifestEmbeddedProvider =
            //    new ManifestEmbeddedFileProvider(typeof(DataSeeder).Assembly);

            //var fileInfo = manifestEmbeddedProvider.GetFileInfo("DataSeed.json");
            ////string jsonfile = Path.Combine(_hostingEnvironment.ContentRootPath, "DataSeed.json");
            //using var stream = fileInfo.CreateReadStream();
            //using var reader = new StreamReader(stream);
            //var json = reader.ReadToEnd();

            var json = EmbeddedProvider.GetFileContentAsString(typeof(DataSeeder).Assembly, "DataSeed.json");

            var entityManager = new EntityManager(_dbContext);
            entityManager.ImportData(json);
        }
    }
}
