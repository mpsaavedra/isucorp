// -----------------------------------------------------------------------
//  <copyright file="IQueryRepository.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Data.Operations;
using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data.Repositories;

/// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}"/>
public interface IQueryRepository<TEntity> :
    IQueryRepository<long, TEntity>
    where TEntity : class, IBusinessEntity
{
}

/// <inheritdoc cref="IQueryRepository{TKey,TUserKey,TEntity,TContext}"/>
public interface IQueryRepository<TKey, TEntity> :
    IQueryRepository<TKey, TKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey>
{
}

/// <summary>
/// Repository interface for read-only query operations.
/// Provides methods for querying entities without modifying them.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
public interface IQueryRepository<TKey, TUserKey, TEntity> : 
    IReadOperation<TKey, TUserKey, TEntity>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
{
    /// <summary>
    /// Gets the base queryable for the entity type.
    /// Use this to build custom LINQ queries.
    /// </summary>
    IQueryable<TEntity> Query { get; }
    
    /// <summary>
    /// Gets the DbSet for a specific entity type.
    /// Useful for accessing related entities or performing direct DbSet operations.
    /// </summary>
    /// <typeparam name="T">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
    /// <returns>The DbSet for the specified entity type</returns>
    DbSet<T> Entity<T>() where T : class, IBusinessEntity<TKey, TUserKey>;
}