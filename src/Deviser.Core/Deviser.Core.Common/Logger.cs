using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog;

namespace Deviser.Core.Common
{
    public static class Logger
    {
        public static ILogger GetLogger()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine("./logs", "log-{Date}.txt"))
                .CreateLogger();
            return logger;
        }
    }
}
