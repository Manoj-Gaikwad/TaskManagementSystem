
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace TaskManagementSystem.Extentions
{
    public static class DbSetExtensions
    {
        public static IQueryable<T> AsEfQueryable<T>(this DbSet<T> dbSet)
          where T : class
        {
            return dbSet.AsQueryable();
        }
    }
}
