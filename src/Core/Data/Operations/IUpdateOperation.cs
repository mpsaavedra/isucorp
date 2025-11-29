// -----------------------------------------------------------------------
//  <copyright file="IUpdateOperation.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data.Operations;

/// <summary>
/// Interface for update operations that modify existing entities in the database.
/// Provides methods for updating single entities or batches of entities.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
/// <typeparam name="TContext">The DbContext type</typeparam>
public interface IUpdateOperation<TKey, TUserKey, TEntity, out TContext> 
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// Updates an existing entity in the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity with updated values</param>
    /// <param name="notification">Optional action to perform after updating (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
    Task<bool> UpdateAsync(TEntity entity, Func<TEntity, TContext, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates multiple entities in the database asynchronously in a batch operation.
    /// </summary>
    /// <param name="entities">The entities with updated values</param>
    /// <param name="notification">Optional action to perform after updating (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of tuples with each entity ID and whether it was successfully updated.</returns>
    Task<(TKey EntityId, bool Updated)[]> UpdateRangeAsync(IEnumerable<TEntity> entities,
        Func<IEnumerable<(TKey EntityId, bool Updated)>, TContext, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);
    
}