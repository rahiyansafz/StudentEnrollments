namespace StudentEnrollment.Data.Entities;
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime ModifiedDateTime { get; set; }
    public string? ModifiedBy { get; set; }
}
