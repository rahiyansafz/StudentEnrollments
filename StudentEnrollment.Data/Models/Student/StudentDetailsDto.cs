using StudentEnrollment.Data.Models.Course;

namespace StudentEnrollment.Data.Models.Student;
public class StudentDetailsDto : CreateStudentDto
{
    public List<CourseDto> Courses { get; set; } = new();
}