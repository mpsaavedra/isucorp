// -----------------------------------------------------------------------
//  <copyright file="Is.cs" company="Nova Forge Solutions, LLC">
//  Copyright (c) IsuCorp. All rights reserved.
//  Licensed under the MIT license. See LICENSE.txt file in the project folder
//  for full license information.
//  Created by: mpsaavedra
//  </copyright>
//  -----------------------------------------------------------------------

using System.Collections;
using System.Text.RegularExpressions;
using IsuCorp.Helpers;

namespace IsuCorp.Guards;

/// <summary>
/// different Validations
/// </summary>
public static class Is
{
    /// <summary>
    /// checks if any of the values is null or empty
    /// </summary>
    /// <param name="values">values to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult NullOrEmpty(params object?[] values)
    {
        if (values.Length == 0) return new (true, 1);
        var fails = values.Where(value =>
        {
            if(value == null) return true;
            if(value is string)
                return string.IsNullOrEmpty((string)value);
            switch (value)
            {
                case string s when string.IsNullOrEmpty(s) || s.Length == 0:
                case Array { Length: 0 }:
                case ICollection { Count: 0 }:
                case IEnumerable e when !e.Cast<object>().Any():    
                    return true;
                default:
                    return false;
            }
        });
        
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are not null, empty or whitespace
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult NullEmptyOrWhiteSpace(params object?[] values)
    {
        var fails = values.Where(value =>
        {
            var isNull = NullOrEmpty(value).HasError;
            if(isNull) return true;
            if(value is string val && string.IsNullOrWhiteSpace(val)) return true;
            return false;
        });
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks if any of the values is not null or empty
    /// </summary>
    /// <param name="values">values to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult NotNullOrEmpty(params object?[] values)
    {
        if (values.Length == 0) return new (true, 0);
        var fails = values.Where(value => !NullOrEmpty(value).HasError);
        
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks if any f the values is greater than the max provided
    /// </summary>
    /// <param name="max">max value</param>
    /// <param name="values">values to be checked</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult Greater(long max, params  object?[] values)
    {
        var fails = values.Where(value =>
        {
            if (NullOrEmpty(value).HasError) return true;

            switch (value)
            {
                case Enum en when (int)value > max:
                case string st when st.Length > max:
                case byte bt when  bt > max:
                case sbyte sbt when sbt > max:
                case int inte when inte > max:
                case long lng when lng > max:
                case ushort usht when usht > max:
                case uint uit when uit > max:
                case char chr when  chr > max:
                case float flt when flt > max:
                case double dlb when dlb > max:
                case decimal de when de > max:
                case Array {  Length: 0 }:
                case ICollection { Count: 0 }:
                case IEnumerable enu when  !enu.Cast<object>().Any():    
                    return true;
                default:
                    return false;
            }
        });
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }
    
    /// <summary>
    /// checks that values are lower that provided value
    /// </summary>
    /// <param name="min"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Lower(long min, params  object?[] values)
    {
        var fails = values.Where(value =>
        {
            if (NullOrEmpty(value).HasError) return true;

            switch (value)
            {
                case Enum en when (int)value < min:
                case string st when st.Length < min:
                case byte bt when  bt < min:
                case sbyte sbt when sbt < min:
                case int inte when inte < min:
                case long lng when lng < min:
                case ushort usht when usht < min:
                case uint uit when uit < min:
                case char chr when  chr < min:
                case float flt when flt < min:
                case double dlb when dlb < min:
                case decimal de when de < min:
                case Array {  Length: 0 }:
                case ICollection { Count: 0 }:
                case IEnumerable enu when !enu.Cast<object>().Any():    
                    return true;
                default:
                    return false;
            }
        });
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks if values are in range
    /// </summary>
    /// <param name="min">minimal value</param>
    /// <param name="max">maximal value</param>
    /// <param name="values">values to check</param>
    /// <returns><see cref="IsResult"/> instance</returns>
    public static IsResult InRange(long min, long max, params object?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError || Greater(min, max).HasError || Lower(min, max).HasError);
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks if values are all equals to value
    /// </summary>
    /// <param name="value">value to use as base</param>
    /// <param name="values">values to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult Equal(object? value, params object?[] values)
    {
        var fails = values.Where(val =>
            NullOrEmpty(val).HasError || Equals(value, val));
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// check if values match with provided regular expression 
    /// </summary>
    /// <param name="regexp">Regular expression to apply</param>
    /// <param name="values">values to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult RegexMatch(string regexp, params object?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError || 
            Regex.IsMatch((string)value, regexp, RegexOptions.CultureInvariant));
        var result = fails as object[] ??  fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that all functions returns true
    /// </summary>
    /// <param name="funcs">functions to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult True(params Func<bool>[] funcs)
    {
        var fails = funcs
            .Select(f => f.Invoke())
            .Where(f => !f);
        var result = fails as bool[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that all functions returns false
    /// </summary>
    /// <param name="funcs">functions to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult False(params Func<bool>[] funcs)
    {
        var fails = funcs
            .Select(f => f.Invoke())
            .Where(f => f);
        var result = fails as bool[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks if provided values has extensions, if it has returns the name and the extension in the
    /// <see cref="IsResult"/> instance returned
    /// </summary>
    /// <param name="values">values to check</param>
    /// <returns>an <see cref="IsResult"/> instance</returns>
    public static IsResult FileExtension(params string?[] values)
    {
        var extensions = new Dictionary<string, string>();
        var fails = values.Where(value =>
        {
            if(NullOrEmpty(value).HasError) return true;
            var extension = System.IO.Path.GetExtension((string)value);
            if(!NullEmptyOrWhiteSpace(extension).HasError)
                extensions.Add(value, extension);
            return false;
        });
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length, extensions);
    }

    /// <summary>
    /// checks that values are valid date representations
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Date(params string?[] values)
    {
        var fails = values.Where(value => 
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForDate, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid decimals
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Decimal(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForDecimal, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid email addresses
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Email(params string?[] values)
    {
        var fails = values.Where(value =>
        {
            if(NullOrEmpty(value).HasError ||
               RegexMatch(CommonExpressions.ForEmail, value).HasError) return true;
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(value!);
                return emailAddress.Address != value;
            }
            catch
            {
                return true;
            }
        });
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid hexadecimal values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Hex(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForHex, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }    

    /// <summary>
    /// checks that values are valid hexadecimal values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Int(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForInteger, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }        

    /// <summary>
    /// checks that values are valid html tags
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult HtmlTag(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForTag, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid time values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Time(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForTime, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid url values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Url(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForUrl, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid name values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Name(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForName, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid domain name values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult DomainName(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForDomain, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are valid sub domain name values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult SubDomainName(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForSubdomain, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values are hostname values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Hostname(params string?[] values)
    {
        var fails = values.Where(value =>
            NullOrEmpty(value).HasError ||
            RegexMatch(CommonExpressions.ForHostname, value).HasError);
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that all values and they're content are digits
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Digit(params string?[] values)
    {
        var fails = values.Where(value =>
            NullEmptyOrWhiteSpace(value).HasError ||
            !value.All(char.IsDigit));
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that values and contained values are not Special chars
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult SpecialChars(params string?[] values)
    {
        var fails = values.Where(value =>
            NullEmptyOrWhiteSpace(value).HasError || 
            !value.All(char.IsLetterOrDigit));
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that all values are uppercase
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult UpperCase(params string?[] values)
    {
        var fails = values.Where(value =>
            NullEmptyOrWhiteSpace(value).HasError ||
            !value.All(char.IsUpper));
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }

    /// <summary>
    /// checks that all values are lowercase
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IsResult Lowercase(params string?[] values)
    {
        var fails = values.Where(value =>
            NullEmptyOrWhiteSpace(value).HasError ||
            !value.All(char.IsLower));
        var result = fails as string[] ?? fails.ToArray();
        return new (result.Length != 0, result.Length);
    }
    
    /// <summary>
    /// Result value for validations
    /// </summary>
    /// <param name="HasError">true if any error</param>
    /// <param name="Count">amount of errors in validations</param>
    /// <param name="Extensions">optional, extensions founded</param>
    public record IsResult(bool HasError, int Count, Dictionary<string, string>? Extensions = null);
}