using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebTemplate.ServerAspects.Auth;

public class AuthModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        // GetRequiredSection throws an exception if the section is missing, so authOptions always has a value
        var authOptions = ctx.Configuration.GetRequiredSection(AuthSettings.SectionName).Get<AuthSettings>()!;
        if (string.IsNullOrEmpty(authOptions.Authority))
        {
            throw new AppConfigException("Missing Authority configuration Auth.Authority");
        }

        if (string.IsNullOrEmpty(authOptions.Audience))
        {
            throw new AppConfigException("Missing Audience configuration Auth.Audience");
        }

        ctx.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authOptions.Authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authOptions.Authority, ValidAudience = authOptions.Audience
                };
            });

        ctx.Services.AddAuthorization(authorizationOptions =>
        {
            foreach (var policyPair in authOptions.PolicyClaims)
            {
                authorizationOptions.AddPolicy(policyPair.Key,
                    policyBuilder => policyBuilder.RequireClaim("permissions", policyPair.Value));
            }
        });
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        // ALWAYS keep UseAuthentication and UseAuthorization together and explicit. While there is some fancy code
        // (see https://source.dot.net/#Microsoft.AspNetCore/WebApplicationBuilder.cs,441) that automatically adds the
        // "other" one if only one is configured explicitly, that fancy code inserts the corresponding middleware in a
        // pretty random place that WILL mess with the expected middleware ordering. Specifically, the automatically
        // added middleware would end up running _before_ the ForwardedHeadersMiddleware instead of _after_ it which
        // breaks the authentication redirects.
        ctx.App.UseAuthentication();
        ctx.App.UseAuthorization();
    }
}