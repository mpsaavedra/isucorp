// -----------------------------------------------------------------------
//  <copyright file="CommandRepository.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------


using IsuCorp.Exceptions;
using IsuCorp.Services;
// ReSharper disable once CheckNamespace
using IsuCorp.Extensions;

namespace IsuCorp.Data.Repositories;

using IsuCorp.Logging;
using Microsoft.EntityFrameworkCore;

public class CommandRepository<TKey, TUserKey, TEntity, TContext>
    (IUnitOfWork unitOfWork, ILoggerService loggerService, ICurrentUserService currentUserService)
    : ICommandRepository<TKey, TUserKey, TEntity, TContext>
    where TEntity : class, IBusinessEntity<TKey, TUserKey>, new()
    where TContext : DbContext, IDbContextBase<TUserKey>

{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger _logger = loggerService.CreateLogger<QueryRepository<TKey, TUserKey, TEntity, TContext>>();

    public async Task<TEntity?> AddAsync(TEntity entity, Func<TEntity, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                var result = await ctx.Set<TEntity>().AddAsync(entity, cancellationToken);
                var savedResult = await ctx.SaveChangesAsync(cancellationToken);
                notification?.Invoke(entity);
                return result.Entity;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<List<(TEntity Entity, bool Added)>> AddRangeAsync(
        Func<List<(TEntity Entity, bool Added)>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default,
        params TEntity[] entities)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                var result = new List<(TEntity Entity, bool Added)>();
                foreach (var entity in entities)
                {
                    var resultEntity = await ctx.AddAsync(entity, cancellationToken);
                    result.Add((entity, resultEntity.State.HasFlag(EntityState.Added)));
                }
                var saveResult = await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(
                    cancellationToken,
                    o =>
                    {
                        notification.Invoke(result);
                    });
                return result;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(TKey id, bool softDeleted = true,
        Func<TEntity, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                var entity = await ctx.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
                if (entity is null)
                {
                    throw new DetailedException(
                        Messages.Data.EntityCouldNotBeFound.Code,
                        Messages.Data.EntityCouldNotBeFound.Msg(entity.Id));
                }
                if(!softDeleted)
                    ctx.Set<TEntity>().Remove(entity);
                else
                {
                    entity.Deleted = true;
                    entity.DeletedAt = DateTime.UtcNow;
                    entity.DeletedBy = currentUserService.GetCurrentUserId<TUserKey>();
                }
                var saveResult = await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(
                    cancellationToken,
                    o =>
                    {
                        notification.Invoke(entity);
                    });
                return saveResult > 0;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<List<(TKey EntityId, bool Deleted)>> DeleteRangeAsync(IEnumerable<TKey> ids, bool softDeleted = true,
        Func<IEnumerable<(TKey EntityId, bool Updated)>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                var result = new List<(TKey EntityId, bool deleted)>();
                foreach (var id in ids)
                {
                    var entity = await ctx.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
                    if (entity is null)
                    {
                        result.Add((id, false));
                        continue;
                    }
                    if(!softDeleted)
                        ctx.Set<TEntity>().Remove(entity);
                    else
                    {
                        entity.Deleted = true;
                        entity.DeletedAt = DateTime.UtcNow;
                        entity.DeletedBy = currentUserService.GetCurrentUserId<TUserKey>();
                    }
                    result.Add((entity.Id, true));
                }
                var saveResult = await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(
                    cancellationToken,
                    o =>
                    {
                        notification.Invoke(result);
                    });
                return result;
                
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(TEntity entity, Func<TEntity, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                
                if (!ctx.Set<TEntity>().Any(x => x.Id.Equals(entity.Id)))
                {
                    throw new DetailedException(
                        Messages.Data.EntityCouldNotBeFound.Code,
                        Messages.Data.EntityCouldNotBeFound.Msg(entity.Id));
                    return false;
                }
                
                ctx.Set<TEntity>().Update(entity);
                return await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(cancellationToken,
                    async o =>
                    {
                        await notification?.Invoke(entity);
                    }) > 0;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<List<(TKey EntityId, bool Updated)>> UpdateRangeAsync(IEnumerable<TEntity> entities,
        Func<IEnumerable<(TKey EntityId, bool Updated)>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                var result = new List<(TKey EntityId, bool Updated)>();
                foreach (var entity in entities)
                {
                    if (!ctx.Set<TEntity>().Any(x => x.Id.Equals(entity.Id)))
                    {
                        result.Add((entity.Id, false));
                        continue;
                    }
                    ctx.Set<TEntity>().Update(entity);
                    result.Add((entity.Id, true));
                }
                var saveResult = await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(
                    cancellationToken,
                    o =>
                    {
                        notification.Invoke(result);
                    });
                return result;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }

    public async Task<bool>? ChangeStatusAsync(TKey id, bool status, Func<TEntity, Task<bool>>? action = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _unitOfWork.ExecuteAsync(async () =>
            {
                var ctx = await _unitOfWork.DbContextFactory<TContext>().CreateDbContextAsync(cancellationToken: cancellationToken);
                ctx.ToValidate(Messages.Core.DbContextCouldNotBeCreated);
                var entity = await ctx.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
                entity.ToValidate(Messages.Data.EntityCouldNotBeFound.Code, Messages.Data.EntityCouldNotBeFound.Msg(id));
                entity.Deleted = !status;
                entity.DeletedAt = DateTime.UtcNow;
                entity.DeletedBy = currentUserService.GetCurrentUserId<TUserKey>();
                
                return await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(
                    cancellationToken,
                    o =>
                    {
                        action?.Invoke(entity);
                    }) > 0;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e);
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }
}