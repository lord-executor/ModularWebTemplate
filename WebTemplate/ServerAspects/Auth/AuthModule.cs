using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebTemplate.ServerAspects.Auth;

public class AuthModule : IAppConfigurationModule
{
	public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
	{
		// GetRequiredSection throws an exception if the section is missing, so authOptions always has a value
		var authOptions = config.GetRequiredSection(AuthSettings.SectionName).Get<AuthSettings>()!;
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = authOptions.Authority;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = authOptions.Authority,
					ValidAudience = authOptions.Audience
				};
			});

		services.AddAuthorization(authorizationOptions =>
		{
			foreach (var policyPair in authOptions.PolicyClaims)
			{
				authorizationOptions.AddPolicy(policyPair.Key,
					policyBuilder => policyBuilder.RequireClaim("permissions", policyPair.Value));
			}
		});
	}

	public void ConfigureApplication(WebApplication app)
	{
		app.UseAuthorization();
	}
}
