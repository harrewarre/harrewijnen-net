{
    "title": "Valheim server on Linux",
    "summary": "A step-by-step guide to setting up a Valheim server on Linux.",
    "slug": "valheim-linux-server",
    "tags": [
        "Gaming",
        "Linux"
    ],
    "created": "2021-09-12"
}
---
# Valheim server on Linux

Here is a step-by-step guide for setting up a [Valheim](https://store.steampowered.com/app/892970/Valheim/) server on Linux. Having done it twice now I thought it would be a good idea to capture this somewhere so that next time I don't have to look any of this up again.

The only assumptions being made here is that your server is running Ubuntu of some semi-recent version and a basic understanding of Linux.

## SteamCMD

The [SteamCMD](https://developer.valvesoftware.com/wiki/SteamCMD) utility from Valve will help us install the gamefiles on the server.
Follow the instructions on the [wiki](https://developer.valvesoftware.com/wiki/SteamCMD#Linux.2FmacOS) to get it up and running.

## Valheim

Now that SteamCMD is available, we can use it to download the gamefiles to the server.

    steamcmd +login anonymous +force_install_dir /home/steam/.steam/steamapps/common/valheim +app_update 896660 validate +exit

Some things to note here:

 - I've forced the install directory to something without spaces or other special crap in the name. This will make setting up systemd easier later.
 - 896660 is the steam identifier for Valheim.
 - You can re-use this command later when you need to update the gamefiles when the devs release an update.

For quick re-use, create an `sh` file and put the command in there. I've named my `update_game.sh`, mark the file as executable and you're all set.

There are some last changes before we can run the server.

## Valheim server configuration

There is not a lot to change but it's a requirement if you want to set up things correctly. Browse to the folder where you installed the game and look for a file called `start_server.sh`. As instructed in the file, make a copy of it (for simplicity I've named mine `start_valheim.sh`). Here's what mine looks like:

    export TERM=xterm
    export templdpath=$LD_LIBRARY_PATH
    export LD_LIBRARY_PATH=./linux64:$LD_LIBRARY_PATH
    export SteamAppId=892970

    echo "Starting server PRESS CTRL-C to exit"

    ./valheim_server.x86_64 -name "Helheim" -port 2456 -world "Dedicated" -password "..."

    export LD_LIBRARY_PATH=$templdpath

I've added an additional export at the top (found after a bunch of searching for specific problems) to prevent some warnings and errors while the game starts. You could maybe get by without it, it'll depend on the rest of your system. If you see weird stuff in the game output, it's worth a shot.

Update the value for the `-name` argument to something you like and of course set the `-password` value. **Make sure it is longer than 5 characters and is not your server name.**

## Port configuration

This step will depend on your network infrastructure, I'm running my server on the Azure cloud platform which has its own way of configuring ports for VMs. Your setup is probably different but the gist of all of it is that you need op open the port range **`2456-2458`** for incoming traffic.

## Running the server

It's tempting now to just run the `start_valheim.sh` file and start gaming but that comes with a drawback. When you close your SSH session, your server will stop. To fix that we'll use systemd to run the gameserver for us (and even try to restart it should it for some reason crash or shut down).

Here is the config file I'm using:

    [Unit]
    Description=Valheim service
    Wants=network.target
    After=syslog.target network-online.target

    [Service]
    Type=simple
    Restart=on-failure
    RestartSec=10
    User=steam
    WorkingDirectory=/home/steam/.steam/steamapps/common/valheim/
    ExecStart=/bin/sh /home/steam/.steam/steamapps/common/valheim/start_valheim.sh

    [Install]
    WantedBy=multi-user.target

If you've stuck with the same names for files and directories you can copy-paste this into a file called `valheim.service` and roll with that, or change it, if needed.

Given the systemd config file above, we can now hand this over to systemd to hand off the running and monitoring of the server. First step is to install the service file. 

 1. Copy the file to `/etc/systemd/system`.
 2. Activate the service so that systemd can start the service: `sudo systemctl enable valheim` (don't include the `.service` part here).
 3. Start the service: `sudo systemctl start valheim`.
 4. Check the server status and see if it's actually running: `sudo systemctl status valheim`.

If everything was set up correctly, the output of step 4 will show a line that looks something like this (truncated for brevity):

    ● valheim.service - Valheim service
        Loaded: loaded (/etc/systemd/system/valheim.service; enabled; vendor preset: enabled)
        Active: active (running)

Should the server fail to start, use [journalctl](https://www.commandlinux.com/man-page/man1/journalctl.1.html) to have a look a the logs. To get the last 20 lines for the valheim server, run the following:

    journalctl -u valheim -n 20

This should give you enough insight to troubleshoot what's going wrong.

## Wrapping up

At this point you should be able to use the public IP of your server to start playing! I don't know much about the resources required to keep everthing running smoothly but I've been running a server on a fairly modest 2-core/8GB RAM Linux (`Standard D2as_v4` on Azure) VM that can easily host a 4 player game.

Now go forth and play the viking game with your friends on your very own Valheim server!