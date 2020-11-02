using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ModelManagement.Tests
{
    [TestClass]
    public class ModelManagerTests
    {
        private DataManager _dataManager;
        private SubscriptionManager _subscriptionManager;

        public ModelManagerTests()
        {
            this._dataManager = new DataManager();
            this._subscriptionManager = new SubscriptionManager(this._dataManager);
        }

        [TestMethod]
        public void KeysAreAvailableFromManagerByType()
        {
            object demoKey = 5;
            CreateSubscription<TestModel>(demoKey);

            var keys = _dataManager.GetSubscriptionKeys<TestModel>();

            Assert.IsTrue(keys.Count() == 1 && keys.First() == demoKey);
        }

        [TestMethod]
        public void AllKeysAreAvailableFromManager()
        {
            object demoKey = 5;
            CreateSubscription<TestModel>(demoKey);

            var interests = _dataManager.EnumerateSubscriptions();
            var testInterest = interests.First();

            Assert.IsTrue(interests.Count() == 1);
            Assert.IsTrue(testInterest.Key == typeof(TestModel) && testInterest.Value == demoKey);
        }

        [TestMethod]
        public void Locks()
        {

        }

        private Subscription<TModel> CreateSubscription<TModel>(object key)
        {
            var sub = _subscriptionManager.GetSubscription<TModel>();
            sub.UpdateKey(key);
            return sub;
        }
    }
}
