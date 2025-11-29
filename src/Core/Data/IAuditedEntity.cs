// -----------------------------------------------------------------------
//  <copyright file="IAuditedEntity.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using IsuCorp.Data.ShadowProperties;

namespace IsuCorp.Data;

/// <inheritdoc cref="IAuditedEntity{TKey,TUserKey}"/>
public interface IAuditedEntity : IAuditedEntity<long>
{
}

/// <summary>
/// <inheritdoc cref="IAuditedEntity{TKey,TUserKey}"/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IAuditedEntity<TKey> : IAuditedEntity<TKey, TKey>
{
}

/// <summary>
/// <inheritdoc cref="IEntity{TKey}"/>. Also include information related with the
/// Create, Update and Delete events.
/// </summary>
public interface IAuditedEntity<TKey, TUserKey> : 
    IEntity<TKey>,
    ICreateAudited<TUserKey>,
    IDeleteAudited<TUserKey>,
    IUpdateAudited<TUserKey>
{
}