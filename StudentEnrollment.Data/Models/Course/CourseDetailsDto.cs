using StudentEnrollment.Data.Models.Student;

namespace StudentEnrollment.Data.Models.Course;

public class CourseDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public int Credits { get; set; }

    public List<StudentDto> Students { get; set; } = new();
}