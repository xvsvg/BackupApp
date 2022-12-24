using Backups.Extra.Core.Logger.Contracts;

namespace Backups.Extra.Core.Logger.Implementations;

public class ConsoleLogger : IBackupLogger
{
    public ConsoleLogger(bool enableDateTimePrefix = false)
    {
        EnableDatePrefix = enableDateTimePrefix;
    }

    public bool EnableDatePrefix { get; }

    public void Log(string v)
    {
        if (EnableDatePrefix)
            WriteDatePrefix();

        Console.WriteLine(v);
    }

    private void WriteDatePrefix()
    {
        Console.WriteLine($"[{DateTime.Now}]");
    }
}