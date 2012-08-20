using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cassette;

namespace App.Infrastructure.Amd
{
    public class ScriptReferenceParser
    {
        static readonly Regex ReferenceCommentRegex = new Regex(
            @"/// \s* <reference \s+ path \s* = \s* ""([^""]*)"" \s* />",
            RegexOptions.IgnorePatternWhitespace
            );

        public static IEnumerable<string> ParseReferences(IAsset asset)
        {
            using (var reader = new StreamReader(asset.OpenStream()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = ReferenceCommentRegex.Match(line);
                    if (match.Success)
                    {
                        yield return EnsureAbsolutePath(match.Groups[1].Value, asset.Path);
                    }
                }
            }
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