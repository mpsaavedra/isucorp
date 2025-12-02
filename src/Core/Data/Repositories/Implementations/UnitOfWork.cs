// -----------------------------------------------------------------------
//  <copyright file="UnitOfWork.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------


using IsuCorp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace IsuCorp.Data;

public class UnitOfWork(IServiceProvider provider) : IUnitOfWork
{
    public Task<TResult?> ExecuteAsync<TResult>(Func<Task<TResult>> func, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IDbContextFactory<TContext> DbContextFactory<TContext>() 
        where TContext : DbContext, IDbContextBase
    {
        throw new NotImplementedException();
    }

    public IRepository<TKey, TUserKey, TEntity, TContext> Repository<TKey, TUserKey, TEntity, TContext>() 
        where TEntity : class, IBusinessEntity<TKey, TUserKey>, new() where TContext : DbContext, IDbContextBase<TUserKey>
    {
        throw new NotImplementedException();
    }
}