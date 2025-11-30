// -----------------------------------------------------------------------
//  <copyright file="GuardIsExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Exceptions;
using IsuCorp.Guards;

namespace IsuCorp.Extensions;

/// <summary>
/// Guard Is related extensions
/// </summary>
public static class GuardIsExtensions
{
    public static bool ToIsNullOrEmpty<TSource>(this TSource source) =>
        Guards.Is.NullOrEmpty(source).HasError;

    public static TSource ToIsNullOrEmptyThrow<TSource>(this TSource source, string? parameter = null)
    {
        try
        {
            return !source.ToIsNullOrEmpty() ? source : throw new ArgumentNullException(parameter);
        }
        catch (Exception e)
        {
            Guards.Except.RegisterException<Exception>(e);
            throw;
        }
    }
}