// -----------------------------------------------------------------------
//  <copyright file="IDbContextBase.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Data;

/// <summary>
/// DbContext is a system database
/// </summary>
public interface IDbContextBase
{
}

/// <summary>
///  <inheritdoc cref="IDbContextBase"/> that includes current UserId for database operations,
/// that could be used in auditory mechanism.
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
public interface IDbContextBase<TUserKey> : IDbContextBase
{
    /// <summary>
    /// Current user id
    /// </summary>
    TUserKey? UserId { get; }
}