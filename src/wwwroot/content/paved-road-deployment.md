{ "slug":"paved-load-deployment", "title": "Paved road - Deployments", "summary": "Reduce waste and improve developer experience with pipelines-as-a-service.", "tags": [ "devops", "developer experience" ], "created": "2022-07-18" }

---

# Paved road - Deployments

Speeding up developer teams by eliminating waste and busywork can come in many forms. A huge step comes through automation, deployment specifically. Getting your app to production in an automated manner through some sort of pipeline is a huge timesaver. 

Given this automation, you can take it one step further by moving the creation and management of those pipelines one step up and offer ready made pipelines that teams can take from the digital shelf. These pipelines can be managed by the internal developer community or maybe a supporting team that already enables teams in other ways. 

Developer experience is greatly improved with these automated systems, so how do you set this up? Well it turns out that most CI/CD systems (eg: Azure DevOps, GitLab, Github actions) have some sort of inheritance. 

Using [GitLab as an example](https://docs.gitlab.com/ee/ci/yaml/includes.html): `include` statements allow you to include the contents of one pipeline into another. Using this mechanism you could let teams include a full pipeline in their project that lives outside of their code and consequently not cost them anything to use. 

This approach does come with some caveats. It will require teams to adhere to conventions described by the pipeline, so documentation is key. Some examples of this are build and test commands or maybe the use of certain frameworks. You'll end up with a pipeline per language and potentially also framework, so this will immediately force you to think about some sort of standardization across teams. 

While these standards will limit the choices of teams if they want to use one of these ready-made paths to production these should not be mandatory. The trick is to make them so easy to use, teams will **want** to use them. Be very careful about forcing teams to use anything or you'll run the risk of teams ignoring what you have to offer entirely. 

So while there are some things to think about when offering such a service, a huge bonus of these pipelines-as-a-service is that their implementation is centralized. Instead of each app having their own pipeline, many apps share a single implementation. Meaning you can introduce additional tooling for huge swaths of apps at once. Things like dependency scanning, software bill of materials, security checks, signing, they all become easier to roll out because of this more centralized implementation and dev teams get all that stuff for free. Just keep an eye on the speed of the pipelines because no one likes slow builds 😉  

As with many things that involve change in some way, implementing is far easier than the actual adoption by the organization. It's easy to get started on these sorts of pipelines but when the get some good adoption you'll be running into questions about ownership and requests to change stuff. Having software guilds or some sort of core team that maintains and decides over the pipelines that belong to each tech stack would be the obvious answer, but that's a whole new subject on its own. 

In the end it will come down to creating a developer community that will take care of their own tools. But as stated earlier, this is far easier said than done. It might even require organization changes to allow teams more freedom to build and maintain things. Closer collaboration between developer teams (and also the operator teams that run the platforms and tools to facilitate all this). 

Before you know it you're on a journey into the DevSecOps world with self-organizing teams and close collaboration between dev teams and platform- and service operators. There's much more to write about all this stuff so see you in the next post 😊  
