using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Blog.Code;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Blog.FileSystemSupport;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFileSystem>(p => {
    var hostingEnvironment = p.GetService<IWebHostEnvironment>();
    return new FileSystem(hostingEnvironment.WebRootPath);
});

builder.Services.AddSingleton<IPostResolver, PostResolver>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ".well-known")),
    RequestPath = "/.well-known",
    ServeUnknownFileTypes = true,
    DefaultContentType = "text/plain"
});

app.UseRouting();

app.UseStatusCodePagesWithRedirects("/error?code={0}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();