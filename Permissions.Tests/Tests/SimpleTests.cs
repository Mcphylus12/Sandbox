using Microsoft.VisualStudio.TestTools.UnitTesting;
using Permissions.Tests.Helpers;
using PermissionsModels;
using System;
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
        public async Task CantDoWhenAssignedOtherOperation()
        {
            await AssignOperation(StopOperation);

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsFalse(canDo);
        }

        [TestMethod]
        public async Task CantDoWhenUnsupportedOperation()
        {
            Service.ClearSupportedOperations();
            await AssignOperation(StartOperation);

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsFalse(canDo);
        }

        [TestMethod]
        public async Task CanDoWhenAssignRoleContainsOperation()
        {
            await AssignOperation(StartOperation, StopOperation);

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        [TestMethod]
        public async Task CanDoWhenInGroupWithRole()
        {
            Manager.AddChild(ActorGroup, User);
            var role = NewRole();
            role.AssignOperation(StartOperation);
            Manager.AssignRole(ActorGroup, role, Service);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        [TestMethod]
        public async Task GroupsTest()
        {
            Manager.AddChild(ActorGroup, User);
            Manager.AddChild(TargetGroup, Service);
            TestRole role = NewRole();
            role.AssignOperation(StartOperation);
            Manager.AssignRole(ActorGroup, role, TargetGroup);
            await Manager.Save();

            var canDo = await Manager.CanDoOperation(User, StartOperation, Service);

            IsTrue(canDo);
        }

        private async Task AssignOperation(params Operation[] operations)
        {
            var role = NewRole();
            operations.ForEach(o => role.AssignOperation(o));
            Manager.AssignRole(User, role, Service);
            await Manager.Save();
        }

        private static TestRole NewRole()
        {
            return new TestRole(Guid.NewGuid());
        }
    }
}
