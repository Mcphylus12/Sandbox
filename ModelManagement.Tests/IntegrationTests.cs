using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelManagement.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private DataManager _dataManager;
        private SubscriptionManager _subscriptionManager;

        public IntegrationTests()
        {
            this._dataManager = new DataManager();
            this._subscriptionManager = new SubscriptionManager(this._dataManager);
        }

        [TestMethod]
        public void SimpleDataSubscriptionAndUpdate()
        {
            TestModel test = null;
            var sub = _subscriptionManager.GetSubscription<TestModel>();
            sub.OnUpdate += newItem => test = newItem;
            sub.UpdateKey(1);
        }
    }
}
