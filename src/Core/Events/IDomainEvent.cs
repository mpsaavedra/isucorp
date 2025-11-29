// -----------------------------------------------------------------------
//  <copyright file="IDomainEvents.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using MediatR;

namespace IsuCorp.Events;

/// <summary>
/// Basic domain event information, includes the event unique identifier
/// and the date and time of when it occurs
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Event unique identifier
    /// </summary>
    Guid EventId { get; }
    
    /// <summary>
    /// When the event occurs
    /// </summary>
    DateTime OccursOn { get; }
}