using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Models.Student;

namespace StudentEnrollment.API.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var route = routes.MapGroup("/api/students").WithTags(nameof(Student));

        route.MapGet("/", async (IStudentRepository repository, IMapper mapper) =>
        {
            var students = await repository.GetAllAsync();
            return mapper.Map<List<StudentDto>>(students);
        })
        .AllowAnonymous()
        .WithName("GetAllStudents")
        .WithOpenApi();

        route.MapGet("/{id}", async Task<Results<Ok<StudentDto>, NotFound>> (int id, IStudentRepository repository, IMapper mapper) =>
        {
            return await repository.GetAsync(id)
                is Student model
                    ? TypedResults.Ok(mapper.Map<StudentDto>(model))
                    : TypedResults.NotFound();
        })
        .AllowAnonymous()
        .WithName("GetStudentById")
        .WithOpenApi();

        route.MapGet("/detail/{id}", async Task<Results<Ok<StudentDetailsDto>, NotFound>> (int id, IStudentRepository repository, IMapper mapper) =>
        {
            return await repository.GetStudentDetails(id)
                is Student model
                    ? TypedResults.Ok(mapper.Map<StudentDetailsDto>(model))
                    : TypedResults.NotFound();
        })
        .WithName("GetStudentDetail")
        .WithOpenApi();

        route.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, IStudentRepository repository, StudentDto student, IMapper mapper) =>
        {
            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            mapper.Map(student, foundModel);
            await repository.UpdateAsync(foundModel);

            return TypedResults.NoContent();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        route.MapPost("/", async (CreateStudentDto student, IStudentRepository repository, IMapper mapper) =>
        {
            var model = mapper.Map<Student>(student);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/students/{model.Id}", model);
        })
        .WithName("CreateStudent")
        .WithOpenApi();

        route.MapDelete("/{id}", [Authorize(Roles = "Administrator")] async Task<Results<Ok<Student>, NoContent, NotFound>> (int id, IStudentRepository repository) =>
        {
            return await repository.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi();
    }
}
