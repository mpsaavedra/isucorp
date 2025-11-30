// -----------------------------------------------------------------------
//  <copyright file="SerilogProvider.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) Seagull.Logging.Serilog. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Net;
using IsuCorp.Logging;
using IsuCorp.Logging.Settings;
using IsuCorp.Logging.Serilog.Extensions;

namespace IsuCorp.Logging.Serilog;

using global::Serilog;

public class SerilogProvider(LoggerSettings settings) : 
    IsuCorp.Logging.ILogger, IsuCorp.Logging.ILoggerService
{
    private ILogger? Logger { get; set; }

    public IsuCorp.Logging.ILogger CreateLogger(string title)
    {
        var config = new LoggerConfiguration()
            .AddDefaultSetings(title)
            .WriteTo.Console(
                restrictedToMinimumLevel: settings.MinimumLevel.ToLogEventLevel(),
                settings.OutputTemplate);
        if (settings.File.Enabled.HasValue && settings.File.Enabled.Value)
        {
            config.WriteTo.File(
                path: settings.File.FilePath,
                restrictedToMinimumLevel: settings.File.MinimumLevel.ToLogEventLevel(),
                outputTemplate: settings.File.OutputTemplate);
        }

        if (settings.Email.Enabled.HasValue && settings.Email.Enabled.Value)
        {
            config.WriteTo.Email(
                settings.Email.From,
                settings.Email.To,
                settings.Email.Server,
                new NetworkCredential(settings.Email.Username, settings.Email.Password),
                settings.OutputTemplate,
                settings.MinimumLevel.ToLogEventLevel());
        }
        Logger = config.CreateLogger();
        return this;
    }

    public IsuCorp.Logging.ILogger CreateLogger<T>() =>
        CreateLogger(typeof(T).Name);
    
    public IsuCorp.Logging.ILogger Trace(string message, object? data = null)
    {
        Logger?.Verbose(message + " => {@Data}", data, true);
        Close();
        return this;
    }

    public IsuCorp.Logging.ILogger Debug(string message, object? data = null)
    {
        Logger?.Debug(message + " => {@Data}", data, true);
        Close();
        return this;
    }

    public IsuCorp.Logging.ILogger Info(string message, object? data = null)
    {
        Logger?.Information(message + " => {@Data}", data, true);
        Close();
        return this;
    }

    public IsuCorp.Logging.ILogger Error(string message, object? data = null)
    {
        Logger?.Error(message + " => {@Data}", data, true);
        Close();
        return this;
    }

    public IsuCorp.Logging.ILogger Error(Exception ex)
    {
        Logger?.Error("Exception => {@Exception}", ex, true);
        Close();
        return this;
    }
    
    private void Close() => Log.CloseAndFlush();
}