namespace VMCS.Core;

public class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
    }

    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
}