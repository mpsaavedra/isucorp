// -----------------------------------------------------------------------
//  <copyright file="QueryRepository.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Linq.Expressions;
using IsuCorp.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IsuCorp.Data.Repositories.Implementations;

public class QueryRepository<TKey, TUserKey, TEntity, TContext>(IUnitOfWork unitOfWork) :
    IQueryRepository<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>, new()
    where TContext : DbContext, IDbContextBase
{
    public Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0,
        int pageSize = 50, bool disableTracking = true, bool ignoreQueryFilters = false, bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? action = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, TResult>>? selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0, int pageSize = 50, bool disableTracking = true,
        bool ignoreQueryFilters = false, bool includeSoftDeleted = false, Func<IQueryable<TResult>, Task<bool>>? action = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0,
        int pageSize = 50, bool disableTracking = true, bool ignoreQueryFilters = false, bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, TResult>>? selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0, int pageSize = 50, bool disableTracking = true,
        bool ignoreQueryFilters = false, bool includeSoftDeleted = false, Func<IQueryable<TResult>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, Task<bool>>? notification = null, bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, Task<bool>>? notification = null, bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> Query
    {
        get
        {
            try
            {
                var ctx = unitOfWork.DbContextFactory<TContext>().CreateDbContext();
                var query = ctx.Set<TEntity>();
                return query;
            }
            catch
            {
                Guards.Except.RegisterException<DetailedException>(new DetailedException(
                    Messages.Core.ErrorQueryRepositoryCouldNotReturnQueryableForEntity.Code,
                    Messages.Core.ErrorQueryRepositoryCouldNotReturnQueryableForEntity.Msg(typeof(TEntity))));
                throw;
            }
        }
    }
    
    public DbSet<T> Entity<T>() where T : class, IBusinessEntity<TKey, TUserKey>
    {
        try
        {
            var ctx = unitOfWork.DbContextFactory<TContext>().CreateDbContext();
            var query = ctx.Set<T>();
            return query;
        }
        catch
        {
            Guards.Except.RegisterException<DetailedException>(new DetailedException(
                Messages.Core.ErrorQueryRepositoryCouldNotReturnQueryableForEntity.Code,
                Messages.Core.ErrorQueryRepositoryCouldNotReturnQueryableForEntity.Msg(typeof(TEntity))));
            throw;
        }
    }
}