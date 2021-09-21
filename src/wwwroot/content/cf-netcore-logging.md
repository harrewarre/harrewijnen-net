{
    "title": "ASP.NET Core logging on Cloud Foundry",
    "summary": "Using ILogger to write logs on Cloud Foundry",
    "slug": "cf-netcore-logging",
    "tags": [
        "cloud foundry",
        "asp.net core",
        "dotnet core",
        "logging",
        "itq"
    ],
    "created": "2019-01-10"
}
---
# ASP.NET Core logging on Cloud Foundry

All apps I've ever worked on have some sort of logging going on for various reasons, mostly to keep track of whats going on or debugging, but it's all the same. Logs are needed to see what your code is up to.

Cloud Foundry [is very explicit](https://docs.cloudfoundry.org/devguide/deploy-apps/streaming-logs.html#writing) about *how* an app should write its logs. As you can see in the documentation, apps must write to `stdout` or `stderr`.

For C# that means we can use the static methods on the `Console` to write our logs. Couldn't be easier. But... now our code is littered with these `Console.Write...` lines all over the place!

Ok, easy, wrap it up in a simple class with an interface and inject it. That's a better solution for sure, but .NET provides an easy to use, injectable `ILogger` we can take advantage of. Let's see how we can set it up for console logging and use it in our apps we want to run on Cloud Foundry.

The `ILogger` mentioned earlier can be configured in the same way the configuration is handled. In the `Main` method, were the host is built, we can call the `.ConfigureLogging(...)` method and get access to logging bits.

    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((logging) => {
                -- [access logging here! --
            })
            .UseStartup<Startup>();

Because we only want to log to the console, we start with removing any default (Console, Debug and EventSource) that might be present:

    logging.ClearProviders();

And we add the console provider:

    logging.AddConsole();

This setup will make the `ILogger` instance log only to the console. Using `ILogger` is just like using a regular service. It's already present in the services collection and can be injected without further configuration. Here's an example with the `ValuesController` from an untouched .NET Core WebAPI project:

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogTrace("--- trace");
            _logger.LogDebug("--- debug");
            _logger.LogInformation("--- info");
            _logger.LogWarning("--- warning");
            _logger.LogError("--- error");
            _logger.LogCritical("--- critical");

            return new string[] { "value1", "value2" };
        }

        ...
    }

Running this locally and calling the `/api/values` endpoint will show the following (and a whole lot more) output:

    trce: app.Controllers.ValuesController[0]
        --- trace
    dbug: app.Controllers.ValuesController[0]
        --- debug
    info: app.Controllers.ValuesController[0]
        --- info
    warn: app.Controllers.ValuesController[0]
        --- warning
    fail: app.Controllers.ValuesController[0]
        --- error
    crit: app.Controllers.ValuesController[0]
        --- critical

Keep in mind that to see ALL the output, you need to tell the logger to do so in `appsettings.*.json` by setting the desired log level! (You might want to keep it at Information, or even Warning)

    {
        "Logging": {
            "LogLevel": {
                "Default": "Trace"
            }
        }
    }

The built-in logger uses a type (in this case `ValuesController`) as the category as seen in the output. For more options and features you can check to full documentation to for logging in aspnet core [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.2).

When you `cf push` this to Cloud Foundry and call the same endpoint you'll see something like this:

    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT trce: app.Controllers.ValuesController[0]
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT       --- trace
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT dbug: app.Controllers.ValuesController[0]
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT       --- debug
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT info: app.Controllers.ValuesController[0]
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT       --- info
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT warn: app.Controllers.ValuesController[0]
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT       --- warning
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT fail: app.Controllers.ValuesController[0]
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT       --- error
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT crit: app.Controllers.ValuesController[0]
    2019-01-10T11:43:19.35+0100 [APP/PROC/WEB/0] OUT       --- critical

So now you are all set up with proper logging in ASP.NET Core (2.2) that complies with the requirements set by the Cloud Foundry platform.