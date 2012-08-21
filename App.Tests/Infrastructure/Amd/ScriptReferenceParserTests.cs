using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cassette;
using Moq;
using Xunit;

namespace App.Infrastructure.Amd
{
    public class ScriptReferenceParserTests
    {
        string path, content;
        IEnumerable<string> parsedReferences;

        [Fact]
        public void ParseEmptyFileReturnsNoReferences()
        {
            GivenAssetPath("~/asset.js");
            WithContent("");
            WhenParse();
            ThenReferencesEqual(new string[0]);
        }

        [Fact]
        public void ParseReferenceWithSingleReferenceCommentReturnsPathAttributeValue()
        {
            GivenAssetPath("~/asset.js");
            WithContent("/// <reference path=\"~/example.js\"/>");
            WhenParse();
            ThenReferencesEqual(new[] {"~/example.js"});
        }

        [Fact]
        public void ParseReferenceWithTwoReferenceCommentsReturnsPathAttributeValues()
        {
            GivenAssetPath("~/asset.js");
            WithContent(@"
/// <reference path=""~/example1.js""/>
/// <reference path=""~/example2.js""/>
");
            WhenParse();
            ThenReferencesEqual(new[] { "~/example1.js", "~/example2.js" });
        }

        [Fact]
        public void ParseRelativeReferenceReturnsAbsolutePath()
        {
            GivenAssetPath("~/asset.js");
            WithContent(@"/// <reference path=""example.js""/>");
            WhenParse();
            ThenReferencesEqual(new[] { "~/example.js" });
        }

        [Fact]
        public void ParseParentRelativeReferenceReturnsAbsolutePath()
        {
            GivenAssetPath("~/sub/asset.js");
            WithContent(@"/// <reference path=""../example.js""/>");
            WhenParse();
            ThenReferencesEqual(new[] { "~/example.js" });
        }

        [Fact]
        public void ParseParentRelativeReferenceToSubPathReturnsAbsolutePath()
        {
            GivenAssetPath("~/sub1/asset.js");
            WithContent(@"/// <reference path=""../sub2/example.js""/>");
            WhenParse();
            ThenReferencesEqual(new[] { "~/sub2/example.js" });
        }

        void GivenAssetPath(string path)
        {
            this.path = path;
        }

        void WithContent(string content)
        {
            this.content = content;
        }

        void WhenParse()
        {
            parsedReferences = ScriptReferenceParser.ParseReferences(content, path).ToArray();
        }

        void ThenReferencesEqual(IEnumerable<string> expectedReferences)
        {
            Assert.Equal(expectedReferences, parsedReferences);
        }
    }
}