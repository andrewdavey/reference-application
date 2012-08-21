using System.Collections.Generic;

namespace App.Infrastructure.Amd
{
    public class ObjectExport : IExport
    {
        public ObjectExport(string identifier, IEnumerable<string> aliases)
        {
            Identifier = identifier;
            Aliases = aliases;
        }

        public string Identifier { get; private set; }
        public IEnumerable<string> Aliases { get; private set; }
    }
}