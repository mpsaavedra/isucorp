#nullable enable
// -----------------------------------------------------------------------
//  <copyright file="Except.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) NovaForge. All rights reserved.
//  Licensed under the MIT license. See the LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------


using System.Reflection;

namespace IsuCorp.Guards;

/// <summary>
/// Exception handling and management in a centralized way,
/// </summary>
public static class Except
{
    // to mke Guard thread safe
    // ReSharper disable once InconsistentNaming
    private static readonly object _lockObject = new();
    private static Dictionary<Type, ConstructorInfo> EncounteredExceptionType { get;  } = new ();

    /// <summary>
    /// gets the types of all launched exceptions and registered in the <see cref="Guard"/>
    /// </summary>
    public static IEnumerable<Type> CachedExceptionTypes
    {
        get
        {
            // lock variable operations in this thread
            lock(_lockObject)
                return EncounteredExceptionType.Keys.ToArray();
        }
    }
    
    /// <summary>
    /// Clear exceptions registry
    /// </summary>
    public static void ClearExceptions () => EncounteredExceptionType.Clear();

    /// <summary>
    /// register and exception object without launch it, this is useful when you handle exception in some way, but
    /// needs to keep a record of every launched exception for debug.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="msg"></param>
    /// <typeparam name="T"></typeparam>
    public static void RegisterException<T>(Exception ex, string? msg = null) where T : Exception
    {
        lock (_lockObject)
        {
            msg ??= ex.Message;
            var tmp = new Dictionary<Exception, string>()
            {
                { ex, msg }
            };

            // register exception and type
            Exceptions.Add(DateTimeOffset.UtcNow, tmp!);
            var t = typeof(T);
            if(!EncounteredExceptionType.ContainsKey(t))
            {
                // TODO: not always a constructor requires and string, perhaps is better to register it a parameterless constructor
                var info = t.GetConstructor([typeof(string)])!;
                EncounteredExceptionType[t] = info;
            }
        }
    }

    /// <summary>
    /// gets the list of registered Exceptions ordered by the <see cref="DateTimeOffset"/> in which were
    /// launched/registered
    /// </summary>
    public static SortedList<DateTimeOffset, Dictionary<Exception, string?>> Exceptions { get; } = new();

    /// <summary>
    /// Throws an Exception of type T, optionally it will have the message msg, before launch the exception
    /// it will be register so we could keep a clean track of exceptions.
    /// </summary>
    /// <param name="msg"></param>
    /// <typeparam name="T"></typeparam>
    public static void Throw<T>(string? msg = null) where T : Exception
    {
        ConstructorInfo info;

        lock (_lockObject)
        {
            var t = typeof(T);
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
            if(EncounteredExceptionType.ContainsKey(t))
                info = EncounteredExceptionType[t];
            else
            {
                info = t.GetConstructor([typeof(string)])!;
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (info is not null)
                {
                    EncounteredExceptionType[t] = info;
                }
            }

            T ex;
            if (string.IsNullOrEmpty(msg))
            {
                ex = (T)info?.Invoke([])!;
            }
            else
            {
                ex = (T)info?.Invoke([msg])!;
            }

            RegisterException<T>(ex, msg);
        }

        // throw the original exception
        throw (T)info?.Invoke([msg])!;
    }

    /// <summary>
    /// Throws an Exception of type T, it have a list of exception messages that
    /// could be concatenated or not, before launch the exception
    /// it will be register so we could keep a clean track of exceptions. This could be
    /// very usefull if you wans to make a full validation before launche the real exception
    /// </summary>
    /// <param name="msgs"></param>
    /// <param name="concatenate"></param>
    /// <typeparam name="T"></typeparam>
    public static void Throw<T>(IEnumerable<string?>? msgs, bool concatenate = true) where T : Exception
    {
        // make sure we are working with a string messages array initialized
        var messages = msgs as string[] ?? (msgs ?? []).ToArray();

        if (!messages.Any()) return;

        var error = ExceptionMessageBuilder.New.Build(messages, concatenate).Trim();
        Throw<T>(error);
    }

