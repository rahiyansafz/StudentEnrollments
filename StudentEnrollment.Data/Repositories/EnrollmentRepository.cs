using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;

namespace StudentEnrollment.Data.Repositories;
public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(DataContext db) : base(db)
    {
    }
}
