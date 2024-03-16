using FluentValidation;

namespace WebTemplate.ServerAspects.Validation;

/// <summary>
/// Since the FluentValidation.AspNetCore package is no longer supported and the documentation under
/// https://docs.fluentvalidation.net/en/latest/aspnet.html?highlight=asp.net%20core#using-a-filter suggests using
/// a custom filter, this is what we are doing.
///
/// The code is heavily inspired by a "coding short" video by Shawn Wildermuth but customized a bit.
/// https://www.youtube.com/watch?v=_S-r6SxLGn4
/// </summary>
/// <typeparam name="T">The object type that should be validated.</typeparam>
public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        var validator = ctx.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            return Results.Problem($"No validator found for type {typeof(T).Name}");
        }
        
        var entity = ctx.Arguments
            .OfType<T>()
            .FirstOrDefault(a => a?.GetType() == typeof(T));
        if (entity is not null)
        {
            var validation = await validator.ValidateAsync(entity);
            if (validation.IsValid)
            {
                return await next(ctx);
            }
            return Results.ValidationProblem(validation.ToDictionary());
        }
        
        return Results.Problem("Could not find type to validate");
    }
}