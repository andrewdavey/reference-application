using System;
using System.Text.RegularExpressions;
using Cassette;
using Cassette.Scripts;

namespace App.Infrastructure.Cassette
{
    public sealed class AmdModule
    {
        readonly string path;
        readonly string alias;

        /// <summary>
        /// Creates a new <see cref="AmdModule"/> from the assets contained in the given <see cref="ScriptBundle"/>.
        /// </summary>
        public AmdModule(ScriptBundle bundle)
        {
            if (bundle == null) throw new ArgumentNullException("bundle");
            path = bundle.Path.Substring(2);
        }

        /// <summary>
        /// Creates a new <see cref="AmdModule"/> from a single asset that defines a module.
        /// </summary>
        /// <param name="asset">The asset that defines an AMD module.</param>
        /// <param name="alias">A JavaScript variable used to refer to the module. For example, "$" is the alias of jQuery.</param>
        public AmdModule(IAsset asset, string alias)
        {
            if (asset == null) throw new ArgumentNullException("asset");
            if (alias == null) throw new ArgumentNullException("alias");

            var match = Regex.Match(asset.Path, @"^~/(.*)\.[a-z]+$");
            path = match.Groups[1].Value;

            this.alias = alias;
        }

        public string Path
        {
            get { return path; }
        }

        public string Alias
        {
            get { return alias; }
        }

        bool Equals(AmdModule other)
        {
            return string.Equals(path, other.path) && string.Equals(alias, other.alias);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is AmdModule && Equals((AmdModule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (path.GetHashCode()*397) ^ (alias != null ? alias.GetHashCode() : 0);
            }
        }

    }
}