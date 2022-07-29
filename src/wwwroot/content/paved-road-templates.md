{ "slug":"paved-load-templates", "title": "Paved road - Templates", "summary": "Reduce waste and improve developer experience with application templates.", "tags": [ "devops", "developer experience" ], "created": "2022-07-29" }

---

# Paved road - Application Templates

In my previous post about paving the road to production there were some ideas around offering deployment pipelines as a service in your organization. In this post, we're going to take it up a level and have a look what we can do to remove even more friction, specifically when creating a new app from scratch. 

The process of creating a new app generally looks something like this: 

- Pick runtime and language 
- Pick any suitable frameworks 
- Add any additional dependencies for stuff like databases, single sign on, logging, etc. 
- Integrate all the above 
- Make sure it all works locally 
- Set up configs for various environments 
- Set up a pipeline 
- Deploy to dev/test or other environment to get pipeline working 
- Create value for your users ✨ 

Before you go ahead and start building something generic that you can quickly duplicate for new apps, you should think about what you are willing to support and what level of customization is going to make the most sense. 

Let's say for example many teams in an organization are using Python with Django for their apps. There might also be teams that have settled on Dotnet Core for their apps but they are far outnumbered by the amount of python. The logical conclusion would be to first cater to the biggest slice which in our case is  Python Django. 

Our imaginary org also has some sort internal single sign on system that is used in pretty much every app that gets deployed. It would make a lot of sense to include that by default so that teams only need to provide config values for their specific app. The same choices can be made for every dependency commonly used. There might be teams that don't need this SSO functionality but they can remove the bits they don't need (more on this later). 

One big thing we can also include in the template app: a pipeline that is able to build and deploy this app. Bonus points if this is a pipeline straight off the shelf as we described previously. 

By this point we have a pretty good understanding of what the most generic version (with all the most used bells and whistles!) of our python app should look like. So how do you make this easy to use? 

A way to give teams easy access is built right into Git. You could use the `subtree` command to stick the contents of our template repo in the repo that the team is working in. This could be a solution but it lacks a key thing that other solutions have to offer: customization (and with many things Git, it has a learning curve and comes with its own complexities). So what else is out there?

[Cookiecutter](https://www.cookiecutter.io/) is one such tool that allows custom settings in templates. It uses a repository that holds the template code and a CLI tool that makes use of the code in that repo to enable the customization. Customization can make the templates a lot more friendly. Don't need SSO? Turn it off before generating the code.

If we're moving into the more enterprise-y world there is [VMware Tanzu Application Accelerator](https://docs.vmware.com/en/Application-Accelerator-for-VMware-Tanzu/1.1/acc-docs/GUID-index.html), this also works with a template repository and allows for advanced transformations on templates to modify it in many ways. It has a web UI where developers can enter values for all the customization options and click a button to download a zip file that contains the template code. This generated code can then be pushed to the teams own repo for further development.

[Backstage Software Templates](https://backstage.io/docs/features/software-templates/software-templates-index) offers features similar to the Application Accelerator. 

So now we have tools to help teams get off the ground much faster by skipping a lot of the boilerplate that goes into creating new apps and should be a big boost in the developer experience. But as with the pipelines from the previous post, you can't really force anyone into using a setup like this and the advice is still the same: Make the templates super easy to use and keep your developers in the loop about any changes in the templates.

Ownership also comes into play once more and as with the pipelines, someone has to take care of the templates. It could be a core team, or the internal developer community, or tech guilds within that community, it largely depends on your internal org structure and culture.

At some point I hope to gather enough insights into what it takes to build such a community and I can write something about community building but it's a fuzzy and soft topic which makes it quite complicated. I'll keep working on that because I find that specific part of helping organizations improve their practices very interesting.

See you in the next one!