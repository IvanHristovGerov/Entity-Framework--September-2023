using EventMe.Infrastructure.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Data.Common
{
    /// <summary>
    /// Repository - method of access to data
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adding element in the DB
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="entity">Element</param>
        /// <returns></returns>
        Task AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Deleting an element in the DBB
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="entity">Element</param>
        void Delete<T>(T entity) where T:class, IDeletable;

        /// <summary>
        /// Getting all elements from the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> All<T>() where T : class;

        /// <summary>
        /// Getting all elements from the table including deleted ones
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns></returns>
        IQueryable<T> AllWithDeleted<T>() where T : class, IDeletable;

        /// <summary>
        /// Getting all elements from the table readonly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> AllReadonly<T>() where T : class;

        /// <summary>
        /// Getting all elements from the table including deleted ones - readonly
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns></returns>
        IQueryable<T> AllWithDeletedReadonly<T>() where T : class, IDeletable;


        /// <summary>
        /// Saving changes in the DB
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
