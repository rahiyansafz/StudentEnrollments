namespace StudentEnrollment.Data.Entities;
public class Student : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Identity { get; set; }
    public string? Picture { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new();
}
