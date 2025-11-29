// -----------------------------------------------------------------------
//  <copyright file="IUpdationAudited.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Data.ShadowProperties;

/// <summary>
/// Add update information to the entity
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
public interface IUpdateAudited<TUserKey>
{
    /// <summary>
    /// when the entity was updated
    /// </summary>
    DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// id of user that update the entity
    /// </summary>
    TUserKey? UpdatedBy { get; set; }
}