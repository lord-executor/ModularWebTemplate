using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebTemplate.ServerAspects.Auth;

public class AuthModule : IAppConfigurationModule
{
	public void ConfigureServices(ServiceConfigurationContext ctx)
	{
		// GetRequiredSection throws an exception if the section is missing, so authOptions always has a value
		var authOptions = ctx.Configuration.GetRequiredSection(AuthSettings.SectionName).Get<AuthSettings>()!;
        ctx.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = authOptions.Authority;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = authOptions.Authority,
					ValidAudience = authOptions.Audience
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
		ctx.App.UseAuthorization();
	}
}
