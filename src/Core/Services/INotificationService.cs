// -----------------------------------------------------------------------
//  <copyright file="INotificationService.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

namespace IsuCorp.Services;

/// <summary>
/// Notification service
/// </summary>
public interface INotificationService
{
    Task<bool> SendNotification(string title, string body, params string[] client);
}