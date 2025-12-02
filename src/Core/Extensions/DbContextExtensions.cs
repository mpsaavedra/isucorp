// -----------------------------------------------------------------------
//  <copyright file="DbContextExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Data;
using IsuCorp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IsuCorp.Extensions;

/// <summary>
/// <see cref="DbContext"/> related extensions
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Saves data into the database, and while doing it, it updates the auditable information of the
    /// entities.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="notification"></param>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static Task<int> SaveEntitiesChangesAsync<TContext>(this TContext context,
        CancellationToken cancellationToken = default, Action<object?>? notification = null)
        where TContext : DbContext, IDbContextBase<long> =>
        context.SaveEntitiesChangesAsync<long, long, TContext>(cancellationToken, notification);
    
    /// <summary>
    /// Saves data into the database, and while doing it, it updates the auditable information of the
    /// entities.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="notification"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static Task<int> SaveEntitiesChangesAsync<TKey, TContext>(this TContext context,
        CancellationToken cancellationToken = default, Action<object?>? notification = null)
        where TContext : DbContext, IDbContextBase<TKey> =>
        context.SaveEntitiesChangesAsync<TKey, TKey, TContext>(cancellationToken, notification);


    /// <summary>
    /// Saves data into the database, and while doing it, it updates the auditable information of the
    /// entities.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="notification"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static async Task<int> SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(this TContext context,
        CancellationToken cancellationToken = default, Action<object?>? notification = null)
        where TContext : DbContext, IDbContextBase<TUserKey>
    {
        var entries = context.ChangeTracker.Entries().Where(x => x.Entity is IBusinessEntity<TKey, TUserKey>);
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        var added = entityEntries.Where(e => e.State == EntityState.Added);
        var modified = entityEntries.Where(e => e.State == EntityState.Modified);
        var deleted = entityEntries.Where(e => e.State == EntityState.Deleted);

        var addedEntries = added as EntityEntry[] ?? added.ToArray();
        var modifiedEntries = modified as EntityEntry[] ?? modified.ToArray();
        var deletedEntries = deleted as EntityEntry[] ?? deleted.ToArray();

        if (addedEntries.Length != 0)
            context.ProcessAddedEntities<TKey, TUserKey, TContext>(addedEntries,  notification);
        if (modifiedEntries.Length != 0)
            context.ProcessModifiedEntities<TKey, TUserKey, TContext>(modifiedEntries,  notification);
        if (deletedEntries.Length != 0)
            context.ProcessDeletedEntities<TKey, TUserKey, TContext>(deletedEntries,  notification);


        var result = await context.SaveChangesAsync(cancellationToken);

        notification?.Invoke(new
        {
            Message = Messages.Data.EntitiesSavedSuccessfully.Msg(),
            Added = addedEntries.Length,
            Modified = modifiedEntries.Length,
            Deleted = deletedEntries.Length
        });

        return result;
    }

    private static void ProcessAddedEntities<TKey, TUserKey, TContext>(this TContext context, IEnumerable<EntityEntry> entries
        , Action<object?>? notification = null)
        where TContext : DbContext, IDbContextBase<TUserKey>
    {
        // get userId from context if is null invoke CurrentUserService to get UserId
        var currentUserService = context.GetService<ICurrentUserService>();
        var userId = context.UserId ?? currentUserService.GetCurrentUserId<TUserKey>();
        userId.ToValidate(Messages.Data.CouldNotRetrieveUserId.Msg());

        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is IBusinessEntity<TKey, TUserKey>)
                    {
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).CreatedAt = DateTime.UtcNow;
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).CreatedBy = userId;
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).RowVersion = Guid.NewGuid().ToString();
                    }

                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Modified:
                default:
                    throw new ApplicationException($"Entry {entry} is not Added state");
            }
        }

        notification?.Invoke(context);
    }

    
    private static void ProcessModifiedEntities<TKey, TUserKey, TContext>(this TContext context,
        IEnumerable<EntityEntry> entries,
        Action<object?>? notification = null)
        where TContext : DbContext, IDbContextBase<TUserKey>
    {
        // get userId from context if is null invoke CurrentUserService to get UserId
        var currentUserService = context.GetService<ICurrentUserService>();
        var userId = context.UserId ?? currentUserService.GetCurrentUserId<TUserKey>();
        userId.ToValidate(Messages.Data.CouldNotRetrieveUserId.Msg());

        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    // ReSharper disable once MergeCastWithTypeCheck
                    if(entry.Entity is IBusinessEntity<TKey, TUserKey>)
                    {
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).UpdatedAt = DateTime.UtcNow;
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).UpdatedBy = userId;
                    }
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Modified state");
            }
        }

        notification?.Invoke(context);
    }

    
    private static void ProcessDeletedEntities<TKey, TUserKey, TContext>(this TContext context, IEnumerable<EntityEntry> entries,
        Action<object?>? notification = null)
        where TContext : DbContext, IDbContextBase<TUserKey>
    {
        // get userId from context if is null invoke CurrentUserService to get UserId
        var currentUserService = context.GetService<ICurrentUserService>();
        var userId = context.UserId ?? currentUserService.GetCurrentUserId<TUserKey>();
        userId.ToValidate(Messages.Data.CouldNotRetrieveUserId.Msg());

        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    // ReSharper disable once MergeCastWithTypeCheck
                    if(entry.Entity is IBusinessEntity<TKey, TUserKey>)
                    {
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).DeletedAt = DateTime.UtcNow;
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).DeletedBy = userId;
                        ((IBusinessEntity<TKey, TUserKey>)entry.Entity).Deleted = true;
                    }
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Modified:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Deleted state");
            }
        }

        notification?.Invoke(context);
    }

    /// <summary>
    /// Execute provided operation in a transactional and resilient environment
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="operation"></param>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static async Task<TResult> ExecuteAsync<TKey, TUserKey, TResult, TContext>(this TContext ctx,
        Func<Task<TResult>>? action = null, Action<TResult>? notification = null,
        CancellationToken cancellationToken = default)
        where TContext : DbContext, IDbContextBase<TUserKey>
    {
        var executionStrategy = ctx.Database.CreateExecutionStrategy();
        if (ctx.Database.ProviderName == null || ctx.Database.ProviderName.Contains("InMemory"))
        {
            // InMemory does no support transactions
            // this should be in tests only
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var result = await action.Invoke();
                if (ctx is IDbContextBase)
                    await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(cancellationToken,
                        async void (_) =>
                        {
                            notification.Invoke(result);
                        });
                else
                    await ctx.SaveChangesAsync(cancellationToken);
                return result;
            });
        }

        return await executionStrategy.ExecuteAsync(async ct =>
        {
            await using var transaction = await ctx.Database.BeginTransactionAsync(ct);
            try
            {
                var result = await action?.Invoke();

                if (ctx is IDbContextBase)
                    await ctx.SaveEntitiesChangesAsync<TKey, TUserKey, TContext>(ct,
                        async void (_) =>
                        {
                            notification?.Invoke(result);
                        });
                else
                    await ctx.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }, cancellationToken);

    }
}