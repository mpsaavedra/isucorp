// -----------------------------------------------------------------------
//  <copyright file="ILogger.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Logging;

public interface ILogger
{
    /// <summary>
    /// Logs anything that happens at a lower level
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    ILogger Trace(string message, object? data = null);
    
    /// <summary>
    /// Logs internal events in debug mode
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    ILogger Debug(string message, object? data = null);
    
    /// <summary>
    /// Logs system events
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    ILogger Info(string message, object? data = null);
    
    /// <summary>
    /// Logs custom errors or when are throws by a handled exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    ILogger Error(string message, object? data = null);
    
    /// <summary>
    /// Logs custom errors or when are throws by a handled exception
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    ILogger Error(Exception ex);
}