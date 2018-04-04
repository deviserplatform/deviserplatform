using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Imageflow.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deviser.Core.Library.Media
{
    public class ImageOptimizer : IImageOptimizer
    {

        public byte[] OptimizeImage(byte[] inputImage, ImageOptimizeParams imageOptimizeParams=null)
        {
            if (imageOptimizeParams == null)
            {
                imageOptimizeParams = GetDefaultParams();
            }

            using (var c = new JobContext())
            using (var ms = new MemoryStream())
            {              
                c.AddInputBytesPinned(0, inputImage);
                c.AddOutputBuffer(1);
                //string commandString = $"dpi={72}&maxwidth={1024}&maxheight={1024}&quality={80}";
                string commandString = $"dpi={imageOptimizeParams.Dpi}&maxwidth={imageOptimizeParams.MaxWidth}&maxheight={imageOptimizeParams.MaxHeight}&quality={imageOptimizeParams.QualityPercent}";
                var response = c.ExecuteImageResizer4CommandString(0, 1, commandString);

                var data = response.DeserializeDynamic();
                var outputStream = c.GetOutputBuffer(1);

                //Assert.Equal(200, (int)data.code);
                //Assert.Equal(true, (bool)data.success);		

                outputStream.CopyTo(ms);
                return ms.ToArray();

                //httpContext.Response.ContentType = "image/png";//cbb.Properties.ContentType;   
                //await outputStream.CopyToAsync(httpContext.Response.Body);
            }
        }

        private ImageOptimizeParams GetDefaultParams()
        {
            return new ImageOptimizeParams
            {
                Dpi = Globals.ImageOptimizeDpi,
                MaxHeight = Globals.ImageOptimizeMaxHeight,
                MaxWidth = Globals.ImageOptimizeMaxWidth,
                QualityPercent = Globals.ImageOptimizeQualityPercent
            };
        }
    }
}
