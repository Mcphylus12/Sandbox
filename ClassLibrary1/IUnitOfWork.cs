using System;

namespace Repository
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetConcreteRepository<TRepository>();

        IRepository<TModel> GetRepository<TModel>();
    }
}
