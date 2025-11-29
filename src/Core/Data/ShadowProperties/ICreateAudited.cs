// -----------------------------------------------------------------------
//  <copyright file="ICreationAudited.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Data.ShadowProperties;

/// <summary>
/// Add creation information to the entity, to allow auditory over the
/// entity events
/// </summary>
/// <typeparam name="TUserKey"></typeparam>
public interface ICreateAudited<TUserKey>
{
    /// <summary>
    /// When the entity was created
    /// </summary>
    DateTime CreatedAt { get; set;}
    
    /// <summary>
    /// id of who create the entity
    /// </summary>
    TUserKey CreatedBy { get; set; }
}