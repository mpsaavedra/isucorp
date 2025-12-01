// -----------------------------------------------------------------------
//  <copyright file="IReadOperation.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IsuCorp.Data.Operations;

/// <summary>
/// Interface for read operations that query entities from the database.
/// Provides methods for retrieving entities with filtering, sorting, pagination, and eager loading support.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
public interface IReadOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
{
    /// <summary>
    /// Retrieves a queryable collection of entities with optional filtering, sorting, pagination, and eager loading.
    /// If includeSoftDeleted is false, soft-deleted entities are excluded from the results.
    /// </summary>
    /// <param name="predicate">Optional filter expression to apply to the query</param>
    /// <param name="orderBy">Optional ordering function to sort the results</param>
    /// <param name="include">Optional function to include related entities (eager loading)</param>
    /// <param name="pageIndex">Zero-based page index for pagination (default: 0)</param>
    /// <param name="pageSize">Number of items per page (default: 50)</param>
    /// <param name="disableTracking">If true, disables change tracking for better read performance (default: true)</param>
    /// <param name="ignoreQueryFilters">If true, ignores global query filters (e.g., soft delete, multi-tenancy)</param>
    /// <param name="includeSoftDeleted">If true, includes soft-deleted entities in the results</param>
    /// <param name="action">Optional action to perform after retrieving data (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a queryable collection of entities.</returns>
    Task<IQueryable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0, int pageSize = 50,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? action = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a queryable collection of projected results with optional filtering, sorting, pagination, and eager loading.
    /// Projects entities to a different type (DTO, view model, etc.) before materializing the query.
    /// </summary>
    /// <typeparam name="TResult">The type to project entities to</typeparam>
    /// <param name="predicate">Optional filter expression to apply to the query</param>
    /// <param name="selector">Expression to project entities to the result type</param>
    /// <param name="orderBy">Optional ordering function to sort the results</param>
    /// <param name="include">Optional function to include related entities (eager loading)</param>
    /// <param name="pageIndex">Zero-based page index for pagination (default: 0)</param>
    /// <param name="pageSize">Number of items per page (default: 50)</param>
    /// <param name="disableTracking">If true, disables change tracking for better read performance (default: true)</param>
    /// <param name="ignoreQueryFilters">If true, ignores global query filters</param>
    /// <param name="includeSoftDeleted">If true, includes soft-deleted entities in the results</param>
    /// <param name="action">Optional action to perform after retrieving data</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a queryable collection of projected results.</returns>
    Task<IQueryable<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TResult>>? selector = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0, int pageSize = 50,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? action = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves the first entity that matches the predicate, or null if no match is found.
    /// </summary>
    /// <param name="predicate">Optional filter expression to apply to the query</param>
    /// <param name="orderBy">Optional ordering function to sort the results before taking the first</param>
    /// <param name="include">Optional function to include related entities (eager loading)</param>
    /// <param name="disableTracking">If true, disables change tracking for better read performance (default: true)</param>
    /// <param name="ignoreQueryFilters">If true, ignores global query filters</param>
    /// <param name="includeSoftDeleted">If true, includes soft-deleted entities in the results</param>
    /// <param name="notification">Optional action to perform after retrieving data</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first matching entity, or null if not found.</returns>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves the first projected result that matches the predicate, or null if no match is found.
    /// </summary>
    /// <typeparam name="TResult">The type to project the entity to</typeparam>
    /// <param name="predicate">Optional filter expression to apply to the query</param>
    /// <param name="selector">Expression to project the entity to the result type</param>
    /// <param name="orderBy">Optional ordering function to sort the results before taking the first</param>
    /// <param name="include">Optional function to include related entities (eager loading)</param>
    /// <param name="disableTracking">If true, disables change tracking for better read performance (default: true)</param>
    /// <param name="ignoreQueryFilters">If true, ignores global query filters</param>
    /// <param name="includeSoftDeleted">If true, includes soft-deleted entities in the results</param>
    /// <param name="notification">Optional action to perform after retrieving data</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first projected result, or null if not found.</returns>
    Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, TResult>>? selector = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool includeSoftDeleted = false,
        Func<IQueryable<TResult>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entity matches the predicate.
    /// If no predicate is specified, returns true if there are any active (non-soft-deleted) entities.
    /// </summary>
    /// <param name="predicate">Optional filter expression to apply to the query</param>
    /// <param name="notification">Optional action to perform before checking</param>
    /// <param name="includeSoftDeleted">If true, includes soft-deleted entities in the check</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether any matching entity exists.</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of entities that match the predicate as a long integer.
    /// </summary>
    /// <param name="predicate">Optional filter expression to apply to the query</param>
    /// <param name="notification">Optional action to perform before counting</param>
    /// <param name="includeSoftDeleted">If true, includes soft-deleted entities in the count</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the count of matching entities.</returns>
    Task<long> LongCountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, Task<bool>>? notification = null,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default);
}