// -----------------------------------------------------------------------
//  <copyright file="IDeleteOperation.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data.Operations;

/// <summary>
/// Interface for delete operations that remove entities from the database.
/// Supports both soft delete (marking as deleted) and hard delete (permanent removal).
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
/// <typeparam name="TContext">The DbContext type</typeparam>
public interface IDeleteOperation<TKey, TUserKey, TEntity, out TContext> 
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext : DbContext
{
    /// <summary>
    /// Deletes an entity from the database asynchronously by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    /// <param name="softDeleted">If true, performs a soft delete (marks as deleted). If false, performs a hard delete (permanent removal).</param>
    /// <param name="notification">Optional action to perform after deleting (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(TKey id, bool softDeleted = true,
        Func<TEntity, TContext, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes multiple entities from the database asynchronously in a batch operation.
    /// </summary>
    /// <param name="ids">The IDs of the entities to delete</param>
    /// <param name="softDeleted">If true, performs soft deletes (marks as deleted). If false, performs hard deletes (permanent removal).</param>
    /// <param name="notification">Optional action to perform after deleting (e.g., notifications, logging)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of tuples with each entity ID and whether it was successfully deleted.</returns>
    Task<(TKey EntityId, bool Deleted)[]> DeleteRangeAsync(IEnumerable<TKey> ids, bool softDeleted = true,
        Func<IEnumerable<(TKey EntityId, bool Updated)>, TContext, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default);
}