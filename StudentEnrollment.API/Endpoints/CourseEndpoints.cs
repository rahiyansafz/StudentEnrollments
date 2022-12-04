using AutoMapper;

using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.Data.Contracts;
using StudentEnrollment.Data.Data;
using StudentEnrollment.Data.Entities;
using StudentEnrollment.Data.Models.Course;

namespace StudentEnrollment.API.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var router = routes.MapGroup("/api/courses").WithTags(nameof(Course));

        router.MapGet("/", async (ICourseRepository repository, DataContext db, IMapper mapper) =>
        {
            var courses = await repository.GetAllAsync();
            return mapper.Map<List<CourseDto>>(courses);
        })
        .WithName("GetAllCourses")
        .WithOpenApi();

        router.MapGet("/{id}", async Task<Results<Ok<CourseDto>, NotFound>> (int id, ICourseRepository repository, DataContext db, IMapper mapper) =>
        {
            return await repository.GetAsync(id)
                is Course model
                    ? TypedResults.Ok(mapper.Map<CourseDto>(model))
                    : TypedResults.NotFound();
        })
        .WithName("GetCourseById")
        .WithOpenApi();

        router.MapGet("/detail/{id}", async Task<Results<Ok<CourseDetailsDto>, NotFound>> (int id, ICourseRepository repository, DataContext db, IMapper mapper) =>
        {
            return await repository.GetStudentList(id)
                is Course model
                    ? TypedResults.Ok(mapper.Map<CourseDetailsDto>(model))
                    : TypedResults.NotFound();
        })
        .WithName("GetCourseDetail")
        .WithOpenApi();

        router.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, CourseDto course, ICourseRepository repository, DataContext db, IMapper mapper) =>
        {
            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            //var foundModel = await db.Courses.AnyAsync(c => c.Id == course.Id);
            //if (!foundModel) return TypedResults.NotFound();

            mapper.Map(course, foundModel);
            //db.Update(course);
            await repository.UpdateAsync(foundModel);

            return TypedResults.NoContent();
        })
        .WithName("UpdateCourse")
        .WithOpenApi();

        router.MapPost("/", async (CreateCourseDto course, ICourseRepository repository, DataContext db, IMapper mapper) =>
        {
            var model = mapper.Map<Course>(course);
            db.Courses.Add(model);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/courses/{model.Id}", model);
        })
        .WithName("CreateCourse")
        .WithOpenApi();

        router.MapDelete("/{id}", async Task<Results<Ok<Course>, NoContent, NotFound>> (int id, ICourseRepository repository, DataContext db) =>
        {
            //if (await db.Courses.FindAsync(id) is Course course)
            //{
            //    db.Courses.Remove(course);
            //    await db.SaveChangesAsync();
            //    return TypedResults.Ok(course);
            //}

            return await repository.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteCourse")
        .WithOpenApi();
    }
}
