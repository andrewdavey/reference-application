using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Infrastructure.Cassette
{
    /// <summary>
    /// In debug-mode every script asset is loaded separately to make debugging easy.
    /// This class builds AMD module shims to maintain the illusion of real modules.
    /// </summary>
    public class DebugModuleBuilder
    {
        readonly string modulePath;
        readonly List<string> assetUrls = new List<string>();
        readonly HashSet<string> dependencyPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
 
        public DebugModuleBuilder(string modulePath)
        {
            this.modulePath = modulePath;
        }

        public void AddAssetUrl(string assetUrl)
        {
            assetUrls.Add(assetUrl);
        }

        public void AddDependencies(IEnumerable<string> dependencyPathsToAdd)
        {
            foreach (var path in dependencyPathsToAdd)
            {
                dependencyPaths.Add(path);
            }
        }

        public string Build()
        {
            // We assume that `debugModules` has been defined already.
            // An array property is added to this object.
            // Each asset will add an initialization function to the array.
            // Once all the assets have been loaded by require.js, we call each initialization function.

            var urlsList = UrlsList(assetUrls, dependencyPaths);
            var parameters = Parameters(dependencyPaths);
            var dependencyObjectProperties = DependencyObjectProperties(dependencyPaths);

            return "debugModules['" + modulePath + "']=[];\r\n" +
                   "define(\r\n" +
                   "    '" + modulePath + "',\r\n" +
                   "    [" + urlsList + "],\r\n" +
                   "    function(" + parameters + "){\r\n" +
                   "        var module = {}, deps = {" + dependencyObjectProperties + "};\r\n" +
                   "        debugModules['" + modulePath + "'].forEach(function(init){\r\n" +
                   "            init(module, deps);\r\n" +
                   "        });\r\n" +
                   "        return module;\r\n" +
                   "    }\r\n" +
                   ");";
        }

        string UrlsList(IEnumerable<string> assetUrls, IEnumerable<string> dependencyPaths)
        {
            var urls = dependencyPaths.Concat(assetUrls);
            return CommaSeparated(urls.Select(d => string.Format("'{0}'", d)));
        }

        string Parameters(IEnumerable<string> dependencyPaths)
        {
            return CommaSeparated(dependencyPaths.Select((d, index) => "d" + index));
        }

        string DependencyObjectProperties(IEnumerable<string> dependencyPaths)
        {
            return CommaSeparated(dependencyPaths.Select((d, index) => string.Format("'{0}':d{1}", d, index)));
        }

        string CommaSeparated(IEnumerable<string> items)
        {
            return string.Join(",", items);
        }
    }
}