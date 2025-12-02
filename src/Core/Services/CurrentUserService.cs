// -----------------------------------------------------------------------
//  <copyright file="CurrentUserService.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace IsuCorp.Services;

public interface ICurrentUserService
{
    TKey GetCurrentUserId<TKey>();
}

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public TKey GetCurrentUserId<TKey>()
    {
        var httpContext = contextAccessor.HttpContext;
        if (httpContext?.User == null)
            return default!;

        // Common claim types used for subject / id
        var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? httpContext.User.FindFirst("sub")
                    ?? httpContext.User.FindFirst("user_id")
                    ?? httpContext.User.FindFirst("id");

        if (claim == null || string.IsNullOrEmpty(claim.Value))
            return default!;

        var raw = claim.Value;

        // Support common target key types
        if (typeof(TKey) == typeof(Guid) || typeof(TKey) == typeof(Guid?))
        {
            return Guid.TryParse(raw, out var g) ? (TKey)(object)g : default!;
        }

        if (typeof(TKey) == typeof(long) || typeof(TKey) == typeof(long?))
        {
            return long.TryParse(raw, out var l) ? (TKey)(object)l : default!;
        }

        if (typeof(TKey) == typeof(int) || typeof(TKey) == typeof(int?))
        {
            return int.TryParse(raw, out var i) ? (TKey)(object)i : default!;
        }

        if (typeof(TKey) == typeof(string))
        {
            return (TKey)(object)raw;
        }
        // best-effort fallback
        try
        {
            var converted = Convert.ChangeType(raw, typeof(TKey));
            return (TKey)converted!;
        }
        catch
        {
            return default!;
        }

    }
}