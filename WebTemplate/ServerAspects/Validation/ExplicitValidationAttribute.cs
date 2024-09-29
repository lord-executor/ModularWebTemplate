using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebTemplate.ServerAspects.Validation;

/// <summary>
/// When applied to an MVC controller action, this attribute performs the same function as the
/// <see cref="ValidationFilter{T}"/> does for minimal APIs. It tries to find the first action argument matching
/// <typeparamref name="T"/> and validates it with the registered fluent validator before the action is actually
/// invoked.
/// </summary>
public class ExplicitValidationAttribute<T> : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            context.Result = new ObjectResult(HttpProblems.ValidationError($"No validator found for type {typeof(T).Name}"));
            return;
        }

        var entity = context.ActionArguments.Values
            .OfType<T>()
            .FirstOrDefault(a => a?.GetType() == typeof(T));
        if (entity is not null)
        {
            var validation = validator.Validate(entity);
            if (!validation.IsValid)
            {
                context.Result = new ObjectResult(HttpProblems.ValidationError(validation.ToDictionary()));
                return;
            }
            base.OnActionExecuting(context);
            return;
        }

        context.Result = new ObjectResult(HttpProblems.ValidationError("Could not find type to validate"));
    }
}
