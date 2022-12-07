using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Models.Enrollment;

namespace StudentEnrollment.API.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var route = routes.MapGroup("/api/enrollments").WithTags(nameof(Enrollment));

        route.MapGet("/", async (IEnrollmentRepository repository, IMapper mapper) =>
        {
            var enrollments = await repository.GetAllAsync();
            return mapper.Map<List<EnrollmentDto>>(enrollments);
        })
        .AllowAnonymous()
        .WithName("GetAllEnrollments")
        .WithOpenApi();

        route.MapGet("/{id}", async Task<Results<Ok<EnrollmentDto>, NotFound>> (int id, IEnrollmentRepository repository, IMapper mapper) =>
        {
            return await repository.GetAsync(id)
                is Enrollment model
                    ? TypedResults.Ok(mapper.Map<EnrollmentDto>(model))
                    : TypedResults.NotFound();
        })
        .AllowAnonymous()
        .WithName("GetEnrollmentById")
        .WithOpenApi();

        route.MapPut("/{id}", async Task<Results<NotFound, NoContent, BadRequest<IDictionary<string, string[]>>>> (int id, EnrollmentDto enrollment, IEnrollmentRepository repository, IMapper mapper, IValidator<EnrollmentDto> validator) =>
        {
            var modelState = await validator.ValidateAsync(enrollment);

            if (!modelState.IsValid)
                return TypedResults.BadRequest(modelState.ToDictionary());

            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            mapper.Map(enrollment, foundModel);
            await repository.UpdateAsync(foundModel);

            return TypedResults.NoContent();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi();

        route.MapPost("/", async (CreateEnrollmentDto enrollment, IEnrollmentRepository repository, IMapper mapper, IValidator<CreateEnrollmentDto> validator) =>
        {
            var modelState = await validator.ValidateAsync(enrollment);

            if (!modelState.IsValid)
                return Results.BadRequest(modelState.ToDictionary());

            var model = mapper.Map<Enrollment>(enrollment);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/enrollments/{model.Id}", model);
        })
        .WithName("CreateEnrollment")
        .WithOpenApi();

        route.MapDelete("/{id}", [Authorize(Roles = "Administrator")] async Task<Results<Ok<Enrollment>, NoContent, NotFound>> (int id, IEnrollmentRepository repository) =>
        {
            return await repository.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi();
    }
}
