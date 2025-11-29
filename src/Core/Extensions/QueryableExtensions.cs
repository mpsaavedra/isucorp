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
        Expression<Func<T, TResult>>? selector = null) =>
        selector == null ? source as IQueryable<TResult> : source.Select(selector);
}