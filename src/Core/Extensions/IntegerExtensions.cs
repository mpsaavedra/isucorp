// -----------------------------------------------------------------------
//  <copyright file="StringExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Extensions;

public static class IntegerExtensions
{
    public static string ToStringFormat(this int value, string? format = null) => 
        format != null ? value.ToString(format) : value.ToString();
}