using Microsoft.AspNetCore.Mvc;

namespace WebTemplate;

/// <summary>
/// Problem types are taken from the unofficial registry of Smartbear https://problems-registry.smartbear.com/
/// but these can be replaced or extended with custom ones. Note that RFC 9457 (see
/// https://www.rfc-editor.org/rfc/rfc9457.txt) prefers resolvable URIs, but for custom use cases, non-resolvable
/// URIs like for example the TAG scheme (RFC 4151) can also be used.
///
/// There is one weird design issue when using the APS.NET core <see cref="ProblemDetails"/> class: It is defined
/// in the Microsoft.AspNetCore.Http.Abstractions assembly which no longer comes with its own NuGet package and is
/// only available as part of the full APS.NET Core SDK which is a dependency that is difficult to justify or even
/// properly "take" in a _library_ which can make things rather difficult when trying to share things like this class.
/// On top of that, there is the fact that there is "magic" functionality in ASP.NET Core that actually sets the
/// HTTP response status code based on <see cref="ProblemDetails.Status"/> which can be rather confusing and
/// unintuitive. If that is causing problems, then the solution is to simply make your own copy of the
/// https://source.dot.net/#Microsoft.AspNetCore.Http.Abstractions/ProblemDetails/ProblemDetails.cs,9fba0244fad2ef1b
/// class and when necessary, fill in the gaps with extension methods to convert to/from the ASP.NET Core version.
/// </summary>
public static class HttpProblems
{
    public const string GenericProblemType = "about:blank";
    public const string ValidationProblemType = "https://problems-registry.smartbear.com/validation-error";

    public static ProblemDetails ValidationError(string? detail, IDictionary<string, string[]>? errors)
    {
        return new ProblemDetails
        {
            Type = ValidationProblemType,
            Title = "Validation Error",
            Status = 422,
            Detail = detail,
            Extensions = errors == null
                ? new Dictionary<string, object?>()
                : new Dictionary<string, object?>
                {
                    ["errors"] = errors
                }
        };
    }

    public static ProblemDetails ValidationError(string detail)
    {
        return ValidationError(detail, null);
    }

    public static ProblemDetails ValidationError(IDictionary<string, string[]>? errors)
    {
        return ValidationError(null, errors);
    }

    public static ProblemDetails ServerError(string detail)
    {
        return new ProblemDetails
        {
            Type = GenericProblemType,
            Title = "Server Error",
            Status = 500,
            Detail = detail
        };
    }
}
