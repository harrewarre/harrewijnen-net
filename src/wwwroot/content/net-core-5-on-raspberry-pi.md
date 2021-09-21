{
    "slug": "net-core-5-on-raspberry-pi",
    "title": "Dotnet core 5 on a raspberry pi",
    "summary": "A quickstart on how to run and use dotnet core 5 on a Raspberry Pi",
    "tags": [
        "dotnet core",
        "raspberry",
        "linux"
    ],
    "created": "2021-01-10T00:00:00"
}
---
# dotnet core 5 on a raspberry pi

Here's a quick get-up-and-go style post on how to get dotnet core 5 usable on a Raspberry Pi (a R-Pi 3 Model B in my case).

We're doing all this over SSH because my goal was to run a small webapp on the Pi which is located in a cupboard somewhere in my house without a keyboard and mouse attached to it 😁
So, after you've connected to the Pi over SSH, the first step is to install the dotnet core SDK.

You can find all the dotnet binaries [here](https://dotnet.microsoft.com/download/dotnet/5.0) and the one we'll need for this is the Linux ARM64 version. Grab the download link you get from the site and run `wget` to download the file to your downloads directory.

     cd ~
     mkdir downloads
     cd downloads
     wget <the dotnet SDK download link>

Now that you have the file (name something like `dotnet-sdk-5.0.101-linux-arm.tar.gz`) you can extract it to some location you can easily find later. I've extract mine to `/usr/bin/dotnet`.

    `sudo tar zxf dotnet-sdk-5.0.101-linux-arm.tar.gz -C /usr/bin/dotnet`

(versions might have changed by now)

Now that the SDK is installed we can start using it but it's not part of the systems `PATH` yet so for ease of use can add it to the end of `.bashrc` so that the `dotnet` command is available everywhere.
Open your `.bashrc` file in a text editor, add the following lines to the end of it and save it.

    export DOTNET_ROOT=/usr/bin/dotnet
    export PATH=$PATH:/usr/bin/dotnet

The first one is needed to tell apps that are started from the command line where dotnet lives, the second adds the dotnet command to our PATH so we can call it from anywhere.

If you now run `dotnet --version` you should see something like this:

    pi@raspberrypi:~ $ dotnet --version
    5.0.101

An additional tool you might want to install is [LibMan](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-cli?view=aspnetcore-5.0), which can help you install javascript dependencies without relying on NPM.
To make sure LibMan works anywhere, we add it to `.bashrc` in the same way as the dotnet itself:

First install LibMan using the following command (full instructions are at the link above)

    dotnet tool install -g Microsoft.Web.LibraryManager.Cli

This will install additional tooling for dotnet into your home directory in a directory called `.dotnet/tools`. AFter the install completes, add the following to the end of your `.bashrc` to have the command available to you anywhere.

    export PATH=$PATH:/home/pi/.dotnet/tools

The only reason I wanted this was to **not** install Node/NPM but still be able to pull in client-side packages such as [SignalR](https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/introduction-to-signalr)

Having all this done you are now free to code and run dotnet apps on your Raspberry Pi.

## Hosting a webapp

Here's another handy thing to keep around. Since I had built a small web app (to monitor the power usage of my household - code [here](https://github.com/harrewarre/powman)) I also wanted it running forever on my Pi without me having to think about managing it. To monitor and manage our app we can use `systemd`. Systemd is the thing that starts all other things when the Pi is booting up and we can add an app to its configuration that we want running at start-up.

To create a new service that runs on start-up we need to create a new `.service` file. So go ahead and create a new file `<your app name>.service` (I'll use the one I created for myself as an example here)

    [Unit]
    Description=Powman power usage app
    After=network.target

    [Service]
    ExecStart=/usr/bin/powman/powman
    WorkingDirectory=/usr/powman
    StandardOutput=inherit
    StandardError=inherit
    Restart=always
    User=pi
    RestartSec=15
    Environment=DOTNET_ROOT=/usr/bin/dotnet

    [Install]
    WantedBy=multi-user.target

Here's the breakdown of the most important things in the file:

 - **After** tells systemd that our app can only start after the network stack has been started (we're starting a webapp so we need networking to be ready).
 - **ExecStart** is the location of the apps binary we want to start. (You create this by publishing or building your app from source)
 - **RestartSec** You'll need to set this to a sensible number, not to low or your service will time-out at startup, but also not to high so that in case of crashes, it's restarted quickly.
 - **Environment** This is an important one, remember when we added the dotnet bits to the `bashrc` file? Since we're running things as a service now, there is no `.bashrc` to set everything up. To make sure that our dotnet app starts when systemd calls it for us we need to set the `DOTNET_ROOT` environment variable (it needs the same value as you as a user would use).

After creating the service file we can go ahead and install it. First move the file to the systemd system folder which can be found here `/etc/systemd/system`.
First step is to enable the service (use the same name you gave it when creating it, but without the `.service` part):

    sudo systemctl enable <your app name>

Next step is to actually start the service:

    sudo systemctl start <your app name>

To see if everything has gone to plan, check the status of the service:

    sudo systemctl status powman

Which will give you something that looks like this (I used my app here as an example):

    ● powman.service - Powman power usage app
    Loaded: loaded (/etc/systemd/system/powman.service; enabled; vendor preset: enabled)
    Active: active (running) since Sat 2021-01-09 17:25:34 CET; 21h ago
    Main PID: 23649 (powman)
        Tasks: 19 (limit: 2063)
    CGroup: /system.slice/powman.service
            └─23649 /usr/powman/powman

    Jan 09 17:25:34 raspberrypi systemd[1]: Started Powman power usage app.
    Jan 09 17:25:36 raspberrypi powman[23649]: info: Microsoft.Hosting.Lifetime[0]
    Jan 09 17:25:36 raspberrypi powman[23649]:       Now listening on: http://0.0.0.0:5100

If your Pi now power cycles or something else goes wrong and everything restarts the app will automatically start again without any manual steps.

Now you're all set to start developing, building and hosting small stuff on a Raspberry Pi, keep in mind that a Pi has limited resources so things like live-rebuilding on code changes and publishing aren't exactly quick 😉