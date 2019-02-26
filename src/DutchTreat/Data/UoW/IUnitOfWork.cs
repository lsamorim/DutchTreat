using System;
using System.Threading.Tasks;

namespace DutchTreat.Data.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();

        Task<bool> CommitAsync();
    }
}
