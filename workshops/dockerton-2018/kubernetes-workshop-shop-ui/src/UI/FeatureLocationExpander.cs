using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DFDS.UI
{
    public class FeatureLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Features/{1}/{0}.cshtml",
                "/Features/{1}s/{0}.cshtml",
                "/Features/Shared/{0}.cshtml"
            };
        }
    }
}