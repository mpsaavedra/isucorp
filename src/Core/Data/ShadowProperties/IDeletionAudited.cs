// -----------------------------------------------------------------------
//  <copyright file="IDeletionAudited.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Data.ShadowProperties;

/// <summary>
/// Add deletion information to the entity to allow auditory over the
/// entity events. You must notice that this parameters are only used
/// when a soft delete is used otherwise this is properties are completly
/// useless
/// </summary>
public interface IDeleteAudited<TUserKey>
{
    /// <summary>
    /// when the entity was deleted
    /// </summary>
    DateTime DeletedAt { get; set; }
    
    /// <summary>
    /// id of user that deletes the entity, Notice that the entity is not
    ///  is only marked as soft deleted
    /// </summary>
    TUserKey DeletedBy { get; set; }
}