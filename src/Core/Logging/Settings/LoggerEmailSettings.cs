// -----------------------------------------------------------------------
//  <copyright file="LoggerEmailSettings.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Logging.Settings;

public class LoggerEmailSettings
{    
    public const LoggerLevel DefaultMinimumLevel = LoggerLevel.Error;
    public const string DefaultOutputTemplate= LoggerSettings.DefaultOutputTemplate;
    public const string DefaultEmailSubject = "Application Error Log";
    public const bool DefaultEmailEnableSsl = true;
    public const bool DefaultEmailIsBodyHtml = false;
    public const int DefaultEmailPort = 465;

    private bool _enabled = false;
    private string _minimumLevel = DefaultMinimumLevel.ToString();
    private string _outputTemplate = DefaultOutputTemplate;
    private string _emailSubject = DefaultEmailSubject;
    private bool _emailSsl = DefaultEmailEnableSsl;
    private bool _emailIsBodyHtml = DefaultEmailIsBodyHtml;
    private int _emailPort = DefaultEmailPort;

    public bool? Enabled
    {
        get => _enabled;
        set
        {
            if(value.HasValue) 
                _enabled = value.Value;
        }
    }
    
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Server { get; set; }
    public string? From { get; set; }
    public string?[] To { get; set; } = [];

    public string? EmailSubject
    {
        get => _emailSubject;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _emailSubject = value;
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

    public bool? EmailSsl
    {
        get => _emailSsl;
        set
        {
            if(value.HasValue)
                _emailSsl = value.Value;
        }
    }

    public bool? EmailIsBodyHtml
    {
        get => _emailIsBodyHtml;
        set
        {
            if(value.HasValue)
                _emailIsBodyHtml = value.Value;
        }
    }

    public int? EmailPort
    {
        get => _emailPort;
        set
        {
            if(value.HasValue)
                _emailPort = value.Value;
        }
    }
}