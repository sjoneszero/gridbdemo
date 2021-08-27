using GridBeyond.Database;
using GridBeyond.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GridBeyondDemo.Api.Services
{
    public abstract class BaseDbService<T> where T : class, IDbModel
    {
        protected readonly DatabaseContext DatabaseContext;

        public BaseDbService(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext; 
        } 

        /// <summary>
        /// Gets a list of entities that matches a predicate
        /// </summary>
        /// <param name="predicate">the predicate to match</param>
        /// <param name="includedPaths">string of included paths (i.e. child entities) to return</param>
        /// <param name="readOnly">Whether or not to track the retrieved entity</param>
        /// <returns>List of MarketPriceDataSets</returns>
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate = null, string includedPaths = null,  bool readOnly = false)
        {
            var query = !string.IsNullOrEmpty(includedPaths) 
                ? DatabaseContext.Set<T>().Include(includedPaths).AsSingleQuery()
                : DatabaseContext.Set<T>().AsSingleQuery();
            if (predicate != null)
                query = query.Where(predicate);

            if (readOnly)
                query = query.AsNoTracking();

            return query.ToList();
        }
        /// <summary>
        /// Gets the first entity that matches a predicate
        /// </summary>
        /// <param name="predicate">the predicate to match</param>
        /// <param name="includedPaths">string of included paths (i.e. child entities) to return</param>
        /// <param name="readOnly">Whether or not to track the retrieved entity</param>
        /// <returns>MarketPriceDataSet object</returns>
        public virtual T GetFirst(Expression<Func<T, bool>> predicate = null, string includedPaths = null, bool readOnly = false)
        {
            var query = !string.IsNullOrEmpty(includedPaths)
                ? DatabaseContext.Set<T>().Include(includedPaths).AsSingleQuery()
                : DatabaseContext.Set<T>().AsSingleQuery();
            if (predicate != null)
                query = query.Where(predicate);

            if (readOnly)
                query = query.AsNoTracking();

            return query.ToList().FirstOrDefault();
        }

        /// <summary>
        /// Inserts a new entity into the database context
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        public virtual void Insert(T entity)
        {
            DatabaseContext.Set<T>().Add(entity);
        }

        /// <summary>
        ///  Inserts a range of entities into the database context
        /// </summary>
        /// <param name="entities">The range of entities to insert</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            DatabaseContext.Set<T>().AddRange(entities);
        }

        /// <summary>
        /// Updates an entity in the database context
        /// </summary>
        /// <remarks>This method is only meant to be used for untracked (readonly) entities</remarks>
        /// <param name="entity">The entity to update</param>
        public virtual void Update(T entity)
        {
            DatabaseContext.Set<T>().Update(entity);
        }

        /// <summary>
        ///  Updates a range of entities in the database context
        /// </summary>
        /// <remarks>This method is only meant to be used for untracked (readonly) entities</remarks>
        /// <param name="entities">The range of entities to update</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            DatabaseContext.Set<T>().UpdateRange(entities);
        }

        /// <summary>
        /// Deletes an entity in the database context
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public virtual void Delete(T entity)
        {
            DatabaseContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Saves all changes made in the context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        public int Commit()
        {
            DatabaseContext.ChangeTracker.DetectChanges();
            return DatabaseContext.SaveChanges();
        }
    }
}
