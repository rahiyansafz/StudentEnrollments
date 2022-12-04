using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Repositories;
public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(DataContext db) : base(db)
    {
    }

    public async Task<Student> GetStudentDetails(int? id)
    {
        var student = await _db.Students
            .Include(q => q.Enrollments).ThenInclude(q => q.Course)
            .FirstOrDefaultAsync(q => q.Id == id);

        return student;
    }
}
