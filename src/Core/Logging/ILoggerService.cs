// -----------------------------------------------------------------------
//  <copyright file="ILoggerService.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Logging;

namespace IsuCorp.Logging;

/// <summary>
/// Define <see cref="ILogger"/> service
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// Create a new logger
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    ILogger CreateLogger(string title);

    /// <summary>
    /// Create a new logger for a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ILogger CreateLogger<T>();
}