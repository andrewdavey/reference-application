using System.Collections.Generic;
using System.Linq;

namespace App.Infrastructure.Cassette
{
    public class DebugModuleAssetWrapper
    {
        /// <summary>
        /// Wraps bare JavaScript source in a debug-module initialization function.
        /// </summary>
        /// <param name="originalJavaScript">The JavaScript to wrap.</param>
        /// <param name="modulePath">The path of the AMD module the JavaScript is part of.</param>
        /// <param name="imports">The imports variables, as pairs of aliases and module paths</param>
        /// <param name="exportedVariables">The variables, exported by the JavaScript, to be added to the module.</param>
        public string Wrap(string originalJavaScript, string modulePath, IEnumerable<Import> imports, IEnumerable<string> exportedVariables, int assetIndex)
        {
            originalJavaScript += "\r\n"; // Add newline in case the original script ends with a comment.

            return "debugModules['" + modulePath + "'][" + assetIndex + "] = function(module,deps){" +
                   ImportVars(imports) +
                   originalJavaScript +
                   ExportAssignments(exportedVariables) +
                   "};";
        }

        string ImportVars(IEnumerable<Import> imports)
        {
            if (imports.Any())
            {
                var assignments = imports.Select(
                    import => import.CreateAssignment("module", "deps")
                );
                return "var " + string.Join(", ", assignments) + ";";
            }
            else
            {
                return "";
            }
        }

        string ExportAssignments(IEnumerable<string> exportedVariables)
        {
            return string.Join("\r\n", exportedVariables.Select(v => string.Format("module.{0} = {0};", v)));
        }
    }
}