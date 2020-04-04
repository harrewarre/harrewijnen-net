# Getting started with Inversion of Control

Alright, lets dive straight into the deep end! ... ok, but not drown in the process of doing so. One step at a time. Lets's start with an example.

## The technical side

Here is some code:

    public class Foo
    {
        private readonly Bar bar;
    
        public Foo()
        {
            this.bar = new Bar();
        }
    }
    

To create an instance of Foo we write

    Foo theFoo = new Foo();
    

In this example we have two classes, Foo and Bar. Foo makes use of a Bar to do some work. When a Foo instance is created the creator of the Foo instance has no control over what kind of Bar the Foo instance is going to use. They are stuck together. This code is completely valid but what if we want to let the creator of Foo **control** the kind of Bar that is used? We have to invert control! Here is the same code with it's control flow inverted:

    public class Foo : IFoo
    {
        private readonly IBar bar;
    
        public Foo(IBar bar)
        {
            this.bar = bar;
        }
    }
    

First change is that Bar is now implementing an interface, IBar. This way, we lose the dependency on the concrete Bar implementation inside Foo. To give control to the creator of Foo, we let the constructor accept an object of type IBar and use this instead of creating it by itself. Foo now accepts any kind of Bar, as long as it implements IBar. Foo does not have to worry about the kind of Bar anymore. Foo itself is now also based on an interface (IFoo), we'll get to that later.

If we now want to create a new Foo, we write it like this:

    IFoo theFoo = new Foo(new Bar());
    

So the creator of the Foo object now gets to decide what kind of Bar is used in the Foo instance - **Foo is in control of the Bar it will be using**. But we still create a concrete Foo and Bar. This is where Dependency Injection comes in to play. What if we could have single place where we define what kind of Foo and Bar are used when our program starts? We could rig up an entire graph of objects that **depend** on each other to form a working program.

The following examples make use of [SimpleInjector][1] for the IoC container and uses constructor injection to inject dependencies.

The 'single place' mentioned earlier is where the IoC container is created. We tell the container which objects it can create for us and it will manage the dependencies for us automatically. The container is created at the start of the application so that it can manage any type of object we feed it and is available anywhere in the application. Here is some code that starts a console application and sets up a container to handle our Foo and Bar:

    using SimpleInjector;
    
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
    
            container.Register(typeof(IFoo), typeof(Foo));
            container.Register(typeof(IBar), typeof(Bar));
        }
    }
    

The container is now aware of Foo and Bar and when we pull an instance from the container it will automatically **inject** the **dependecies** in the constructors of the objects its creating.

    var someFoo = container.GetInstance(typeof(IFoo));
    

SomeFoo is now created by the container and Bar is injected on the fly into its constructor.

A little caution here. You **DO NOT** want to pass around the container object and use GetInstance all over your application. Doing so will make the container a dependency in your application! My reason for not passing around the container is simply to avoid having a dependency on the container itself. It also hides other dependencies the class might have since the constructors only take a container instead of the actual dependencies.

## But why?

Now that you know all this, why should you use it? The code is now loose coupled, meaning that they do not depend on concrete dependencies and only depend on interfaces. Because of this it is now very easy to swap out Bar for another Bar with different logic. The container will take care of injecting IBar objects so after writing a new OtherBar class that implements IBar we only need to change one line of code to make our application use the new OtherBar implementation.

Change this

    container.Register(typeof(IBar), typeof(Bar));
    

To this

    container.Register(typeof(IBar), typeof(OtherBar));
    

And every object that depends on IBar will now use an OtherBar instance without requiring changes to their own logic. This greatly improves the maintainability of your code because no object depends directly on another. The concrete types know about the dependencies through interfaces, not their implementations. Another bonus is testability. Because its interfaces all around, its is very easy to create a mock for a dependency and have it injected into the code that is being tested.

So in a nutshell, this is Inversion of Control and Dependency Injection.

## But there is more!

Some great principles in software development are at the base of these patterns. Take a look at the [SOLID][2] principles which are good no matter how you design your application.

My examples use SimpleInjector as a container, but there are many more. Search around and find out more about them.

To try all this for yourself and see the benefits from it very quickly you can write up a sample ASP.NET MVC app and inject dependencies into the controllers. (Simple Injector has ASP.NET MVC support that you can get straight from NuGet!)

 [1]: http://simpleinjector.codeplex.com/
 [2]: https://en.wikipedia.org/wiki/SOLID_(object-oriented_design)