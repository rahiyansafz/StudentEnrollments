using Microsoft.EntityFrameworkCore;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Repositories;
public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(DataContext db) : base(db)
    {
    }

    public async Task<Course> GetStudentList(int? id)
    {
        var course = await _db.Courses
            .Include(q => q.Enrollments).ThenInclude(q => q.Student)
            .FirstOrDefaultAsync(q => q.Id == id);

        return course;
    }
}
