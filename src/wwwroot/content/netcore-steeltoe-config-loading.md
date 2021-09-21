{
  "slug": "netcore-steeltoe-config-loading",
  "title": "Reading Cloud Foundry app configuration using SteelToe in ASP.NET Core",
  "summary": "A look at loading configuration using SteelToe and making it injectable",
  "tags": [
    "cloud foundry",
    "steeltoe",
    "aspnet core",
    "dotnetcore",
    "itq"
  ],
  "created": "2019-01-11T00:00:00"
}
---
# Reading Cloud Foundry app configuration using SteelToe in ASP.NET Core

I've been playing around with the [SteelToe](https://steeltoe.io/) framework for .net apps on Cloud Foundry a bit and it's been nice overall (I'll probably write a few more posts about it at some point 😊). While playing around with it the configuration part of it I wasn't really liking the way settings can be read from the main configuration.

Let's start with a really short primer on how the configuration system works. Cloud Foundry apps can be bound to services, doing so adds a reference to the bound service in the environment variables of the app that was bound. You can take a look at what is provided to an applications environment by running

    cf env <app-name>

For this example we're interested in the `VCAP_SERVICES` section, because it contains a user provided service that I bound to this app:

    {
        "VCAP_SERVICES": {
            "user-provided": [
                {
                    "binding_name": null,
                    "credentials": {
                        "connectionString": "<...>"
                    },
                    "instance_name": "content-storage",
                    "label": "user-provided",
                    "name": "content-storage",
                    "syslog_drain_url": "",
                    "tags": [],
                    "volume_mounts": []
                }
            ]
        }
    }

SteelToe has some extension methods than can help you load these into dictionaries to be used in your code. To load these settings, you need to do two things (this is for .NET Core 2.2). The first thing is add the Cloud Foundry configuration provider:

    WebHost.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) => {
            ...
            config.AddCloudFoundry();
            ...
        })
        .UseStartup<Startup>();

The next step is to let SteelToe add the settings to the service collection (in `Startup.cs`) as instances of `IOption<T>`.

    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.ConfigureCloudFoundryOptions(Configuration);
        ...
    }

It will add two instances that contain different things:

    IOptions<CloudFoundryApplicationOptions>
    IOptions<CloudFoundryServicesOptions>

Because we're dealing with a service, we need `IOptions<CloudFoundryServicesOptions>`.

When you inject the `IOptions<CloudFoundryServicesOptions>` you can dig around in the dictionaries it provides to get the settings you want, but that doesn't look very nice:

    _connectionString = serviceConfig.Value.Services["user-provided"].First(s => s.Name == "content-storage").Credentials["connectionString"].Value;

I would rather inject a simpler object containing only the connectionstring, so here's a way to do this in the `ConfigureServices(...)` method in `Startup.cs`:

<small>A little side note: There is some discussion around the usefulnes of injecting `IOptions<T>` instead of just plain objects containing the properties. For this example I'm adding more `IOptions<T>` to the service collection you can easily adjust this to add POCOs instead of `IOptions<T>`.</small>

Add the following to the `ConfigureServices(...)` method in `Startup.cs`.

    services.AddSingleton(provider =>
    {
        var cloudServiceConfig = Configuration.GetSection("vcap").Get<CloudFoundryServicesOptions>();
        var storageConnectionString = cloudServiceConfig.Services["user-provided"].First(s => s.Name == "content-storage").Credentials["connectionString"].Value;

        var storageConfig = new StorageConfig();
        storageConfig.ConnectionString = storageConnectionString;

        return Options.Create(storageConfig);
    });

So what is happening here? Let's walk through it.

First, we read back the configuration that was just added:

    var cloudServiceConfig = Configuration.GetSection("vcap").Get<CloudFoundryServicesOptions>();

`vcap` is the name of the root property in the environment JSON that holds all the configuration.

Then we fish out our connectionstring:

    var storageConnectionString = cloudServiceConfig.Services["user-provided"].First(s => s.Name == "content-storage").Credentials["connectionString"].Value;

And finally we create our object that hold the connectionstring and turn it into an instance of `Options<T>`:

    var storageConfig = new StorageConfig();
    storageConfig.ConnectionString = storageConnectionString;

    return Options.Create(storageConfig);

The returned instance is added as a singleton to the service collection. Now you can inject the storage configuration on its own instead of the entire CF service configuration and you can replace this:

    public StorageService(IOptions<CloudFoundryServicesOptions> serviceConfig)
    {   
        _connectionString = serviceConfig.Value.Services["user-provided"].First(s => s.Name == "content-storage").Credentials["connectionString"].Value;
        ...
    }


With this:

    public StorageService(IOptions<StorageConfig> storageConfig)
    {   
        _connectionString = storageConfig.Value.ConnectionString;
        ...
    }

Now all the digging through dictionaries can stay in the `ConfigureServices(...)` method and we can use simple objects to read the configuration! Much nicer 👌