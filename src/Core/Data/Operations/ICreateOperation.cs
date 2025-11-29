// -----------------------------------------------------------------------
//  <copyright file="IReadOperation.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data.Operations;

/// <summary>
/// Interface for create operations that add new entities to the database.
/// Provides methods for adding single entities or batches of entities.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
/// <typeparam name="TContext">The DbContext type</typeparam>
public interface ICreateOperation<TKey, TUserKey, TEntity, out TContext> 
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// Adds a new entity to the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="notification">Optional action to perform after adding (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added entity with generated ID, or null if the operation failed.</returns>
    Task<TEntity?> AddAsync(TEntity entity, Func<TEntity, TContext, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple entities to the database asynchronously in a batch operation.
    /// </summary>
    /// <param name="notification">Optional action to perform after adding (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <param name="entities">The entities to add</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of tuples with each entity and whether it was successfully added.</returns>
    Task<(TEntity Entity, bool Added)[]> AddRangeAsync(Func<TEntity[], TContext, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default, params TEntity[] entities);
}