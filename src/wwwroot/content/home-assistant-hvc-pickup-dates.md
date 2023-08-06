{
    "title": "HVC Trash pickup dates in Home Assistant",
    "summary": "Display HVC (my local waste management company) trash pickup dates in Home Assistant",
    "slug": "home-assistant-hvc-pickup-dates",
    "tags": [
        "Home Assistant",
        "YAML"
    ],
    "created": "2023-08-06"
}
---

# HVC Trash pickup dates in Home Assistant

Maybe something you can use, maybe not, but if you are in intersection of people who use Home Assistant and live in the Netherlands and in a region where HVC operates, this might be useful.

The integrations below will let you display a day counter until the next trash pickup takes place. This sensor data can also be used in other automations such as notifications or reminders.

The payload we want to load into Home Assistant looks like this:

    [{"afvalstroom_id":5,"ophaaldatum":"2023-08-08"},{"afvalstroom_id":3,"ophaaldatum":"2023-08-10"},{"afvalstroom_id":6,"ophaaldatum":"2023-08-31"}]

Each element contains an ID and a date. The ID is the type of waste and the date is the next pickup date for that type of waste. We want to extract the date for each type of waste and then calculate the number of days until that date.

The `afvalstroom_id` determines which waste stream we are looking at. We don't want to depend on the order in which the streams are listed so we need to check for each ID and get the correct date for each type of waste. The IDs are as follows:
* 3: Paper
* 5: Bio (greens)
* 6: Plastics and metal

## Rest integration

First, we need a `rest` integration that fetches the JSON that contains the dates we need, checks the ID and then extracts the date for each type of waste. This is the configuration I use:

    rest:
      resource: "https://inzamelkalender.hvcgroep.nl/rest/adressen/0479200000035232/ophaaldata"
      scan_interval: 7200
      sensor:
        - name: "Plastic ophaaldatum"
          unique_id: "waste_plastic_next_pickup"
          value_template: >
            {% set item = value_json | selectattr('afvalstroom_id','eq', 6) | list | first %}
            {{ item.ophaaldatum  }}
        - name: "GFT Ophaaldatum"
          unique_id: "waste_green_next_pickup"
          value_template: >
            {% set item = value_json | selectattr('afvalstroom_id','eq', 5) | list | first %}
            {{ item.ophaaldatum  }}
        - name: "Papier ophaaldatum"
          unique_id: "waste_papier_next_pickup"
          value_template: >
            {% set item = value_json | selectattr('afvalstroom_id','eq', 3) | list | first %}
            {{ item.ophaaldatum  }}

I've set the URL to something NOT my address so you'll have to use the HVC website to figure out what your address ID is. The URL is in the format `https://inzamelkalender.hvcgroep.nl/rest/adressen/<address ID>/ophaaldata`. Use the developer tools in your browser to find the address ID.

Be mindfull of the `scan_interval` setting. This is the number of seconds between each API call. I've set it to 7200 seconds (2 hours) because the pickup dates don't change that often. You can set it to whatever you want, but there could be rate-limiting on the API side which will cause errors in the sensors.

You can put rest sensor in `configuration.yml` or any other spot where YAML is loaded from through the main configuration.

## Template sensors

The above sensors expose the date of the next pickup for the three waste streams we have here in a single API call, so no unnecessary calls are made to the API.
To get day counters we can create some template sensors that work with the dates we gathered from the API:

    days_until_waste_plastic_pickup:
      unique_id: "days_until_waste_plastic_pickup"
      friendly_name: "Days until waste plastic pickup"
      value_template: >
        {{ ((as_timestamp(states('sensor.plastic_ophaaldatum')) - as_timestamp(now())) / 86400) | round(0, "ceil") }}
        
    days_until_waste_paper_pickup:
      unique_id: "days_until_waste_paper_pickup"
      friendly_name: "Days until waste paper pickup"
      value_template: >
        {{ ((as_timestamp(states('sensor.papier_ophaaldatum')) - as_timestamp(now())) / 86400) | round(0, "ceil") }}
        
    days_until_waste_green_pickup:
      unique_id: "days_until_waste_green_pickup"
      friendly_name: "Days until waste bio pickup"
      value_template: >
        {{ ((as_timestamp(states('sensor.gft_ophaaldatum')) - as_timestamp(now())) / 86400) | round(0, "ceil") }}

These custom sensors can go in any YAML file where you've defined `- platform: template` sensors. We take the timestamp of the pickup date and subtract the current timestamp. This gives us the number of seconds until the pickup date. We divide this by the number of seconds in a day (86400) and round up to the nearest integer. This gives us the number of days until the pickup date 😊

From here you can update your UI and include these in your dashboards and automations. Enjoy!