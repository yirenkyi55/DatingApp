using System.Threading.Tasks;
using DatingApp.API.Data.Abstract;

namespace DatingApp.API.Data.Concrete
{
    public abstract class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }


        public async Task<bool> SaveAllChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}