using Permissions.Tests.Helpers;
using PermissionsModels;

namespace Permissions.Tests
{
    public abstract class BaseTest
    {
        protected readonly InMemoryManager Manager;
        protected readonly User User;
        protected readonly Service Service;
        protected readonly Group ActorGroup;
        protected readonly Group TargetGroup;

        public BaseTest()
        {
            Manager = new InMemoryManager();
            User = new User();
            Service = new Service();
            ActorGroup = new Group();
            TargetGroup = new Group();
        }

        protected static Operation StartOperation => new Operation("Start");

        protected static Operation StopOperation => new Operation("Stop");
    }
}
