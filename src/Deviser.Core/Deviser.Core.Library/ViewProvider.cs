using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace Deviser.Core.Library
{
    public interface IViewProvider
    {
        IList<CompiledViewDescriptor> GetCompiledViewDescriptors();
    }

    public class ViewProvider : IViewProvider
    {
        private readonly ApplicationPartManager _applicationPartManager;
        public ViewProvider(ApplicationPartManager applicationPartManager)
        {
            _applicationPartManager = applicationPartManager;
        }

        public IList<CompiledViewDescriptor> GetCompiledViewDescriptors()
        {
            var feature = new ViewsFeature();
            _applicationPartManager.PopulateFeature(feature);
            var views = feature.ViewDescriptors;
            return views;
        }
    }
}
