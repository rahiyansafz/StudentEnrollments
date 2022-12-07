using FluentValidation;

namespace StudentEnrollment.API.Filters;

public class ValidatationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidatationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Runs beforte endpoint code
        var arg = context.GetArgument<T>(0);

        var modelState = await _validator.ValidateAsync(arg);

        if (!modelState.IsValid)
            return Results.BadRequest(modelState.ToDictionary());

        var result = await next(context);

        return result;
    }
}
