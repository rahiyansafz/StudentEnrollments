namespace StudentEnrollment.Data.Entities;
public class Student : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Identity { get; set; } = default!;
    public string Picture { get; set; } = default!;
    public List<Enrollment> Enrollments { get; set; } = new();
}
