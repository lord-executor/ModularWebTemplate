using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace WebTemplate.ServerAspects.Spa;

public class SpaDefaultPageMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IFileProvider _fileProvider;
	private readonly IOptions<SpaDefaultPageConfig> _defaultPageOptions;

	public SpaDefaultPageMiddleware(
		RequestDelegate next,
		IOptions<StaticFileOptions> staticFileOptions,
		IOptions<SpaDefaultPageConfig> defaultPageOptions,
		IWebHostEnvironment hostingEnvironment)
	{
		_next = next;
		_fileProvider = staticFileOptions.Value.FileProvider ?? hostingEnvironment.WebRootFileProvider;
		_defaultPageOptions = defaultPageOptions;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// See https://github.com/dotnet/aspnetcore/blob/main/src/Middleware/Spa/SpaServices.Extensions/src/SpaDefaultPageMiddleware.cs
		// where this is mostly borrowed from - in combination with the Microsoft.AspNetCore.StaticFiles.DefaultFilesMiddleware code.
		if (HttpMethods.IsGet(context.Request.Method) && context.GetEndpoint() == null)
		{
			// StaticFileMiddleware comes _after_ this middleware, so we use the file provider that is used by
			// the static file middleware to figure out ahead of time if there is actually a file matching the
			// request path and if not, we just "re-target" the request to our default page.
			var fileInfo = _fileProvider.GetFileInfo(context.Request.Path);
			if (!fileInfo.Exists)
			{
				context.Request.Path = _defaultPageOptions.Value.DefaultPage;
			}
		}

		await _next(context);
	}
}
