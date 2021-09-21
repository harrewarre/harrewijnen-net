{
  "slug": "netcore-az-container-environment",
  "title": "Environment variables in an Azure Web App for Containers",
  "summary": "A little gotcha when using a .net core app in a container on Azure that needs environment variables.",
  "tags": [
    "docker",
    "containers",
    "dotnet core",
    "azure",
    "itq"
  ],
  "created": "2019-05-04T00:00:00"
}
---
# Environment variables in an Azure Web App for Containers

Here's a little gotcha I ran into. A .net core app I wrote needs to run inside a container and the target was a Web App for Containers on the Azure platform. No big deal so far.

The app also needs a few settings for it to do its work. We provide these via environment variables for maximum flexibility. After setting up some CI/CD stuff and deploying the app it wouldn't start because it wasn't loading the settings.

But why?? The [spec](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2#hierarchical-configuration-data) is pretty clear on how to write the keys for the settings in such a way that .net understands them.

![Settings](/content/netcore-az-container-environment/settings.png)

So the secret sauce to this problem is replacing `:` in the settings for your app with `__` (that's 2 underscores). I managed to get to this by messing around (there were some other settings with just 1 `_` as well ...) and I wasn't able to find an official source for this.

It probably has something to do with passing stuff on the command-line that launches the container and the arguments containing `:`-characters, I have no idea. Using `__` is what works right now 🙂