// -----------------------------------------------------------------------
//  <copyright file="LoggerFileSettings.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Logging.Settings;

public class LoggerFileSettings
{
    public const string DefaultFilePath = @"logs\\.log"; 
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Error;
    public const LoggerFileInterval DefaultLoggerInterval = LoggerFileInterval.Day;
    public const string DefaultOutputTemplate= LoggerSettings.DefaultOutputTemplate;

    private bool _enabled = false;
    private string _filePath = DefaultFilePath;
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

    public string? FilePath
    {
        get => _filePath;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                _filePath = value;
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
}