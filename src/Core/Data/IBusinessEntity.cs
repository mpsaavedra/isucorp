// -----------------------------------------------------------------------
//  <copyright file="IBusinessEntity.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;

using IsuCorp.Events;

namespace IsuCorp.Data;
/// <summary>
/// Base interface for business entities with default key type (long).
/// Business entities are domain entities that can raise domain events and are audited.
/// </summary>
public interface IBusinessEntity : IBusinessEntity<long>
{
}

/// <summary>
/// Base interface for business entities with custom key type.
/// Business entities are domain entities that can raise domain events and are audited.
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public interface IBusinessEntity<TKey> : IBusinessEntity<TKey, TKey> 
{
}

/// <summary>
/// Base interface for business entities that support domain events and auditing.
/// Business entities are the core domain objects that encapsulate business logic and can raise domain events
/// to communicate changes within the domain. They also support auditing (creation, modification, deletion tracking).
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
/// <typeparam name="TUserKey">The type of the user's key used for auditing</typeparam>
public interface IBusinessEntity<TKey, TUserKey> : IAuditedEntity<TKey, TUserKey>
{
    /// <summary>
    /// Gets a read-only collection of domain events registered on this entity.
    /// Domain events are raised when important state changes occur and are dispatched after the entity is saved.
    /// </summary>
    [NotMapped]
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    
    /// <summary>
    /// Adds a domain event to this entity's event collection.
    /// The event will be dispatched after the entity is successfully saved to the database.
    /// </summary>
    /// <param name="domainEvent">The domain event to add</param>
    void AddDomainEvent(IDomainEvent domainEvent);
    
    /// <summary>
    /// Removes a specific domain event from this entity's event collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove</param>
    void RemoveDomainEvent(IDomainEvent domainEvent);    
    
    /// <summary>
    /// Clears all domain events from this entity's event collection.
    /// Typically called after events have been dispatched.
    /// </summary>
    void ClearEvents();
}