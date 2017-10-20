using ConsoleRouter;

namespace TestApp
{
    class Program
    {
        static int Main(string[] args)
        {
            var builder = new AppHostBuilder()
                .WithRoute("doit(controller=do,action=it)")
                .WithRoute("hi(controller=action,action=hi) -n{name}")
                .WithRoute("{controller=help} {action=help}");
            var host = builder.Build();
            return host.Run(args);
        }
    }
}