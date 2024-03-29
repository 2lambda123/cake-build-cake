﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions
{
    /// <summary>
    /// Base class used to provide information about the GitHub Actions environment.
    /// </summary>
    public abstract class GitHubActionsInfo
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected GitHubActionsInfo(ICakeEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="string"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected string GetEnvironmentString(string variable)
        {
            return _environment.GetEnvironmentVariable(variable) ?? string.Empty;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="DirectoryPath"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected DirectoryPath GetEnvironmentDirectoryPath(string variable)
        {
            return _environment.GetEnvironmentVariable(variable) is string value
                ? DirectoryPath.FromString(value)
                : null;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="FilePath"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected FilePath GetEnvironmentFilePath(string variable)
        {
            return _environment.GetEnvironmentVariable(variable) is string value
                ? FilePath.FromString(value)
                : null;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected int GetEnvironmentInteger(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (!string.IsNullOrWhiteSpace(value))
            {
                int result;
                if (int.TryParse(value, out result))
                {
                    return result;
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected bool GetEnvironmentBoolean(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
