# SignalR dependency injection

My go-to container for IoC at the moment is [SimpleInjector][1] because you know.. it's simple. I've used it in a couple of projects and used in a hobby project recently again.

This hobby project uses [SignalR][2] for real-time communication between browser and server and after setting that up I wanted to inject dependencies in to the SignalR hubs. It is pretty easy but there was a minor catch. Let's look at some code.

The first thing that is required is a dependency resolver that works with SignalR, lucky for us, it provides a built-in default one which we can implement:

    public class SignalrSimpleInjectorDependencyResolver : DefaultDependencyResolver
    {
        private readonly Container container;
    
        public SignalrSimpleInjectorDependencyResolver(Container container)
        {
            this.container = container;
        }
    
        public override object GetService(Type serviceType)
        {
            var serviceProvider = this.container as IServiceProvider;
            return serviceProvider.GetService(serviceType) ?? base.GetService(serviceType);
        }
    
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.GetAllInstances(serviceType).Concat(base.GetServices(serviceType));
        }
    }
    

This is also the place where I ran into that catch I mentioned earlier. Take a look a the GetService method, see that cast there. SimpleInjector is playing it safe by throwing an exception when it cannot find a registration for a type. Since we implement the default resolver we need to be able to also check the built-in container for registrations so that SignalR can do it's work like it used to. The container is an IServiceProvider and it exposes a method called GetService which will return null if it cannot find the requested type in the container. Just what we need :-) Now we first check the SimpleInjector container, if that turns up null, we check the built-in container.

You can see the code break at runtime by replacing the GetService call with a GetInstance call directly on the container and then running your application. SignalR has all sorts of dependencies of its own for which SimpleInjector will throw an exception when requested, making the **?? base.GetService(serviceType)** part useless.

Now that the resolver is done, we still need to register it.

    var hubConfiguration = new HubConfiguration
    {
        Resolver = new SignalrSimpleInjectorDependencyResolver(container)
    };
    

The code above is placed in the Application_Start method of the application where the container is created. The configuration is then passed on to SignalR via the MapHubs method that sets up SignalR.

    RouteTable.Routes.MapHubs(hubConfiguration);
    

You can now create interfaces for your hubs and register them in the SimpleInjector container which in turn will inject the dependencies into the hub constructors.

 [1]: https://simpleinjector.codeplex.com/
 [2]: http://www.asp.net/signalr