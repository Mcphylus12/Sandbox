using Commander;

namespace Application
{
    internal class TestCommandResolver : ICommandResolver
    {
        public ICommand<TCommandParams, TResult> ResolveCommandImplementation<TCommandParams, TResult>() 
            where TCommandParams : ICommandParams<TResult>
        {
            var resultType = typeof(TResult);
            var paramType = typeof(TCommandParams);
            if (resultType == typeof(string) && paramType == typeof(TestParam))
            {
                return new TestCommand() as ICommand<TCommandParams, TResult>;
            }

            return null;
        }
    }
}