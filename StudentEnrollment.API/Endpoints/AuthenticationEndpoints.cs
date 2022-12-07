using FluentValidation;

using Microsoft.AspNetCore.Http.HttpResults;

using StudentEnrollment.API.Filters;
using StudentEnrollment.API.Services;
using StudentEnrollment.Data.Models;
using StudentEnrollment.Data.Models.Authentication;

namespace StudentEnrollment.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
    {
        var route = routes.MapGroup("/api/authenticate").WithTags("Authentication");

        route.MapPost("/login", async Task<Results<Ok<AuthResponse>
            , UnauthorizedHttpResult, BadRequest<IDictionary<string, string[]>>>> (Login login, IAuthManager authManager, IValidator<Login> validator) =>
        {
            var response = await authManager.Login(login);

            if (response is null)
                return TypedResults.Unauthorized();

            return TypedResults.Ok(response);
        })
        .AddEndpointFilter<ValidatationFilter<Login>>()
        .AllowAnonymous()
        .WithName("Login")
        .WithOpenApi();

        route.MapPost("/register", async Task<Results<Ok, BadRequest<List<ErrorResponse>>>> (Register register, IAuthManager authManager, IValidator<Register> validator) =>
        {
            var response = await authManager.Register(register);

            if (!response.Any())
                return TypedResults.Ok();

            var errors = new List<ErrorResponse>();
            foreach (var error in response)
            {
                errors.Add(new ErrorResponse
                {
                    Code = error.Code,
                    Description = error.Description
                });
            }

            return TypedResults.BadRequest(errors);
        })
        .AddEndpointFilter<ValidatationFilter<Register>>()
        .AllowAnonymous()
        .WithName("Register")
        .WithOpenApi();
    }
}
