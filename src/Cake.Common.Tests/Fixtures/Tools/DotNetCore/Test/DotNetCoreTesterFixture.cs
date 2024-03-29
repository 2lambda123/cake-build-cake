﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Test;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Test
{
    internal sealed class DotNetTesterFixture : DotNetFixture<DotNetTestSettings>
    {
        public string Project { get; set; }

        public ProcessArgumentBuilder Arguments { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetTester(FileSystem, Environment, ProcessRunner, Tools);
            tool.Test(Project, Arguments, Settings);
        }
    }
}