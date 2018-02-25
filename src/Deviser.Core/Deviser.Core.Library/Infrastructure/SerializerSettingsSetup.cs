using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Infrastructure
{
    /// <summary>
    /// Implementation based on https://github.com/aspnet/Mvc/issues/4562
    /// </summary>
    public class SerializerSettingsSetup : IConfigureOptions<MvcOptions>
    {   
        private readonly ArrayPool<char> _charPool;

        public SerializerSettingsSetup(
            ArrayPool<char> charPool)
        {            
            _charPool = charPool;
        }

        public void Configure(MvcOptions options)
        {
            options.OutputFormatters.RemoveType<JsonOutputFormatter>();
            //options.InputFormatters.RemoveType<JsonInputFormatter>();
            //options.InputFormatters.RemoveType<JsonPatchInputFormatter>();

            var outputSettings = new JsonSerializerSettings();

            outputSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            outputSettings.Formatting = Formatting.Indented;
            outputSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var jsonOutputFormatter = new JsonOutputFormatter(outputSettings, _charPool);
            
            options.OutputFormatters.Add(jsonOutputFormatter);

            //var inputSettings = new JsonSerializerSettings();
            //var jsonInputLogger = _loggerFactory.CreateLogger<JsonInputFormatter>();
            //options.InputFormatters.Add(new JsonInputFormatter(
            //    jsonInputLogger,
            //    inputSettings,
            //    _charPool,
            //    _objectPoolProvider));

            //var jsonInputPatchLogger = _loggerFactory.CreateLogger<JsonPatchInputFormatter>();
            //options.InputFormatters.Add(new JsonPatchInputFormatter(
            //    jsonInputPatchLogger,
            //    inputSettings,
            //    _charPool,
            //    _objectPoolProvider));
        }
    }
}
