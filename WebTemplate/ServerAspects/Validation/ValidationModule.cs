using FluentValidation;

namespace WebTemplate.ServerAspects.Validation;

/// <summary>
/// For minimal APIs, you can just add the <see cref="ValidationFilter{T}"/> to any mapped endpoint by doing something
/// like
/// <code>
///   .AddEndpointFilter&lt;ValidationFilter&lt;MyRequest&gt;&gt;()
/// </code>
/// </summary>
public class ValidationModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        // If you add a validator as described in https://github.com/FluentValidation/FluentValidation?tab=readme-ov-file
        // that validator should automatically be registered with this.
        ctx.Services.AddValidatorsFromAssemblyContaining(typeof(ValidationModule));
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
    }
}
