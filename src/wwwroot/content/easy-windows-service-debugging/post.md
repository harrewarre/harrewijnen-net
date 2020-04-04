# Easy Windows Service debugging

Long running processes in a web applicatoin running on IIS is a bad idea. The reason for that is that IIS might unload the AppPool that is running your process due to inactivity. Inactivity? Yep, if IIS does not recieve a requests for a while, it will shut the site down to conserve system resources. (This is all configurable ofcourse, but I've
seen it happen)

To allow for long running stuff, a Windows Service might be a right fit. Here is what the default startup code for a Windows service looks like:

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }

If you run this as your start-up project in Visual Studio, it fails to launch with the following message:

> Cannot start service from the command line or a debugger. A Windows Service must first be installed (using installutil.exe) and then started with the ServerExplorer, Windows Services Administrative tool or the NET START command.

You can't start a service by just running it. Not as a real service at least. Here is a neat trick you can implement to make this run inside the debugger.

First thing is this method that we need:

    public void StartDebug()
    {
        Debugger.Launch();
        OnStart(new string[] { });
        Thread.Sleep(Timeout.Infinite);
    }

It instructs the debugger to attach, calls the OnStart method and then blocks the current thread (more about this in second). So where do we put this code?

When you created your service, a file (in this example) was added called Service1, when you select the file in the Solution Explorer and hit F7 you can access the code that runs the service. Add the method seen above to Service1 class and you end up with this:

    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        public void StartDebug()
        {
            Debugger.Launch();
            OnStart(new string[] { });
            Thread.Sleep(Timeout.Infinite);
        }
    }

Don't forget these using statements:

    using System.Diagnostics;
    using System.Threading;

When all that is in place, we can modify the start-up code to call this method instead of running the default Run method.

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var service = new Service1();

    #if DEBUG
            service.StartDebug();
    #else
            ServiceBase.Run(service);
    #endif
        }
    }

This new code runs our new StartDebug() method when Visual Studio is set to the **Debug** Solution Configuration (which is available by default). If any other configuration is selected, the normal ServiceBase.Run(...) method is used instead. So what does all this accomplish?

Our StartDebug() method does the following things:

    Debugger.Launch();

Attaches a debugger to the process (see [here](https://msdn.microsoft.com/en-s/library/system.diagnostics.debugger.launch(v=vs.110).aspx) for some more info), 
*the process* in this case, is our Windows Service code launching.

    OnStart(new string[] { });

We call the OnStart(...) method the same way the actual Windows service would be started.

    Thread.Sleep(Timeout.Infinite);

By blocking the thread that starts the service code, the debugger will only detach when we stop our process (by hitting **stop** in Visual Studio).

If we don't block the thread, the program immediately exits and we won't be able to debug. This all assumes that you run all your heavy work on a new thread inside the service. Locking the start-up thread like this in a real service will cause it to never start (thats what the #if DEBUG is for). The Service Control manager in Windows waits for the OnStart(...) method to complete. Never completing it will cause a time-out and your service will never start.

So, use with a little care but this should make your life a little easier.