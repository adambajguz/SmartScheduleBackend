﻿namespace SmartSchedule.Application.DAL.Interfaces.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using SmartSchedule.Domain.Entities.Base;

    public interface IGenericReadOnlyRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId> 
        where TId : IComparable
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TEntity> GetOneAsync(
        Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> GetFirstAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TEntity> GetByIdAsync(TId id);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);
    }
}
