﻿namespace StudentEnrollment.Data.Models.Student;
public class CreateStudentDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Identity { get; set; }
    public string? Picture { get; set; }
}
