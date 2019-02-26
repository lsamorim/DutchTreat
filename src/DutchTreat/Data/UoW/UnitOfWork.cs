using System.Threading.Tasks;

namespace DutchTreat.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DutchContext _context;

        public UnitOfWork(DutchContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
