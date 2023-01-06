{
    "title": "Self-host FreshRSS on a Microsoft Azure Web App",
    "summary": "A quick guide to setting up FreshRSS on Microsoft Azure using Docker Compose and an Azure Web App.",
    "slug": "fresh-rss-on-azure",
    "tags": [
        "Azure",
        "RSS",
        "FreshRSS"
    ],
    "created": "2023-01-06"
}
---

# Self-host FreshRSS on a Microsoft Azure Web App

I'm getting back to the days of old with RSS and since I really don't want to depend on any third party service the only way forward is self hosting.

After some searching around for some options I ended up with [FreshRSS](https://www.freshrss.org/).

For the self-hosting part, I went with Microsoft Azure because I still get credits for it so it's basically free. The Azure App Service is a good fit, it supports code, docker image and docker compose deployements.

I initially went with a regular docker image and mounting the volumes on an Azure Storage container but that turned out bad because the database kept getting locked. Some reading that I of course didn't do up front even warned about this, but it was a fun experiment either way.

So the next thing is docker compose. Some googling gave me a complete YAML that didn't work right away but with some minor trail and error started working. The notable things I had to fix were exposing the port and setting up the background job `CRON_MIN` setting so it would update the feeds in the background. The `1,31` means every first and thirtyfirst minute of the hour.    

```
version: "3.3"

volumes:
  data:
  extensions:

services:

  freshrss:
    image: freshrss/freshrss
    container_name: freshrss
    hostname: freshrss
    restart: unless-stopped
    logging:
      options:
        max-size: 10m
    volumes:
      - data:/var/www/FreshRSS/data
      - extensions:/var/www/FreshRSS/extensions
    ports:
       - "80:80"
    environment:
      TZ: Europe/Amsterdam
      CRON_MIN: '1,31'
```

To ensure that the volumes use actual storage on the App Service, a single setting is needed on the App Service: `WEBSITES_ENABLE_APP_SERVICE_STORAGE` with its value set to `true`.

The whole thing can be set up through the Azure portal itself. During the creation of the App Service, select the docker compose deployment and upload the above snippet in a YAML file. After working through the rest of the setup and starting the app for the first time, you can go ahead and set the storage variable and let the app restart.

If you now browse to the url you picked for your app you should be greeted with the FreshRSS setup screen.
