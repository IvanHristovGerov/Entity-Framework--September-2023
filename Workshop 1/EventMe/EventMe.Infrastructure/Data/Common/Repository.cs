using EventMe.Infrastructure.Data.Contracts;
using Microsoft.EntityFrameworkCore;


namespace EventMe.Infrastructure.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Repository : IRepository
    {
        private readonly EventMeDbContext dbContext;

        /// <summary>
        /// Constructor of injecting the context in DB
        /// </summary>
        /// <param name="_dbContext"></param>
        public Repository(EventMeDbContext _dbContext)
        {

            dbContext = _dbContext;

        }

        /// <summary>
        /// Returns DbSet for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private DbSet<T> DbSet<T>() where T : class => dbContext.Set<T>();

        /// <summary>
        /// Adding element in the DB
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="entity">Element</param>
        /// <returns></returns>
        public async Task AddAsync<T>(T entity) where T : class
        {
           await DbSet<T>().AddAsync(entity);
        }

        /// <summary>
        /// Getting all elements from the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>();
        }

        /// <summary>
        /// Getting all elements from the table including deleted ones - readonly
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns></returns>
        public IQueryable<T> AllReadonly<T>() where T : class
        {
            return DbSet<T>()
                .AsNoTracking();
        }

        /// <summary>
        /// Saving changes in the DB
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
           return await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Deleting an element in the DBB
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="entity">Element</param>
        void IRepository.Delete<T>(T entity)
        {
            entity.IsActive = false;
            entity.DeletedOn = DateTime.Now;
        }


        /// <summary>
        /// Getting all elements from the table including deleted ones
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns></returns>
        IQueryable<T> IRepository.AllWithDeleted<T>()
        {
            return DbSet<T>()
                .IgnoreQueryFilters();
        }
        /// <summary>
        /// Getting all elements from the table including deleted ones - readonly
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns></returns>
        IQueryable<T> IRepository.AllWithDeletedReadonly<T>()
        {
            return DbSet<T>()
                .IgnoreQueryFilters()
                .AsNoTracking();
        }
    }
}
