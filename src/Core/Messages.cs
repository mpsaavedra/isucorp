// -----------------------------------------------------------------------
//  <copyright file="Messages.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Globalization;
using System.Resources;
using IsuCorp.Exceptions;

namespace IsuCorp;

public class Messages
{
    public static class Core
    {
        public static readonly Message ErrorMessageLocalization = Message.New("SG-00001", "Localization Error: Code- '{0}', Message - {|}");
        public static readonly Message ErrorPlatformExceptionCodeNotSpecified  = Message.New("SG-00002", "Platform Exception Code- '{0}', Message - {|}");
        public static readonly Message ErrorQueryRepositoryCouldNotReturnQueryableForEntity = Message.New("SG-00003", "QueryRepository could not return a queryable instance for entity '{0}'");
    }

    /// <summary>
    /// Localized message
    /// </summary>
    public class Message
    {
        private string _code = String.Empty;
        private string _message = String.Empty;

        /// <summary>
        /// returns a new <see cref="Message"/> instance
        /// </summary>
        /// <param name="code">Message code</param>
        /// <param name="message">Message content</param>
        /// <returns></returns>
        public static Message New(string code, string? message = null) => new()
        {
            _code = code,
            _message = message
        };
        
        /// <summary>
        /// Message code
        /// </summary>
        public string Code => _code;
        
        /// <summary>
        /// Plain message with not format
        /// </summary>
        public string Details => _message;
        
        /// <summary>
        /// Error message
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Msg(params object[] args) => LocalizedMsg(null, args);

        /// <summary>
        /// returns a Message in the localized language if exists in the Resource, if not it returns the
        /// default message
        /// </summary>
        /// <returns></returns>
        public string LocalizedMsg(params object?[] parameters) => LocalizedMsg(null, parameters);

        /// <summary>
        /// returns a Message in the localized language if exists in the Resource, if not it returns the
        /// default message
        /// </summary>
        /// <param name="language"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string LocalizedMsg(string? language = null, params object?[] parameters)
        {
            try
            {
                var cultureInfo = string.IsNullOrEmpty(language)
                    ? Thread.CurrentThread.CurrentCulture
                    : new CultureInfo(language);
                var resourceName = GetType().Assembly.GetName().Name;
                var rMngr = new ResourceManager($"{resourceName}.Resources", this.GetType().Assembly);
                var resource = rMngr.GetString(Code, cultureInfo);
                var msg = (resource ?? _message)!;
                if (!msg.Contains("{0}")) return msg;
                return string.Format(msg, parameters!);

            }
            catch (Exception e)
            {
                Guards.Except.RegisterException<DetailedException>(new DetailedException(
                    Core.ErrorMessageLocalization.Code,
                    Core.ErrorMessageLocalization.Msg(_code, e.Message)
                ));
                return _message;
            }
        }
    }
}