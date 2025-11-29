// -----------------------------------------------------------------------
//  <copyright file="ISoftDeleted.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Data.ShadowProperties;

public interface ISoftDeleted
{
    /// <summary>
    /// If true entity has been marked as soft deleted and is not available to normal
    /// queries.
    /// </summary>
    bool Deleted {  get; set; }
}