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
            // repository + Specification pattern
            RunIntegrationTests();
        }

        private static void RunIntegrationTests()
        {
            var container = new TestApplicationContainer();
            new TestStartUp(container).Start();

            var service = new TestService(container);
            service.Init();

            var component = new TestComponent(container);
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
