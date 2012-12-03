using System;
using System.IO;
using Cassette;

namespace App.Infrastructure.Cassette
{
    public abstract class StringAssetTransformer : IAssetTransformer
    {
        public Func<Stream> Transform(Func<Stream> openSourceStream, IAsset asset)
        {
            return () =>
            {
                using (var stream = openSourceStream())
                using (var reader = new StreamReader(stream))
                {
                    var source = reader.ReadToEnd();
                    var output = Transform(source, asset);
                    return CreateStreamContainingString(output);
                }
            };
        }

        protected abstract string Transform(string source, IAsset asset);

        Stream CreateStreamContainingString(string output)
        {
            var outputStream = new MemoryStream();
            var writer = new StreamWriter(outputStream);
            writer.Write(output);
            writer.Flush();
            outputStream.Position = 0;
            return outputStream;
        }
    }
}