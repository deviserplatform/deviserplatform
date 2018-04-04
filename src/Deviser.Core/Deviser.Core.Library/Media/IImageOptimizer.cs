using System.IO;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Library.Media
{
    public interface IImageOptimizer
    {
        byte[] OptimizeImage(byte[] inputImage, ImageOptimizeParams imageOptimizeParams = null);
    }
}