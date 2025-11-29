// -----------------------------------------------------------------------
//  <copyright file="LoggerFileInterval.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Logging.Settings;

/// <summary>
/// Log file creation interval
/// </summary>
public enum LoggerFileInterval
{
    Infinite = 0,
    Year = 1,
    Month = 2,
    Day = 3,
    Hour = 4,
    Minute = 5
}