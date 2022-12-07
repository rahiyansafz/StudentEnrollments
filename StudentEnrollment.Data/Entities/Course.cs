namespace StudentEnrollment.Data.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; } = default!;
    public int Credits { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new();
}
