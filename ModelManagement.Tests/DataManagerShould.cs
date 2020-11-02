using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelManagement.Tests
{
    [TestClass]
    public class DataManagerTests
    {
        private readonly DataManager sut;
        private readonly MockDataUpdater dataUpdaterMock;

        public DataManagerTests()
        {
            this.sut = new DataManager();
            this.dataUpdaterMock = new MockDataUpdater();
            this.sut.RegisterDataUpdater(this.dataUpdaterMock);
        }

        [TestMethod]
        public void InterestsUpdated_When_SubscriptionGivenKey()
        {
            const string key = "test";
            ISubscription sub = CreateSub(key);

            AssertLatestKeys(key);
        }

        private void AssertLatestKeys(params string[] keys)
        {
            CollectionAssert.AreEqual(this.dataUpdaterMock.LatestKeys, keys);
        }

        [TestMethod]
        public void InterestRemoved_When_SubscriptionDisposed()
        {
            ISubscription sub = CreateSub("test");

            sub.Dispose();
            AssertLatestKeys();
        }

        [TestMethod]
        public void InterestChanges_When_SubscriptionUpdates()
        {
            ISubscription sub = CreateSub("test");
            AssertLatestKeys("test");

            sub.UpdateKey("newTest");
            AssertLatestKeys("newTest");
        }

        [TestMethod]
        public void WhileAtLeast1SubIsActiveTheInterestIsRegistered()
        {
            ISubscription sub = CreateSub("test");
            ISubscription sub2 = CreateSub("test");

            sub.Dispose();
            AssertLatestKeys("test");

            sub2.Dispose();
            AssertLatestKeys();
        }

        //Type RelatedTests

        private ISubscription CreateSub(string key)
        {
            var sub = this.sut.Subscribe<MockModel>(_ => { });
            sub.UpdateKey(key);
            return sub;
        }
    }
}
