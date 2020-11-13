using System;
using System.Collections.Generic;

namespace Commander
{
    public interface ICommandResolverStore
    {
        void AddCommandResolver(ICommandResolver resolver, List<Type> commandParamTypes);
    }
}