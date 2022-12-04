using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Contracts;
public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course> GetStudentList(int? id);
}
