using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Infrastructure.Amd
{
    public static class ScriptReferenceParser
    {
        static readonly Regex ReferenceCommentRegex = new Regex(
            @"/// \s* <reference \s+ path \s* = \s* ""([^""]*)"" \s* />",
            RegexOptions.IgnorePatternWhitespace
            );

        public static IEnumerable<string> ParseReferences(string script, string scriptPath)
        {
            return ReferenceCommentRegex
                .Matches(script)
                .Cast<Match>()
                .Select(match => EnsureAbsolutePath(match.Groups[1].Value, scriptPath));
        }

        static string EnsureAbsolutePath(string path, string assetPath)
        {
            if (path.StartsWith("~")) return path;
            
            var stack = new Stack<string>(assetPath.Split('/'));
            stack.Pop(); // Remove the asset filename

            var parts = path.Split('/');
            foreach (var part in parts)
            {
                if (part == "..")
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push(part);
                }
            }
            return string.Join("/", stack.Reverse());
        }
    }
}