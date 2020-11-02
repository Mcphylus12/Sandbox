using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Application
{
    class Entrypoint
    {
        public static void Main(string[] args)
        {
            //TODO (DONE): Work,
            //TODO: Unit tests,
            //TODO: Triple slash comments
            //TODO: WS and poll implementations for model management
            //TODO: ServiceProvider and HttpClient implementations for commander
            //TODO: get it all running on .net 5.0
            //TODO: get on github with nuget package builds

            RunIntegrationTests();
        }

        private static void RunIntegrationTests()
        {
            var startup = new TestApplicationStartup();

            var service = new TestService(startup);
            service.Init();

            var component = new TestComponent(startup);
            component.Render = ComponentRendered;
            component.Init();

            service.UpdateData();
            service.UpdateData();
            service.UpdateData();

            component.Destroy();
        }

        private static void ComponentRendered(TestModel model)
        {
            Console.WriteLine($"Data updated: {model.Value} ");
        }
    }
}
