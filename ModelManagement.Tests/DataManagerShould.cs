using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            ISubscription sub = CreateSub(key, _ => { });

            AssertLatestKeys(key);
        }

        private void AssertLatestKeys(params string[] keys)
        {
            CollectionAssert.AreEqual(this.dataUpdaterMock.LatestKeys, keys);
        }

        [TestMethod]
        public void InterestRemoved_When_SubscriptionDisposed()
        {
            ISubscription sub = CreateSub("test", _ => { });

            sub.Dispose();
            AssertLatestKeys();
        }

        [TestMethod]
        public void InterestChanges_When_SubscriptionUpdates()
        {
            ISubscription sub = CreateSub("test", _ => { });
            AssertLatestKeys("test");

            sub.UpdateKey("newTest");
            AssertLatestKeys("newTest");
        }

        [TestMethod]
        public void WhileAtLeast1SubIsActiveTheInterestIsRegistered()
        {
            ISubscription sub = CreateSub("test", _ => { });
            ISubscription sub2 = CreateSub("test", _ => { });

            sub.Dispose();
            AssertLatestKeys("test");

            sub2.Dispose();
            AssertLatestKeys();
        }

        [TestMethod]
        public void UpdateOnlyCorrectSubscription()
        {
            bool goodSubUpdate = false;
            bool badTypeSubUpdate = false;
            bool badKeySubUpdate = false;
            var goodSub = CreateSub<MockModel>("goodKey", _ => goodSubUpdate = true);
            var badTypeSub = CreateSub<OtherMockModel>("goodKey", _ => badTypeSubUpdate = true);
            var badKeySub = CreateSub<MockModel>("badKey", _ => badKeySubUpdate = true);

            this.sut.NotifyDataUpdated("goodKey", new MockModel());

            Assert.IsTrue(goodSubUpdate);
            Assert.IsFalse(badTypeSubUpdate);
            Assert.IsFalse(badKeySubUpdate);
        }

        private ISubscription CreateSub(string key, Action<MockModel> action)
        {
            return CreateSub<MockModel>(key, action);
        }

        private ISubscription CreateSub<T>(string key, Action<T> action)
        {
            var sub = this.sut.Subscribe(action);
            sub.UpdateKey(key);
            return sub;
        }
    }
}
