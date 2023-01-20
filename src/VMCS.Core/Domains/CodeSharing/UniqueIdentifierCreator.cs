namespace VMCS.Core.Domains.CodeSharing;

public class UniqueIdentifierCreator
{
    private int id;

    public int GetUniqueIdentifier()
    {
        return id++;
    }
}