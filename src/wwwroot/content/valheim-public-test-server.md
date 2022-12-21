{
    "title": "Valheim public-test server",
    "summary": "How to update your Valheim server on Linux to the public-test branch.",
    "slug": "valheim-public-test-server",
    "tags": [
        "Valheim",
        "Gaming",
        "SteamCMD",
        "Linux"
    ],
    "created": "2022-11-23"
}
---

# Valheim public-test server

**UPDATE**

The testing appears to be done and is now part of the regular version of the game. This is still a handy guide for any future betas though.

---

Here's a cool update for [your Valheim linux server](/blog/valheim-linux-server) if you're up from some beta testing.

The Valheim devs announced the Mistlands update and we can all get in on the action early through a beta branch on Steam (and GamePass). You can find out more about the update [here](https://valheim.com/news/mistlands-public-test/) and there's a nice trailer [here](https://youtu.be/cZOuBjvETR8).

To update your game to the `public-test` version you have to opt into the beta version by going to the game's properties in Steam, selecting Betas and then entering `yesimadebackups` to unlock the public-test version. Once you've unlocked it you can update to this beta version of the game.

As with any beta or public-test stuff, there's going to be bugs and you can lose your world and/or your character. The game also warns about this. With that out of the way, let's update the server.

Before doing anything, make sure you stop your game server.

    sudo systemctl stop valheim

Once stopped we need to make a backup of the world, just in case anything goes bad. The world files are stored at `~/.config/unity3d/Irongate/Valheim/worlds_local/` so we just copy everything in there to a different folder.

    cp ~/.config/unity3d/IronGate/Valheim/worlds_local/*.* ~/valheim-backup/

Next up, updating the game. Let's have a quick look at the command we used to installed the stable version before we do anything.

    steamcmd +login anonymous +force_install_dir /home/harre/.steam/steamapps/common/valheim +app_update 896660 validate +exit

To get into the public-test version we need to specify which branch we want to download and also specify the beta password. Having a quick look in the [docs](https://developer.valvesoftware.com/wiki/SteamCMD) and searching for the word `branch` gives us the things we need to supply to the `app_update` command.

    -beta <branch name> -betapassword <password>

Which for Valheim `public-test` becomes

    -beta public-test -betapassword yesimadebackups

Then added to the existing command we already have:

    steamcmd +login anonymous +force_install_dir /home/harre/.steam/steamapps/common/valheim +app_update 896660 -beta public-test -betapassword yesimadebackups validate +exit

Run the command and wait for the game to download and install. Once completed you can start the server back up. Everthing we configured during the setup [in this post](/blog/valheim-linux-server) should still work.

    sudo systemctl start valheim

Now you're all set up to connect the game to your server and start exploring those Mistlands. Have fun out there!
