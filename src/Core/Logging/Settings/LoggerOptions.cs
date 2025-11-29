// -----------------------------------------------------------------------
//  <copyright file="LoggerOptions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Logging.Settings;

public class LoggerOptions
{
    public LoggerSettings Settings { get; private set; } = new();

    public LoggerOptions AddFile(
        string filePath = LoggerFileSettings.DefaultFilePath,
        LoggerLevel minimumLevel = LoggerFileSettings.DefaultMinimumLevel,
        LoggerFileInterval interval = LoggerFileSettings.DefaultLoggerInterval)
    {
        Settings.File = new()
        {
            Enabled = true,
            FilePath = filePath,
            MinimumLevel = minimumLevel.ToString(),
            Interval = interval.ToString()
        };
        return this;
    }

    public LoggerOptions AddEmail(
        string username,
        string password,
        string server,
        string from,
        string[] to,
        bool enabledSsl = LoggerEmailSettings.DefaultEmailEnableSsl,
        int port = LoggerEmailSettings.DefaultEmailPort,
        string subject = LoggerEmailSettings.DefaultEmailSubject,
        bool isBodyHtml = LoggerEmailSettings.DefaultEmailIsBodyHtml,
        LoggerLevel minimumLevel = LoggerEmailSettings.DefaultMinimumLevel,
        string outputTemplate = LoggerEmailSettings.DefaultOutputTemplate)
    {
        Settings.Email = new()
        {
            Enabled = true,
            Username = username,
            Password = password,
            Server = server,
            From = from,
            To = to,
            EmailSsl = enabledSsl,
            EmailPort = port,
            EmailSubject = subject,
            EmailIsBodyHtml = isBodyHtml,
        };
        return this;
    }
}