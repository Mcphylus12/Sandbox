using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelManagement
{
    public class DataManager : IDataManager, ISubscriptionManager
    {
        private List<IDataUpdater> updaters;
        private readonly InterestStore interestStore;

        public DataManager()
        {
            updaters = new List<IDataUpdater>();
            interestStore = new InterestStore();
        }

        public void RegisterDataUpdater(IDataUpdater updater)
        {
            this.updaters.Add(updater);
        }

        public ISubscription Subscribe<T>(Action<T> onDataUpdated)
        {
            Subscription<T> subscription = new Subscription<T>(onDataUpdated, this);
            return subscription;
        }

        internal void RemoveSubscription<T>(Subscription<T> subscription)
        {
            interestStore.RemoveInterest(subscription);
        }

        public void NotifyDataUpdated<T>(object key, T newData)
        {
            foreach (Subscription<T> sub in interestStore.GetSubscriptions<T>(key))
            {
                sub.UpdateData(newData);
            }
        }

        internal void InterestUpdated<T>(object newKey, Subscription<T> subscription)
        {
            interestStore.RemoveInterest(subscription);
            interestStore.AddInterest(subscription, newKey);

            foreach (var updater in updaters)
            {
                updater.OnInterestsChanged<T>(interestStore.GetInterestedKeys<T>());
            }
        }
    }
}