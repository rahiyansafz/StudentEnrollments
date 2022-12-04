using Microsoft.AspNetCore.Identity;

namespace StudentEnrollment.Data.Entities;
public class SchoolUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
}