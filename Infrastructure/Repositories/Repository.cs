using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Infrastructure.Utils.ErrorUtils;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDBContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while getting entity by ID.", ex);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while getting all entities.", ex);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while getting all entities with includes.", ex);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while finding entities.", ex);
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while adding entity.", ex);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while updating entity.", ex);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while deleting entity.", ex);
            }
        }

        public async Task<TEntity> DeleteByIdAsync(int id)
        {
            try
            {
                var entityToDelete = await _dbSet.FindAsync(id);

                if (entityToDelete != null)
                {
                    _dbSet.Remove(entityToDelete);
                    await _context.SaveChangesAsync();

                    return entityToDelete;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new SqlErrorException("SQL error occurred while deleting entity.", ex);
            }
        }
    }
}

