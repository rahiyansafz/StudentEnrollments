using FluentValidation;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Models.Course;
using StudentEnrollment.Data.Models.Student;

namespace StudentEnrollment.Data.Models.Enrollment;
public class EnrollmentDto : CreateEnrollmentDto
{
    public virtual CourseDto? Course { get; set; }
    public virtual StudentDto? Student { get; set; }
}

public class EnrollmentValidator : AbstractValidator<EnrollmentDto>
{
    public EnrollmentValidator(ICourseRepository courseRepository, IStudentRepository studentRepository)
    {
        Include(new CreateEnrollmentValidator(courseRepository, studentRepository));
    }
}