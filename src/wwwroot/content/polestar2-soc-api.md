{
    "title": "Polestar 2 battery 'API'",
    "summary": "How to read the battery state of a Polestar 2 using the Tibber API",
    "slug": "polestar2-soc-api",
    "tags": [
        "polestar 2",
        "api",
        "ev",
        "soc"
    ],
    "created": "2023-10-16"
}
---

# Polestar 2 battery 'API'

Polestar (the car manufaturer) offers an app to control (a very limited) set of features of their cars remoteley (locks, and climate) and can display the state of the battery in % remaining. It's nice to have but the app isn't all that great and sometimes takes very long to show up-to-date information.

Tibber (the electricity provider) recently started offering an integration on their platform that can read the battery state of the Polestar 2. Very handy of course, this particular app can show me state of charge and also how much electricity my car has used in the this month.

While Polestar does not offer any public API that I know of, Tibber does! Two of them actually. One is for their back-end and the other for their app. Both are well documented and easy to use with the help of GraphQL.

- https://developer.tibber.com/explorer
- https://app.tibber.com/

So it turns out we can easly get the battery state from the Tibber API for the Polestar 2. The only thing needed is a Tibber account (and, well... a Polestar 2). The same API that powers the Tibber app can be used programmatically as well. There is one catch with this approach though, and that is the authentication, the Tibber app API does not seem to support token generation so we'll need to sign in to the API with a username and password and then obtain a token that way. Not ideal but not really a problem either.

## Getting the battery state

The very first thing you need to do is configure the Polestar "Power-up" in the Tibber app. It can be done right in the app and requires you sign into your Polestar account to configure it. Once that is done, we can start calling the Tibber API to get the battery state.

After you sign into the [app API](https://app.tibber.com/), you can run this query (still works at the time of writing - The ID fields in the query will depend on your account and car so you'll have to play around a bit with the API explorer to obtain them):

```graphQL
{
  me {
    home(id:"[your-home-id]") {
      electricVehicle(id:"[your-vehicle-id]") {
        shortName,
        lastSeen,
        battery {
          percent,
          isCharging
        }
      }
    }
  }
}
```

You can use the API explorer to figure out your home and vehicle ID. If everything went according to plan you should get a response like this:

```json
{
  "data": {
    "me": {
      "home": {
        "electricVehicle": {
          "shortName": "Polestar 2",
          "lastSeen": "2023-10-16T06:39:47.000+00:00",
          "battery": {
            "percent": 85,
            "isCharging": false
          }
        }
      }
    }
  }
}
```

Given all the above, we can set up something that does all that work programmatically for us. Below you'll find some `dotnet core` C# code you can reference to get started.

Knowing all this opens up many more options for us. We can now build our own app that can display the battery state, or we can integrate it into our home automation system. The possibilities are endless! I do hope that this functionality will be exposed through the back-end API at some point where we can supply a token generated for our account and not expose a username/password combo.

### A quick-and-dirty sample implementation

Here's a quick little demo app you can copy over. It does require you to set up some environment variables that contain your Tibber username, password, home ID and vehicle ID (which you should have if you've played around with the Tibber API a bit).

- TIBBER_USER
- TIBBER_PASSWORD
- TIBBER_HOME_ID
- TIBBER_VEHICLE_ID

Create a new `dotnet core webapi` app and paste the following stuff in the right places.

**`Program.cs`**

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "TIBBER_");
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IStateOfChargeApi, TibberStateOfChargeApi>();

var app = builder.Build();

app.MapGet("/soc", async (IStateOfChargeApi api) =>
{
    return await api.GetStateOfCharge();
});

app.Run();
```

**`TibberApi.cs`**

```csharp
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public interface IStateOfChargeApi
{
    Task<StateOfCharge> GetStateOfCharge();
}

public class TibberStateOfChargeApi : IStateOfChargeApi
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public TibberStateOfChargeApi(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<StateOfCharge> GetStateOfCharge()
    {
        var user = _configuration.GetValue<string>("USER");
        var password = _configuration.GetValue<string>("PASSWORD");

        var homeId = _configuration.GetValue<string>("HOME_ID");
        var vehicleId = _configuration.GetValue<string>("VEHICLE_ID");

        var token = await GetToken(user!, password!);
        var query = GetQuery(homeId, vehicleId);

        using HttpClient client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Add("Cookie", $"token={token}");
        var response = await client.PostAsJsonAsync("https://app.tibber.com/v4/gql", new { query });

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to run query!");
        }

        var content = await response.Content.ReadFromJsonAsync<JsonNode>();

        var percent = content!["data"]!["me"]!["home"]!["electricVehicle"]!["battery"]!["percent"]!;
        var isCharging = content!["data"]!["me"]!["home"]!["electricVehicle"]!["battery"]!["isCharging"]!;

        return new StateOfCharge(percent.GetValue<int>(), isCharging.GetValue<bool>());
    }

    private async Task<string> GetToken(string username, string password)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync("https://app.tibber.com/login.credentials", new { email = username, password });

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get token!");
        }

        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();

        return content!.Token;
    }

    private string GetQuery(string homeId, string vehicleId)
    {
        return $"{{ me {{ home(id:\"{homeId}\") {{ electricVehicle(id:\"{vehicleId}\") {{ lastSeen, battery {{ percent, isCharging }}}}}}}}";
    }
}

public record StateOfCharge(int Percent, bool IsCharging);

public record TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}
```

After building and running the app, browse to the `/soc` endpoint and you should see something like this:

```json
{
  "percent": 85,
  "isCharging": false
}
```

Enjoy ⚡