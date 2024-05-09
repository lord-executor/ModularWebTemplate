# Modular Web Template

Have you ever noticed how the vast majority of "getting started" code samples for ASP.NET Core simply do not scale at all? How minimal APIs are very easy to get started with and then become very messy very quickly? How the configuration of the `WebApplication` builder creates code snippets that _should_ be together but aren't? How you end up solving the same basic problems of _integrating_ the same libraries in every new ASP.NET Core project?

If so, then you might be interested in this template. This project is a _template_ in the more traditional sense. While it _can_ be used to quickly get a **structured** project off the ground, its main purpose is to provide a much more complete code sample for an ASP.NET Core application that provides a maintainable structure.

At the core of the project is a _modular_ system of _aspects_ which are cross-cutting concerns of most web applications like validation, authentication, OpenAPI documentation, etc. These aspects are using a simple `IAppConfigurationModule` interface to encapsulate the necessary configuration in the form of _modules_ that are then registered in the `AppServer` which creates a much more readable and maintainable code structure.

The template can of course easily be customized by simply removing the things that are not needed or changing them to fit your needs. If the SPA module is not needed, remove it. If you are using a different authentication mechansim than JWT bearer tokens, change the code in the `AuthModule`.

## Features

- Authentication using JWT bearer tokens - see the `appSettings.json` for configuration
  - depends on [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
- CORS configuration for API endpoints that can be used from a separate or integrated SPA application
- More reasonable defaults for the JSON serialization configuration
- OpenAPI documentation for API endpoints using Swagger
  - depends on [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
  - depends on [Swashbuckle.AspNetCore.Annotations](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- Validation of incoming DTOs using [FluentValidation](https://docs.fluentvalidation.net/en/latest/aspnet.html)
  - depends on [FluentValidation.AspNetCore](https://github.com/FluentValidation/FluentValidation.AspNetCore)
- ASP.NET Core controllers configuration with custom injectable settings - see `appSettings.json`
- SPA routing with custom middleware that routes all unhandled requests to a central location like `/index.html` for Angular apps
- Automatic versioning based on git tags
  - depends on [GitInfo](https://github.com/devlooped/GitInfo)
- Status endpoint for version information and health check `/v1/status`

## Instantiate with `dotnet new ...`

The template can easily be used as a template with the `dotnet new` command. To do that, you first need to install it.

1. Clone the repository
2. In the terminal, naviate to the `WebTemplate` directory
3. Run `dotnet new install .` - add the `--force` flag to _update_ the installed template with a newer version of the source

After that, you can instantiate a new project like this.

```shell
dotnet new modweb -o MyNewProject
cd MyNewProject
git init
git add .
git commit -m "Initial commit with code from "https://github.com/lord-executor/ModularWebTemplate"
dotnet run
```