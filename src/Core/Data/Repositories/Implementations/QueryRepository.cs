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
using IsuCorp.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ILogger = IsuCorp.Logging.ILogger;
using ILoggerService = IsuCorp.Logging.ILoggerService;

// ReSharper disable once CheckNamespace
namespace IsuCorp.Data.Repositories;

public class QueryRepository<TKey, TUserKey, TEntity, TContext>(IUnitOfWork unitOfWork, ILoggerService loggerService) :
    IQueryRepository<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>, new()
    where TContext : DbContext, IDbContextBase
{
    private readonly ILogger _logger = loggerService.CreateLogger<QueryRepository<TKey, TUserKey, TEntity, TContext>>();

    public async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0,
        int pageSize = 50, bool disableTracking = true, bool ignoreQueryFilters = false,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? action = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Debug("Retrieving entities that match the predicate");
            var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
            ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
            return await ctx
                .Set<TEntity>()
                .ToQueryable()
                .ToNoTracking(disableTracking)
                .ToIncludeSoftDeleted(includeSoftDeleted)
                .ToOrderBy(orderBy)
                .ToInclude(include)
                .ToPaginated(pageIndex, pageSize)
                .ToWhereAsync(predicate, action, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<IQueryable<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TResult>>? selector = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0,
        int pageSize = 50, bool disableTracking = true,
        bool ignoreQueryFilters = false, bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? action = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Debug("Retrieving entities that match the predicate");
            var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
            ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
            return await ctx
                .Set<TEntity>()
                .ToQueryable()
                .ToNoTracking(disableTracking)
                .ToIncludeSoftDeleted(includeSoftDeleted)
                .ToOrderBy(orderBy)
                .ToInclude(include)
                .ToWhere(predicate)
                .ToPaginated(pageIndex, pageSize)
                .ToSelectAsync(selector, action, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool disableTracking = true, bool ignoreQueryFilters = false, bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Debug("Retrieving first entity that matches the predicate");
            var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
            ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
            return await ctx
                .Set<TEntity>()
                .ToQueryable()
                .ToNoTracking(disableTracking)
                .ToIncludeSoftDeleted(includeSoftDeleted)
                .ToOrderBy(orderBy)
                .ToInclude(include)
                .ToWhere(predicate)
                .ToFirstOrDefaultAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TResult>>? selector = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool disableTracking = true, bool ignoreQueryFilters = false, bool includeSoftDeleted = false,
        Func<IQueryable<TResult>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Debug("Retrieving first entity that matches the predicate");
            var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
            ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
            return await ctx
                .Set<TEntity>()
                .ToQueryable()
                .ToNoTracking(disableTracking)
                .ToIncludeSoftDeleted(includeSoftDeleted)
                .ToOrderBy(orderBy)
                .ToInclude(include)
                // .ToWhere<TEntity>(predicate, notification)
                .ToWhere(predicate)
                .ToSelect(selector)
                .ToFirstOrDefaultAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null, bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
            ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
            return await ctx.Set<TEntity>()
                .ToQueryable()
                .ToIncludeSoftDeleted(includeSoftDeleted)
                .ToWhere(predicate, notification)
                .ToAnyAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null, bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ctx = await unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken);
            ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
            return await ctx.Set<TEntity>()
                .ToQueryable()
                .ToWhere(predicate, notification)
                .ToIncludeSoftDeleted(includeSoftDeleted)
                .ToLongCountAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
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