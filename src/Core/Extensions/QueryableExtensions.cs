// -----------------------------------------------------------------------
//  <copyright file="QueryableExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Linq.Expressions;
using IsuCorp.Data;
using IsuCorp.Data.ShadowProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IsuCorp.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ToQueryable<T>(this DbSet<T> set)  
        where T: class=>
        set.AsQueryable();
    
    public static IQueryable<T> ToIncludeSoftDeleted<T>(this IQueryable<T> source, bool includeSoftDeleted = true)
        where T : class, ISoftDeleted => 
        includeSoftDeleted ? source : source.Where(e => e.Deleted == false);

    public static IQueryable<T> ToNoTracking<T>(this IQueryable<T> source, bool asNoTracking = true)
        where T : class => 
        asNoTracking ? source : source.AsNoTracking();
    
    public static IQueryable<T> ToOrderBy<T>(this IQueryable<T> source, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) =>
        orderBy == null ? source : orderBy(source);
    
    public static IQueryable<T> ToInclude<T>(this IQueryable<T> source,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null) =>
        include == null ? source : include(source);

    public static IQueryable<T> ToPaginated<T>(this IQueryable<T> source,
        int pageIndex = 1, int pageSize = 50)
    {
        if (pageIndex <= 0 || pageSize <= 0) return source;
        return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    public static IQueryable<TResult> ToSelect<T, TResult>(this IQueryable<T> source,
        Expression<Func<T, TResult>>? selector = null,
        Func<IQueryable<T>, Task<bool>>? action = null)
    {
        action?.Invoke(source);
        return selector == null ? source as IQueryable<TResult> : source.Select(selector);
    }

    public static async Task<IQueryable<TResult>> ToSelectAsync<T, TResult>(this IQueryable<T> source,
        Expression<Func<T, TResult>>? selector = null,
        Func<IQueryable<T>, Task<bool>>? action = null,
        CancellationToken cancellationToken = default)
    {
        action?.Invoke(source);
        return await Task.Run(
            () => selector == null ? source as IQueryable<TResult> : source.Select(selector), 
            cancellationToken);
    }

    public static IQueryable<T> ToWhere<T>(this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, Task<bool>>? notification = null)
    {
        notification?.Invoke(queryable).GetAwaiter();
        return queryable.Where(predicate);
    }

    public static async Task<IQueryable<T>> ToWhereAsync<T>(this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        await notification?.Invoke(queryable);
        return await Task.Run(() => queryable.Where(predicate), cancellationToken);
    }

    public static async Task<T> ToFirstOrDefaultAsync<T>(this IQueryable<T> source,
        Func<IQueryable<T>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        await notification?.Invoke(source);
        return await source.FirstOrDefaultAsync(cancellationToken);
    }

    public static async Task<bool> ToAnyAsync<T>(this IQueryable<T> source,
        Func<IQueryable<T>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        notification?.Invoke(source);
        return await source.AnyAsync(cancellationToken);
    }

    public static async Task<long> ToLongCountAsync<T>(this IQueryable<T> source,
        Func<IQueryable<T>, Task<bool>>? notification = null,
        CancellationToken cancellationToken = default)
    {
        notification?.Invoke(source);
        return await source.LongCountAsync(cancellationToken);
    }
}