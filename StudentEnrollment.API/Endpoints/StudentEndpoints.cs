using AutoMapper;

using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Models.Student;

namespace StudentEnrollment.API.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var router = routes.MapGroup("/api/students").WithTags(nameof(Student));

        router.MapGet("/", async (IStudentRepository repository, DataContext db, IMapper mapper) =>
        {
            var students = await repository.GetAllAsync();
            return mapper.Map<List<StudentDto>>(students);
        })
        .WithName("GetAllStudents")
        .WithOpenApi();

        router.MapGet("/{id}", async Task<Results<Ok<StudentDto>, NotFound>> (int id, IStudentRepository repository, DataContext db, IMapper mapper) =>
        {
            return await repository.GetAsync(id)
                is Student model
                    ? TypedResults.Ok(mapper.Map<StudentDto>(model))
                    : TypedResults.NotFound();
        })
        .WithName("GetStudentById")
        .WithOpenApi();

        router.MapGet("/detail/{id}", async Task<Results<Ok<StudentDetailsDto>, NotFound>> (int id, IStudentRepository repository, DataContext db, IMapper mapper) =>
        {
            return await repository.GetStudentDetails(id)
                is Student model
                    ? TypedResults.Ok(mapper.Map<StudentDetailsDto>(model))
                    : TypedResults.NotFound();
        })
        .WithName("GetStudentDetail")
        .WithOpenApi();

        router.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, IStudentRepository repository, StudentDto student, DataContext db, IMapper mapper) =>
        {
            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            mapper.Map(student, foundModel);
            //db.Update(student);
            await repository.UpdateAsync(foundModel);

            return TypedResults.NoContent();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        router.MapPost("/", async (CreateStudentDto student, IStudentRepository repository, DataContext db, IMapper mapper) =>
        {
            var model = mapper.Map<Student>(student);
            db.Students.Add(model);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/students/{model.Id}", model);
        })
        .WithName("CreateStudent")
        .WithOpenApi();

        router.MapDelete("/{id}", async Task<Results<Ok<Student>, NoContent, NotFound>> (int id, IStudentRepository repository, DataContext db) =>
        {
            //if (await db.Students.FindAsync(id) is Student student)
            //{
            //    db.Students.Remove(student);
            //    await db.SaveChangesAsync();
            //    return TypedResults.Ok(student);
            //}

            return await repository.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi();
    }
}
