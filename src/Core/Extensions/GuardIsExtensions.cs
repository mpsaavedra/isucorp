// -----------------------------------------------------------------------
//  <copyright file="GuardIsExtensions.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Extensions;

/// <summary>
/// Guard Is related extensions
/// </summary>
public static class GuardIsExtensions
{
    public static bool ToNullOrEmpty<TSource>(this TSource source) =>
        Guards.Is.NullOrEmpty(source).HasError;
}