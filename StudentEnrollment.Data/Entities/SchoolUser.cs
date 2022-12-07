using Microsoft.AspNetCore.Identity;

namespace StudentEnrollment.Data.Entities;
public class SchoolUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
}