# SpringOne Platform 2019

Pivotal organized another SpringOne conference this year, and the ITQ Cloud Native team hopped across the large pond into the Texes heat to attend.

![SpringOne Platform by Pivotal](/content/sp1p-2019/sp1p.jpg)

## Spring for the .net dev
While Spring is a Java thing, SteelToe is there for us .net folks. It's getting a big shot of TLC with a new website and new and improved docs!
Creating microservices using .net got a bit easier than it already was with some new stuff such as actuators and service connectors.
Go have a look at the [SteelToe website](https://www.steeltoe.io) for all the ins and outs. (also, [this](https://start.steeltoe.io) exists to quickly start a new service) 

## Azure Spring Cloud
A very nice surprise is the a fully managed Spring Cloud option over on Microsoft Azure. It's still very much a preview but you can already use it to host your Spring microservices.
It being a fully managed setup means that things like service discovery and configuration server and the cluster itself are all taken care of.

![Austin capitol](/content/sp1p-2019/azure-spring-cloud.png)

I requested access to the preview and was allowed in to play around a bit. My one wish currently is the ability to upload a .net core `.dll` in there instead of `.jar` files. There's no word out there but given the work being done on SteelToe, I don't see any reason why that feature should not exist.

More info on Azure Spring cloud can be found [here](https://azure.microsoft.com/en-us/services/spring-cloud/) (You can request preview access there as well).

## The `cf push` experience and kubernetes
Given my experience with Cloud Foundry and Kubernetes, it still saddens me you can't "just push" something onto k8s. That might be over soon though because with the rise of [pack](https://github.com/buildpack/pack) and [kpack](https://content.pivotal.io/blog/introducing-kpack-a-kubernetes-native-container-build-service) we might see something similar soon! (k)pack make use of [cloud native buildpacks](https://buildpacks.io/) to create runnable images from source.

We got to see some pretty cool demos from both a dev and SRE perspective. It takes away all the dockerfile nonsense but also allows for managing the layers in the image.

## Wrapping up

![Austin capitol](/content/sp1p-2019/austin.png)

We had a bit of time off so we went on a tour the most dutch way possible, on bicycles. Enjoyed the local food and beer. And a visit to Texas wouldn't be complete without a visit to the gun range.

Like my previous experiences in the US, the city is very nice mostly, but the social issues in the US clearly show when you walk/ride around town.

Hot, it was extremely hot. Maybe not crazy hot by Texan standards, but compared to our rainy little Netherlands, extremely hot 😄.

Still not a fan of the long flights but all in all it was great.