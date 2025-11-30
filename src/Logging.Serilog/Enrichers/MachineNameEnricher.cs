// -----------------------------------------------------------------------
//  <copyright file="MachineNameEnricher.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) Seagull.Logging.Serilog. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using Serilog.Core;
using Serilog.Events;

namespace IsuCorp.Logging.Serilog.Enrichers;

public class MachineNameEnricher : ILogEventEnricher
{
    public const string MachineNamePropertyName = "MachineName";
    private LogEventProperty? lastValue;
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var machineName = Environment.MachineName;
        var last = lastValue;
        
        if(last is null || (string)((ScalarValue)last.Value).Value != machineName)
        {
            lastValue = last = new LogEventProperty(MachineNamePropertyName, new ScalarValue(machineName));
            logEvent?.AddPropertyIfAbsent(last);   
        }
    }
}