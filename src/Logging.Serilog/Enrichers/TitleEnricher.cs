// -----------------------------------------------------------------------
//  <copyright file="TitleEnricher.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) Seagull.Logging.Serilog. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using Serilog.Core;
using Serilog.Events;

namespace IsuCorp.Logging.Serilog.Enrichers;

public class TitleEnricher : ILogEventEnricher
{
    public const string TitlePropertyName = "Title";
    private readonly string? title;
    private LogEventProperty? lastValue;

    public TitleEnricher(string title)
    {
        if(string.IsNullOrWhiteSpace(title)) 
            title = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        this.title = title;
    }
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var last = lastValue;
        if (last is null || (string)((ScalarValue)last.Value).Value != title)
        {
            lastValue = last = new LogEventProperty(TitlePropertyName, new ScalarValue(title));
            logEvent?.AddPropertyIfAbsent(last);
        }
    }
}