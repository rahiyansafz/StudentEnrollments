namespace StudentEnrollment.Data.Entities;
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime ModifiedDateTime { get; set; } = DateTime.Now;
    public string? ModifiedBy { get; set; }
}
