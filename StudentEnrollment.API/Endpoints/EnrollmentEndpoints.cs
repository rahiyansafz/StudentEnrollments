using AutoMapper;

using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Models.Enrollment;

namespace StudentEnrollment.API.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var router = routes.MapGroup("/api/enrollments").WithTags(nameof(Enrollment));

        router.MapGet("/", async (IEnrollmentRepository repository, IMapper mapper) =>
        {
            var enrollments = await repository.GetAllAsync();
            return mapper.Map<List<EnrollmentDto>>(enrollments);
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi();

        router.MapGet("/{id}", async Task<Results<Ok<EnrollmentDto>, NotFound>> (int id, IEnrollmentRepository repository, IMapper mapper) =>
        {
            return await repository.GetAsync(id)
                is Enrollment model
                    ? TypedResults.Ok(mapper.Map<EnrollmentDto>(model))
                    : TypedResults.NotFound();
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi();

        router.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, EnrollmentDto enrollment, IEnrollmentRepository repository, IMapper mapper) =>
        {
            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            mapper.Map(enrollment, foundModel);
            //db.Update(enrollment);
            await repository.UpdateAsync(foundModel);

            return TypedResults.NoContent();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi();

        router.MapPost("/", async (CreateEnrollmentDto enrollment, IEnrollmentRepository repository, IMapper mapper) =>
        {
            var model = mapper.Map<Enrollment>(enrollment);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/enrollments/{model.Id}", model);
        })
        .WithName("CreateEnrollment")
        .WithOpenApi();

        router.MapDelete("/{id}", async Task<Results<Ok<Enrollment>, NoContent, NotFound>> (int id, IEnrollmentRepository repository) =>
        {
            //if (await db.Enrollments.FindAsync(id) is Enrollment enrollment)
            //{
            //    repository.Enrollments.Remove(enrollment);
            //    await db.SaveChangesAsync();
            //    return TypedResults.NoContent();
            //}

            return await repository.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi();
    }
}
