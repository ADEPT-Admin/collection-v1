using ACTCore.CollectionService.Infrastructure.Interface;
using SharedKernel.Data.Repositories;

namespace ACTCore.CollectionService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(AppDbContext context) : base(context)
        {
        }
    }
}