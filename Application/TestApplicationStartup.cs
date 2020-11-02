using ModelManagement;

namespace Application
{
    class TestApplicationStartup
    {
        private DataManager manager;

        public ISubscriptionManager SubscriptionManager { get { return manager; } }
        public IDataManager DataManager { get { return manager; } }

        public TestApplicationStartup()
        {
            this.manager = new DataManager();
        }
    }
}
