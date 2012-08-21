namespace App.Infrastructure.Amd
{
    public interface IAmdModule
    {
        string Path { get; }
        IExport Export { get; }
    }
}