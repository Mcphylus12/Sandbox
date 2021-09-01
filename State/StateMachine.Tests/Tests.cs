using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace StateMachine.Tests
{
    [TestClass]
    public class Tests
    {
        private StateMachineBuilder<State, Operation, string> stateMachineBuilder;
        private StateMachine<State, Operation, string> machine;
        private string contextString = string.Empty;

        [TestInitialize]
        public void Init()
        {
            stateMachineBuilder = new StateMachineBuilder<State, Operation, string>()
                            .SetStart(State.Start)
                            .WithTransition(State.Start, Operation.StartToFinish, State.Finish, async ctx => this.contextString = ctx.Data)
                            .WithTransition(State.Start, Operation.StartToMiddle, State.Middle)
                            .WithTransition(State.Middle, Operation.MiddleToFinish, State.Finish);

            this.machine = stateMachineBuilder
                .Build();
        }

        [TestMethod]
        public async Task SimpleTransition()
        {
            machine.State.Should().Be(State.Start);

            await machine.Process(Operation.StartToFinish);

            machine.State.Should().Be(State.Finish);
            machine.IsFinished.Should().BeTrue();
        }

        [TestMethod]
        public async Task UnSupportedOperationOnState()
        {
            machine.State.Should().Be(State.Start);

            await machine.Process(Operation.StartToMiddle);

            machine.State.Should().Be(State.Middle);

            Action testFn = () => machine.Process(Operation.StartToFinish);

            testFn.Should().Throw<NotSupportedException>().WithMessage("Unsupported transition on this state");
        }

        [TestMethod]
        public async Task CantTransitionFromStateThatHasNoOps()
        {
            machine.State.Should().Be(State.Start);

            await machine .Process(Operation.StartToFinish);

            machine.State.Should().Be(State.Finish);

            Action testFn = () => machine.Process(Operation.StartToFinish);

            testFn.Should().Throw<NotSupportedException>().WithMessage("Cannot Transition From this state");
        }

        [TestMethod]
        public async Task SaveAndLoad()
        {
            machine.State.Should().Be(State.Start);

            await machine .Process(Operation.StartToMiddle);

            machine.State.Should().Be(State.Middle);

            State savedMiddleState = machine.State;

            var newMachine = stateMachineBuilder.Build(savedMiddleState);

            newMachine.State.Should().Be(State.Middle);
            newMachine.IsFinished.Should().BeFalse();

            await newMachine.Process(Operation.MiddleToFinish);

            newMachine.State.Should().Be(State.Finish);
            newMachine.IsFinished.Should().BeTrue();
        }

        [TestMethod]
        public async Task TriggerBehaviour()
        {
            machine.State.Should().Be(State.Start);

            await machine.Process(Operation.StartToFinish, "Kek");

            this.contextString.Should().Be("Kek");
        }

        [TestMethod]
        public void SerialiseEntireMachineConfig()
        {
            throw new NotImplementedException();
        }
    }

    public enum State
    {
        Start,
        Middle,
        Finish
    }

    public enum Operation
    {
        StartToMiddle,
        MiddleToFinish,
        StartToFinish
    }
}
