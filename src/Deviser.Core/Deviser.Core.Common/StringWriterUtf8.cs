using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deviser.Core.Common
{
    public class StringWriterUtf8 : StringWriter
    {
        public StringWriterUtf8() //: base(sb)
        {
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
