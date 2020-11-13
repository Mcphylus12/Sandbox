using Commander;
using ModelManagement;

namespace Application
{
    class TestApplicationContainer
    {
        private readonly DataManager manager;
        private readonly Cmder cmder;

        public ICmder Cmder { get { return cmder; } }
        public ICommandResolverStore CommandResolverStore { get { return cmder; } }
        public IDataManager DataManager { get { return manager; } }
        public ISubscriptionManager SubscriptionManager { get { return manager; } }

        public TestApplicationContainer()
        {
            this.manager = new DataManager();
            this.cmder = new Cmder();
        }
    }
}
