// -----------------------------------------------------------------------
//  <copyright file="DetailedException.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Net;

namespace IsuCorp.Exceptions;

/// <summary>
/// PhoneBit platform error that include an error code
/// </summary>
public class DetailedException(string code, string message) : Exception(message)
{
    public DetailedException(string message) : this(Messages.Core.PlatformExceptionCodeNotSpecified.Code, message)
    { }
    
    public DetailedException(Messages.Message message) : this(message.Code, message.Msg()) { }

    public string Code { get; set; } = code;
}