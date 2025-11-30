// -----------------------------------------------------------------------
//  <copyright file="SerilogExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) Seagull.Logging.Serilog. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Diagnostics;
using IsuCorp.Extensions;
using IsuCorp.Logging.Settings;
using Serilog;
using Serilog.Events;

namespace IsuCorp.Logging.Serilog.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Net;
using global::Serilog;
using global::Serilog.Context;
using global::Serilog.Debugging;
using Seri = global::Serilog;


public static class SerilogExtensions
{
    public static LoggerConfiguration AddDefaultSetings(this LoggerConfiguration config, string title)
    {
        SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));
        LogContext.PushProperty("Title", title);
        return config.ToIsNullOrEmptyThrow(nameof(LoggerConfiguration))
            // .Enrich.WithMachineName()
            ;
    }

    public static LogEventLevel ToLogEventLevel(this string logLevel)
    {
        
        return logLevel switch
        {
            
            _ => LogEventLevel.Error
        };
    }
}