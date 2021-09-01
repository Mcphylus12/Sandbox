using Commander;
using ModelManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    class TestComponent
    {
        private ISubscriptionManager subscriptionManager;
        private readonly ICmder cmder;
        private ISubscription? subscription;

        public Action<TestModel>? Render;

        public TestComponent(TestApplicationContainer container)
        {
            this.subscriptionManager = container.SubscriptionManager;
            this.cmder = container.Cmder;
        }

        internal void Init()
        {
            this.subscription = subscriptionManager.Subscribe<TestModel>(model => Render?.Invoke(model));
            this.subscription.UpdateKey("testKey");

            Console.WriteLine($" command result: {this.cmder.Execute(new TestParam()).Result}");
        }

        internal void Destroy()
        {
            this.subscription?.Dispose();
        }
    }
}
