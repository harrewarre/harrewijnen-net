using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Blog.Code;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSingleton<IEventLogger, EventLogger>();

builder.Services.AddSingleton<IPostResolver>(provider =>
{
    var hostingEnvironment = provider.GetService<IWebHostEnvironment>();
    return new PostResolver(hostingEnvironment.WebRootPath, provider.GetService<IEventLogger>());
});


builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseStatusCodePagesWithRedirects("/error?code={0}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();