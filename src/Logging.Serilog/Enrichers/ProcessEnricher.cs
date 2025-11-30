// -----------------------------------------------------------------------
//  <copyright file="ProcessEnricher.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp.Logging.Serilog. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace IsuCorp.Logging.Serilog.Enrichers;

public class ProcessEnricher : ILogEventEnricher
{
    private const string ProcessPropertyName = "Process";
    private LogEventProperty? lastValue;
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();
        var processValue = string.Empty;
        if(process?.Id > 0)
            processValue = process.Id.ToStringFormat();
        
        if(!string.IsNullOrWhiteSpace(processValue))
            processValue += process?.Id > 0 ? $" - {process.ProcessName}" : process.ProcessName;
        
        var last = lastValue;

        if (last is null || (string)((ScalarValue)last.Value).Value != processValue)
        {
            lastValue = last = new LogEventProperty(ProcessPropertyName, new ScalarValue(processValue));
            logEvent?.AddPropertyIfAbsent(last);
        }
    }
}