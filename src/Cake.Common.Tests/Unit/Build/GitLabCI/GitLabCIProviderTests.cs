﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI
{
    public sealed class GitLabCIProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new GitLabCIProvider(null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new GitLabCIProvider(environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }
        }

        public sealed class TheIsRunningOnGitLabCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GitLabCI()
            {
                // Given
                var fixture = new GitLabCIFixture();
                fixture.IsRunningOnGitLabCI();
                var gitLabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitLabCI.IsRunningOnGitLabCI;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_GitLabCI()
            {
                // Given
                var fixture = new GitLabCIFixture();
                var gitLabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitLabCI.IsRunningOnGitLabCI;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new GitLabCIFixture();
                var gitLabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitLabCI.Environment;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheCommandsProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new GitLabCIFixture();
                var gitLabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitLabCI.Commands;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
