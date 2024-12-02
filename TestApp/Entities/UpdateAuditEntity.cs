namespace TestApp.Entities;

public abstract class UpdateAuditEntity : CreateAuditEntity
{
    public string UpdatedBy { get; set; } = null!;
    public DateTime UpdatedOn { get; set; }
}
