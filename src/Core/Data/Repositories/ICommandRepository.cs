// -----------------------------------------------------------------------
//  <copyright file="ICommandRepository.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Data.Operations;
using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data.Repositories;

/// <inheritdoc cref="ICommandRepository{TEntity,TContext}"/>
public interface ICommandRepository<TEntity, out TContext> :
    ICommandRepository<long, TEntity, TContext> 
    where TEntity : class, IBusinessEntity
    where TContext : DbContext, IDbContextBase<long>
{
}

/// <inheritdoc cref="ICommandRepository{TEntity,TContext}"/>
public interface ICommandRepository<TKey, TEntity, out TContext> :
    ICommandRepository<TKey, TKey, TEntity, TContext> 
    where TEntity : class, IBusinessEntity<TKey, TKey>
    where TContext : DbContext, IDbContextBase<TKey>
{
}

/// <summary>
/// Repository interface for data modification operations (create, update, delete).
/// Provides methods for modifying entities in the database.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
/// <typeparam name="TContext">The DbContext type that implements IDbContextBase&lt;TKey, TUserKey&gt;</typeparam>
public interface ICommandRepository<TKey, TUserKey, TEntity, out TContext> :
    ICreateOperation<TKey, TUserKey, TEntity, TContext>,
    IDeleteOperation<TKey, TUserKey, TEntity, TContext>,
    IUpdateOperation<TKey, TUserKey, TEntity, TContext> 
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext: DbContext, IDbContextBase<TUserKey>
{
    /// <summary>
    /// Changes the status (active/inactive) of an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to change status</param>
    /// <param name="status">The new status value (true = active, false = inactive)</param>
    /// <param name="action">Optional action to perform after status change (e.g., notifications)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the status was changed successfully.</returns>
    Task<bool>? ChangeStatusAsync(TKey id, bool status, Func<TEntity, Task<bool>>? action = null,
        CancellationToken cancellationToken = default);
}