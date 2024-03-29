﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.Chocolatey.ApiKey;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey
{
    internal sealed class ChocolateyApiKeySetterFixture : ChocolateyFixture<ChocolateyApiKeySettings>
    {
        public string Source { get; set; }

        public ChocolateyApiKeySetterFixture()
        {
            Source = "source1";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyApiKeySetter(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Set(Source, Settings);
        }
    }
}