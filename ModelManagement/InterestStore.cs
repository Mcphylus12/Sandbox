using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelManagement
{
    internal class InterestStore
    {
        private readonly Dictionary<Type, Dictionary<object, List<ISubscription>>> subscriptionIndex;

        public InterestStore()
        {
            subscriptionIndex = new Dictionary<Type, Dictionary<object, List<ISubscription>>>();
        }

        internal void RemoveInterest<T>(Subscription<T> subscription)
        {
            this.subscriptionIndex[typeof(T)][subscription.currentKey].Remove(subscription);
        }

        internal void AddInterest<T>(Subscription<T> subscription, object newKey)
        {
            InitSubscriptionIndex<T>(newKey);
            this.subscriptionIndex[typeof(T)][newKey].Add(subscription);
        }

        private void InitSubscriptionIndex<T>(object key)
        {
            if (!subscriptionIndex.ContainsKey(typeof(T))) subscriptionIndex.Add(typeof(T), new Dictionary<object, List<ISubscription>>());
            if (!subscriptionIndex[typeof(T)].ContainsKey(key)) subscriptionIndex[typeof(T)].Add(key, new List<ISubscription>());
        }

        internal IEnumerable<Subscription<T>> GetSubscriptions<T>(object key)
        {
            return subscriptionIndex[typeof(T)][key].Cast<Subscription<T>>();
        }

        internal object[] GetInterestedKeys<T>()
        {
            return subscriptionIndex[typeof(T)].Where(kv => kv.Value.Count > 0).Select(kv => kv.Key).ToArray();
        }
    }
}
