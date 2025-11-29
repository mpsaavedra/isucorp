// -----------------------------------------------------------------------
//  <copyright file="IEntity.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Data.ShadowProperties;

namespace IsuCorp.Data;

/// <inheritdoc cref="IEntity{TKey}"/>
public interface IEntity : IEntity<long>
{
}

/// <summary>
/// Database entity basic information, it includes only Id, Rowversion and Deleted. this is the
/// basic information needed for an entity.
/// </summary>
public interface IEntity<TKey> : ISoftDeleted, IEquatable<TKey>
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    TKey Id { get; set; }
    
    /// <summary>
    /// Guid to avoid concurrency issues
    /// </summary>
    string RowVersion { get; set; }
}