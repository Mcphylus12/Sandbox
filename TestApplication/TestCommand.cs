using Commander;
using System.Threading.Tasks;

namespace Application
{
    internal class TestCommand : ICommand<TestParam, string>
    {
        public Task<string> Execute(ICommandParams<string> parameterModel)
        {
            return Task.FromResult("test resolved successfully");
        }
    }
}