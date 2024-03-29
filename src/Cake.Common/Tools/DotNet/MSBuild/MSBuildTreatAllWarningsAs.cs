﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.MSBuild
{
    /// <summary>
    /// Represents how all warnings should be treated as by MSBuild.
    /// </summary>
    public enum MSBuildTreatAllWarningsAs
    {
        /// <summary>
        /// Use the default MSBuild behaviour.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Treat all warnings as low importance messages.
        /// </summary>
        Message = 1,

        /// <summary>
        /// Treat all warnings as errors.
        /// </summary>
        Error = 2
    }
}