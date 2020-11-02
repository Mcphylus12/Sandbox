using ModelManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    class TestComponent
    {
        private ISubscriptionManager subscriptionManager;
        private ISubscription subscription;

        public Action<TestModel> Render;

        public TestComponent(TestApplicationStartup startup)
        {
            this.subscriptionManager = startup.SubscriptionManager;
        }

        internal void Init()
        {
            this.subscription = subscriptionManager.Subscribe<TestModel>(model => Render(model));
            this.subscription.UpdateKey("testKey");
        }

        internal void Destroy()
        {
            this.subscription.Dispose();
        }
    }
}
