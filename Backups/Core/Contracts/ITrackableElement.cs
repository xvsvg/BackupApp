namespace Backups.Contracts;

public interface ITrackableElement
{
    string ElementPath { get; }
    string Name { get; }

    void Accept(ITrackableElementVisitor visitor);
}