    /// <summary>
    /// throws and exception of provided type, it
    /// </summary>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void Throw<T>(Func<ExceptionMessageBuilder, string> action) where T : Exception
    {
        var msgBuilder = ExceptionMessageBuilder.New;
        var msg = action.Invoke(msgBuilder);
        Throw<T>(msg);
    }

    /// <summary>
    /// Returns an <see cref="ExceptionMessageBuilder"/> object that could be used to concatenate different
    /// error messages. This function can be used when you need to make several validations but throw the
    /// exception at the end of the chain with all message specified
    /// <code>
    ///  NovaForge.Guards.Throw()
    ///     .AddMessage("Username could not be null or empty") // add first validation message
    ///     .AddMessage("Password could not be null or empty") // add second validation message
    ///     .Throw{Exception}();  // throw the exception
    /// </code>
    /// </summary>
    /// <returns></returns>
    public static ExceptionMessageBuilder Throw()
    {
        var builder = ExceptionMessageBuilder.New;
        return builder;
    }

    /// <summary>
    /// Simple helpers to specify several different exception messages that will be concatenated in a single
    /// exception
    /// </summary>
    public class ExceptionMessageBuilder
    {
        private List<string> _messages = new();

        /// <summary>
        /// returns a new <see cref="ExceptionMessageBuilder"/>
        /// </summary>
        public static ExceptionMessageBuilder New => new ExceptionMessageBuilder();

        /// <summary>
        /// <inheritdoc cref="Build"/>
        /// </summary>
        public string Message => Build();

        /// <summary>
        /// Add a new error Message to the message list
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ExceptionMessageBuilder Add(string message)
        {
            if(!_messages.Contains(message))
                _messages.Add(message);
            return this;
        }

        /// <summary>
        /// Execute the check function and if returns any message it registers the message, this
        /// if very useful when you need to do inline validation of some values or parameters
        /// <code>
        /// var builder = ExceptionMessageBuilder.New
        ///     .Add((builder) =>
        ///      {
        ///         if(string.IsNullOrEmpty(username))
        ///             return
        ///      })
        ///     .Throw{Exception}();
        /// </code>
        /// </summary>
        /// <param name="checkAction"></param>
        /// <returns></returns>
        public ExceptionMessageBuilder Add(Func<string?> checkAction)
        {
            var msg = checkAction.Invoke();
            if (!string.IsNullOrEmpty(msg))
                Add(msg);
            return this;
        }

        /// <summary>
        /// Returns a plain string with registered messages if any messages
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="concatenate"></param>
        /// <returns></returns>
        public string Build(IEnumerable<string?>? messages = null, bool concatenate = true)
        {
            // also add provided messages if any
            foreach (var message in messages ?? [])
            {
                if(!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                    _messages.Add(message);
            }

            var errors = _messages.Where(m => !string.IsNullOrEmpty(m) && !string.IsNullOrWhiteSpace(m)).AsEnumerable();
            var error = errors.Aggregate("  ", (current, m) => current +
                                                               (m + (!concatenate ? Environment.NewLine : ", ")));
            error = error?.Substring(1, error.Length - 3); // remove the last ", "
            error = error!.TrimStart();
            error = error.TrimEnd();
            return error!;
        }

        /// <summary>
        /// throws an Exception of provided type with register messages and/or specified messages.
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="concatenate"></param>
        /// <param name="dontThrowIfNoMessages">if no messages, it does not throw the exception</param>
        /// <typeparam name="T"></typeparam>
        public void Throw<T>(IEnumerable<string?>? messages = null, bool concatenate = true, bool dontThrowIfNoMessages = true) where T : Exception
        {
            if (dontThrowIfNoMessages && (messages == null || !messages!.Any()) && !_messages.Any()) return;
            var msg = Build(messages!, concatenate);
            Guards.Except.Throw<T>(msg);
        }
    }
}