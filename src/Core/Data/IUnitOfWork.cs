// -----------------------------------------------------------------------
//  <copyright file="IUnitOfWork.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data;

/// <summary>
/// The Unit of Work pattern ensures that all changes made during a business transaction
/// are committed together or rolled back together, maintaining data consistency.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Executes an operation within a database transaction.
    /// The operation is executed atomically - either all changes are committed, or all are rolled back.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the operation</typeparam>
    /// <param name="operation">The operation to execute within the transaction</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the operation's result.</returns>
    Task<TResult?> ExecuteAsync<TResult>(
        Func<Task<TResult>> func,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// gets the <see cref="DbContextFactory{TContext}"/>
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    IDbContextFactory<TContext> DbContextFactory<TContext>() 
        where TContext : DbContext, IDbContextBase;

    /// <summary>
    /// Gets a repository for the specified entity type.
    /// The repository must be registered in the dependency injection container.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns>The repository instance for the specified entity type</returns>
    IRepository<TKey, TUserKey, TEntity, TContext> Repository<TKey, TUserKey, TEntity, TContext>() 
        where TEntity : class, IBusinessEntity<TKey, TUserKey>, new() 
        where TContext : DbContext, IDbContextBase<TUserKey>;

}