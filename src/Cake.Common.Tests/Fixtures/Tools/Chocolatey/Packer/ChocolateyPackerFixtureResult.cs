﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer
{
    internal sealed class ChocolateyPackerFixtureResult : ToolFixtureResult
    {
        public string NuspecContent { get; }

        public ChocolateyPackerFixtureResult(FakeFileSystem fileSystem, FilePath path, ProcessSettings process)
            : base(path, process)
        {
            NuspecContent = GetNuSpecContent(fileSystem, process);
        }

        private static string GetNuSpecContent(FakeFileSystem fileSystem, ProcessSettings process)
        {
            var args = process.Arguments.Render();
            var parts = args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var nuspecFilePath = parts[1];
            var file = fileSystem.GetFile(nuspecFilePath.UnQuote());
            return file.Exists ? file.GetTextContent() : null;
        }
    }
}