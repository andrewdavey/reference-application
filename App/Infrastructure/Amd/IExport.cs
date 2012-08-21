using System.Collections.Generic;

namespace App.Infrastructure.Amd
{
    public interface IExport
    {
        string Identifier { get; }
        IEnumerable<string> Aliases { get; }
    }
}