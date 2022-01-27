// -----------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Authentication.Basic;

#region Usings

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

#endregion

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal")]
internal static class LoggerExtensions
{
    #region Static Fields

    private static readonly Action<ILogger, Exception?> AuthorizationHeaderNotBasicDefinition =
        LoggerMessage.Define(LogLevel.Debug, new EventId(100, "AuthorizationHeaderNotPresent"), "'Authorization' header is not in 'Basic' scheme in the request");

    private static readonly Action<ILogger, Exception?> AuthorizationHeaderNotPresentDefinition =
        LoggerMessage.Define(LogLevel.Debug, new EventId(100, "AuthorizationHeaderNotPresent"), "'Authorization' header is not present in the request");

    #endregion

    #region Public Methods

    public static void AuthorizationHeaderNotBasic(this ILogger logger)
    {
        AuthorizationHeaderNotBasicDefinition(logger, null);
    }

    public static void AuthorizationHeaderNotPresent(this ILogger logger)
    {
        AuthorizationHeaderNotPresentDefinition(logger, null);
    }

    #endregion
}