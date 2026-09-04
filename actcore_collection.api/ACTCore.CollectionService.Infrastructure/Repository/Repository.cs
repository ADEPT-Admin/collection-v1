using SharedKernel.Data.Repositories;

namespace ACTCore.CollectionService.Infrastructure.Repository
{
    public class Repository<T> : BaseGenericRepository<T>
        where T : class
    {
        public Repository(AppDbContext context) : base(context)
        {
        }
    }
}
