using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public interface IClientAction
    {

    }

    public class OpenUrlAction : IClientAction
    {
        /// <summary>
        /// Url to open
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Url to open after N sec
        /// </summary>
        public int OpenAfterSec { get; set; }
    }
}
