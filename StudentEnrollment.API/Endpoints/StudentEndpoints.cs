using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.API.Filters;
using StudentEnrollment.API.Services;
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

        route.MapPut("/{id}", async Task<Results<NotFound, NoContent, BadRequest<IDictionary<string, string[]>>>> (int id, StudentDto student, IStudentRepository repository, IMapper mapper, IValidator<StudentDto> validator, IFileUpload fileUpload) =>
        {
            var modelState = await validator.ValidateAsync(student);

            if (!modelState.IsValid)
                return TypedResults.BadRequest(modelState.ToDictionary());

            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            mapper.Map(student, foundModel);

            if (student.ProfilePicture is not null)
                foundModel.Picture = fileUpload.UploadStudentFile(student.ProfilePicture, student.OriginalFileName);

            await repository.UpdateAsync(foundModel);
            return TypedResults.NoContent();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        route.MapPost("/", async (CreateStudentDto student, IStudentRepository repository, IMapper mapper, IValidator<CreateStudentDto> validator, IFileUpload fileUpload) =>
        {
            var modelState = await validator.ValidateAsync(student);

            if (!modelState.IsValid)
                return Results.BadRequest(modelState.ToDictionary());

            var model = mapper.Map<Student>(student);
            model.Picture = fileUpload.UploadStudentFile(student.ProfilePicture, student.OriginalFileName);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/students/{model.Id}", model);
        })
        .AddEndpointFilter<ValidatationFilter<CreateStudentDto>>()
        .AddEndpointFilter<LoggingFilter>()
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
