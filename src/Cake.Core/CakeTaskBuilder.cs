﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Allows configuration to be performed for a registered <see cref="CakeTask"/>.
    /// </summary>
    public sealed class CakeTaskBuilder
    {
        /// <summary>
        /// Gets a read-only representation of the task being configured.
        /// </summary>
        public ICakeTaskInfo Task => Target;

        internal CakeTask Target { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskBuilder"/> class.
        /// </summary>
        /// <param name="task">The target task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is null.</exception>
        public CakeTaskBuilder(CakeTask task)
        {
            Target = task ?? throw new ArgumentNullException(nameof(task));
        }
    }
}