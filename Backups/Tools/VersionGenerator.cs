namespace Backups.Tools;

public class VersionGenerator
{
    private int _version = 0;

    public int Generate()
        => _version++;
}
