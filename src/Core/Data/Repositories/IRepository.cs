// -----------------------------------------------------------------------
//  <copyright file="IRepository.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace IsuCorp.Data.Repositories;

/// <summary>
/// Base repository interface for entities with default key type (long).
/// </summary>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity</typeparam>
public interface IRepository<TEntity> where TEntity : class, IBusinessEntity;

/// <summary>
/// Repository interface combining query and command operations for entities with default key types.
/// </summary>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity</typeparam>
/// <typeparam name="TContext">The DbContext type that implements IDbContextBase</typeparam>
public interface IRepository<TEntity, out TContext> :
    IRepository<long, TEntity, TContext>
    where TEntity : class, IBusinessEntity
    where TContext: DbContext, IDbContextBase<long>
{
}

/// <summary>
/// Repository interface combining query and command operations for entities with custom key type.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey&gt;</typeparam>
/// <typeparam name="TContext">The DbContext type that implements IDbContextBase&lt;TKey&gt;</typeparam>
public interface IRepository<TKey, TEntity, out TContext> :
    IRepository<TKey, TKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TKey>
    where TContext: DbContext, IDbContextBase<TKey>
{
}

/// <summary>
/// Complete repository interface that combines both query and command operations.
/// Provides a unified interface for reading and modifying entities in the database.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
/// <typeparam name="TEntity">The entity type that implements IBusinessEntity&lt;TKey, TUserKey&gt;</typeparam>
/// <typeparam name="TContext">The DbContext type that implements IDbContextBase&lt;TKey, TUserKey&gt;</typeparam>
public interface IRepository<TKey, TUserKey, TEntity, out TContext> :
    IQueryRepository<TKey, TUserKey, TEntity>,
    ICommandRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>
    where TContext: DbContext, IDbContextBase<TUserKey>
{
}