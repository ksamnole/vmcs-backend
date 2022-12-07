namespace VMCS.Core;

public class BaseEntity
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
    }
}