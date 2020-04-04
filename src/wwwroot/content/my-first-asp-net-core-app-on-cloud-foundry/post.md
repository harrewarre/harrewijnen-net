# My first ASP.NET Core app on Cloud Foundry

Today I tried to deploy a .NET Core web app to a Pivotal Cloud Foundry platform that my [ITQ](http://www.itq.nl) buddy [Ruurd](http://www.ruurdkeizer.com) set up for us to tinker with. The app itself is a very basic ASP.NET MVC app built with the new ASP.NET Core framework to make it cross-platform. It being cross platform makes it very easy to deploy on something like Cloud Foundry since there is no dependency on Windows or the full .NET Framework anymore.

There were some bumps along the way though and here is what I learned.

The post assumes some basic Cloud Foundry knowledge so YMMV as far as this post goes.

## Buildpacks

Before Cloud Foundry can run an app it must be able to understand how to compile and run said app. There are some default buildpacks for Java and such but there is no default buildpack for ASP.NET 5/Core. Luckily there is one available over at the [Cloud Foundry community github](https://github.com/cloudfoundry-community/asp.net5-buildpack).

This takes care of building and running the app. One thing still puzzles me. I added the buildpack to my `manifest.yml` but CF did not pick up on it during the `cf push`. Setting the buildpack on the push command had the desired effect: `cf push -b https://github.com/cloudfoundry-community/asp.net5-buildpack.git` and CF downloaded the required components to handle my .NET Core app. 

There must still be something I'm forgetting or doing different from what is expected, but it won't work if I stick the buildpack in the manifest. If anyone knows why, please enlighten me :-)

## Pushing the wrong things

When I first pushed I noticed it was uploading ~8200 files. That can't be right. I forgot to add a `.cfignore` file to exclude things like my `node_modules` folder. It works just like a .gitignore file and so I copy-pasted my .gitignore file to a new `.cfignore` file and tried again. I was down to 24 files now.

![Pusing the wrong things](http://s.pikabu.ru/post_img/2013/08/02/6/1375430904_1823370079.gif)

The `.cfignore` file is an important one because uploading the installed packages resulted in all sorts of failures. My new rule of thumb is to only upload files that I would commit, nothing else, the platform will handle it.

## Setting up the host

This last step took me the longest because the platform only told me that health checks were failing when the app was starting up. Documentation pointed me in the direction of the port being set dynamically.

I already had a command in my manifest which was the basic command to start the web app: `dnx web`. This however uses the settings you specify in your `project.json` file for the command (`server.url ...` in this case). 

Cloud Foundry will expose a port for you and the number can be found in the `PORT` environment variable. You also need to set the IP to 0.0.0.0 since no actual binding is required (CF will take care of it for you). So the url that app is running on becomes: `http://0.0.0.0:$PORT`.

The final piece is setting up the command to start the app. Since you can no longer use the `web` command name from the `project.json` file, we need to call the Kestrel server directly (which is what the command in your `project.json` is doing) so the command we give to the platform becomes `dnx Microsoft.AspNet.Server.Kestrel server.urls=http://0.0.0.0:$PORT`.

Having done all that should get you up and running with your ASP.NET 5/Core on Cloud Foundry.

###### TL;DR:
 - Use the correct buildpack.
   - Would not stick when using the manifest file.
   - Did work on the command line using `cf push -b <buildpack>`.
   - I don't know why.
 - Add a `.cfignore` file to your project.
    - Copy-Pasta of my `.gitignore`.
 - Add a command to the manifest that dynamically sets the IP and port number the app is running on.
    - Incorrect port results in health-check failure.
    - Make use of the `$PORT` environment variable provided by CF.
    - IP address set to 0.0.0.0.
