using System.Collections.Generic;

namespace App.Infrastructure.Amd
{
    public class SingleValueExport : IExport
    {
        public SingleValueExport(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; private set; }

        public IEnumerable<string> Aliases
        {
            get { yield break; }
        }
    }
}