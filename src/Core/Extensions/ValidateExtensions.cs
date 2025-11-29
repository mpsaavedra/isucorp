// -----------------------------------------------------------------------
//  <copyright file="ValidateExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Exceptions;

namespace IsuCorp.Extensions;

/// <summary>
/// Validation extensions
/// </summary>
public static class ValidateExtensions
{
    /// <summary>
    /// Validates that source is not null, if is null it throws an <see cref="DetailedException"/>
    /// with information about the error.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="DetailedException"></exception>
    public static object? ToValidate(this object? source, string? code, string? message = null)
    {
        message ??= "Validation failure, value is null";
        code ??= "SG-00004";
        if (source is not null) return source;
        var ex = new DetailedException(code, message);
        Guards.Except.RegisterException<DetailedException>(ex, message);
        throw ex;
    }
}