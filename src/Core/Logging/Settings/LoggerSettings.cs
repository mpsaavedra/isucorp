// -----------------------------------------------------------------------
//  <copyright file="LoggerSettings.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Logging.Settings;

/// <summary>
/// base <see cref="ILogger"/> settings
/// </summary>
public class LoggerSettings
{
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Error;
    public const LoggerFileInterval DefaultLoggerInterval = LoggerFileInterval.Day;
    public const string DefaultOutputTemplate= "[{Timestamp:HH:mm:ss.fff} | {Level:u3}] {Message:lj}{NewLine}{Exception}";

    private bool _enabled = false;
    private string _minimumLevel = DefaultMinimumLevel.ToString();
    private string _interval = DefaultLoggerInterval.ToString();
    private string _outputTemplate = DefaultOutputTemplate;

    public bool? Enabled
    {
        get => _enabled;
        set
        {
            if(value.HasValue) 
                _enabled = value.Value;
        }
    }

    public string? MinimumLevel
    {
        get => _minimumLevel;
        set
        {
            if (Enum.TryParse(value, out LoggerLevel level))
                _minimumLevel = level.ToString();
        }
    }

    public string? Interval
    {
        get => _interval;
        set
        {
            if (Enum.TryParse(value, out LoggerFileInterval interval))
                _interval = interval.ToString();
        }
    }

    public string? OutputTemplate
    {
        get => _outputTemplate;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _outputTemplate = value;
        }
    }
    
    public LoggerFileSettings File { get; set; } = new();
    
    public LoggerEmailSettings Email { get; set; } = new();
}