using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachine.Tests
{
    [TestClass]
    class DITests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var services = new ServiceCollection();

            services.AddStateMachines(registry =>
            {
                registry.AddStateMachine(new StateMachineBuilder<State, Operation, string>()
                                            .SetStart(State.Start)
                                            .WithTransition(State.Start, Operation.StartToFinish, State.Finish)
                                            .WithTransition(State.Start, Operation.StartToMiddle, State.Middle)
                                            .WithTransition(State.Middle, Operation.MiddleToFinish, State.Finish));
            });
        }
    }
}
