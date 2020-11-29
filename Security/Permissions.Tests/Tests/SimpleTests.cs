using Microsoft.VisualStudio.TestTools.UnitTesting;
using Permissions.Tests.Helpers;
using PermissionsModels;
using System.Threading.Tasks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Permissions.Tests.Tests
{
    [TestClass]
    public class SimpleTests : BaseTest
    {
        [TestMethod]
        public async Task CantDoByDefault()
        {
            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsFalse(canDo);
        }

        [TestMethod]
        public async Task CanDoWhenAssigned()
        {
            Manager.Assign(User, StartOperation, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }


        [TestMethod]
        public async Task CantDoWhenAssignedOtherOperation()
        {
            Manager.Assign(User, StopOperation, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsFalse(canDo);
        }

        [TestMethod]
        public async Task CantDoWhenUnsupportedOperation()
        {
            Service.ClearSupportedOperations();
            Manager.Assign(User, StartOperation, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsFalse(canDo);
        }

        [TestMethod]
        public async Task CanDoWhenAssignRoleContainsOperation()
        {
            var editOperation = new Operation("Edit");
            Manager.AddChild(editOperation, StartOperation, StopOperation);
            Manager.Assign(User, editOperation, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        [TestMethod]
        public async Task CanDoWhenInGroupWithRole()
        {
            Manager.AddChild(ActorGroup, User);
            Manager.Assign(ActorGroup, StartOperation, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        [TestMethod]
        public async Task GroupsTest()
        {
            var editOperation = new Operation("Edit");
            Manager.AddChild(editOperation, StartOperation);
            Manager.AddChild(ActorGroup, User);
            Manager.AddChild(TargetGroup, Service);
            
            Manager.Assign(ActorGroup, editOperation, TargetGroup);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        [TestMethod]
        public async Task MultipleOperationGroupLevelTest()
        {
            var editOperation = new Operation("Edit");
            var adminOperation = new Operation("Admin");
            Manager.AddChild(editOperation, StartOperation);
            Manager.AddChild(adminOperation, editOperation);

            Manager.Assign(User, adminOperation, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        [TestMethod]
        public async Task MultipleObjectGroupLevelTest()
        {
            var TopActorGroup = new Group();
            var TopTargetGroup = new Group();
            Manager.AddChild(TopTargetGroup, TargetGroup);
            Manager.AddChild(TopActorGroup, ActorGroup);

            Manager.AddChild(TargetGroup, Service);
            Manager.AddChild(ActorGroup, User);

            Manager.Assign(TopActorGroup, StartOperation, TopTargetGroup);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }
    }
}
