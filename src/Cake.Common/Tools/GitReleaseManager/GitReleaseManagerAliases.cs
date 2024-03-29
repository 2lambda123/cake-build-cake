﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.GitReleaseManager.AddAssets;
using Cake.Common.Tools.GitReleaseManager.Close;
using Cake.Common.Tools.GitReleaseManager.Create;
using Cake.Common.Tools.GitReleaseManager.Discard;
using Cake.Common.Tools.GitReleaseManager.Export;
using Cake.Common.Tools.GitReleaseManager.Label;
using Cake.Common.Tools.GitReleaseManager.Open;
using Cake.Common.Tools.GitReleaseManager.Publish;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseManager
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/gittools/gitreleasemanager">GitReleaseManager</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=gitreleasemanager"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("GitReleaseManager")]
    public static class GitReleaseManagerAliases
    {
        /// <summary>
        /// Creates a Package Release.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerCreate("token", "owner", "repo");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Create")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Create")]
        public static void GitReleaseManagerCreate(this ICakeContext context, string token, string owner, string repository)
        {
            GitReleaseManagerCreate(context, token, owner, repository, new GitReleaseManagerCreateSettings());
        }

        /// <summary>
        /// Creates a Package Release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerCreate("token", "owner", "repo", new GitReleaseManagerCreateSettings {
        ///     Milestone         = "0.1.0",
        ///     Prerelease        = false,
        ///     Assets            = "c:/temp/asset1.txt,c:/temp/asset2.txt",
        ///     TargetCommitish   = "master",
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// GitReleaseManagerCreate("token", "owner", "repo", new GitReleaseManagerCreateSettings {
        ///     Name              = "0.1.0",
        ///     InputFilePath     = "c:/repo/releasenotes.md",
        ///     Prerelease        = false,
        ///     Assets            = "c:/temp/asset1.txt,c:/temp/asset2.txt",
        ///     TargetCommitish   = "master",
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Create")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Create")]
        public static void GitReleaseManagerCreate(this ICakeContext context, string token, string owner, string repository, GitReleaseManagerCreateSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var creator = new GitReleaseManagerCreator(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            creator.Create(token, owner, repository, settings);
        }

        /// <summary>
        /// Add Assets to an existing release.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tagName">The tag name.</param>
        /// <param name="assets">The assets.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerAddAssets("token", "owner", "repo", "0.1.0", "c:/temp/asset1.txt,c:/temp/asset2.txt");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddAssets")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.AddAssets")]
        public static void GitReleaseManagerAddAssets(this ICakeContext context, string token, string owner, string repository, string tagName, string assets)
        {
            GitReleaseManagerAddAssets(context, token, owner, repository, tagName, assets, new GitReleaseManagerAddAssetsSettings());
        }

        /// <summary>
        /// Add Assets to an existing release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tagName">The tag name.</param>
        /// <param name="assets">The assets.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerAddAssets("token", "owner", "repo", "0.1.0", "c:/temp/asset1.txt,c:/temp/asset2.txt", new GitReleaseManagerAddAssetsSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("AddAssets")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.AddAssets")]
        public static void GitReleaseManagerAddAssets(this ICakeContext context, string token, string owner, string repository, string tagName, string assets, GitReleaseManagerAddAssetsSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var assetsAdder = new GitReleaseManagerAssetsAdder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            assetsAdder.AddAssets(token, owner, repository, tagName, assets, settings);
        }

        /// <summary>
        /// Closes the milestone associated with a release.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerClose("token", "owner", "repo", "0.1.0");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Close")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Close")]
        public static void GitReleaseManagerClose(this ICakeContext context, string token, string owner, string repository, string milestone)
        {
            GitReleaseManagerClose(context, token, owner, repository, milestone, new GitReleaseManagerCloseMilestoneSettings());
        }

        /// <summary>
        /// Closes the milestone associated with a release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerClose("token", "owner", "repo", "0.1.0", new GitReleaseManagerCloseMilestoneSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Close")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Close")]
        public static void GitReleaseManagerClose(this ICakeContext context, string token, string owner, string repository, string milestone, GitReleaseManagerCloseMilestoneSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var milestoneCloser = new GitReleaseManagerMilestoneCloser(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            milestoneCloser.Close(token, owner, repository, milestone, settings);
        }

        /// <summary>
        /// Publishes the release.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tagName">The tag name.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerPublish("token", "owner", "repo", "0.1.0");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Publish")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Publish")]
        public static void GitReleaseManagerPublish(this ICakeContext context, string token, string owner, string repository, string tagName)
        {
            GitReleaseManagerPublish(context, token, owner, repository, tagName, new GitReleaseManagerPublishSettings());
        }

        /// <summary>
        /// Publishes the release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tagName">The tag name.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerPublish("token", "owner", "repo", "0.1.0", new GitReleaseManagerPublishSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Publish")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Publish")]
        public static void GitReleaseManagerPublish(this ICakeContext context, string token, string owner, string repository, string tagName, GitReleaseManagerPublishSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var publisher = new GitReleaseManagerPublisher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            publisher.Publish(token, owner, repository, tagName, settings);
        }

        /// <summary>
        /// Exports the release notes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="fileOutputPath">The output file path.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerExport("token", "owner", "repo", "c:/temp/releasenotes.md")
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Export")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Export")]
        public static void GitReleaseManagerExport(this ICakeContext context, string token, string owner, string repository, FilePath fileOutputPath)
        {
            GitReleaseManagerExport(context, token, owner, repository, fileOutputPath, new GitReleaseManagerExportSettings());
        }

        /// <summary>
        /// Exports the release notes using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="fileOutputPath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerExport("token", "owner", "repo", "c:/temp/releasenotes.md", new GitReleaseManagerExportSettings {
        ///     TagName           = "0.1.0",
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Export")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Export")]
        public static void GitReleaseManagerExport(this ICakeContext context, string token, string owner, string repository, FilePath fileOutputPath, GitReleaseManagerExportSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var publisher = new GitReleaseManagerExporter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            publisher.Export(token, owner, repository, fileOutputPath, settings);
        }

        /// <summary>
        /// Deletes and creates labels.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerLabel("token", "owner", "repo");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Label")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Label")]
        public static void GitReleaseManagerLabel(this ICakeContext context, string token, string owner, string repository)
        {
            GitReleaseManagerLabel(context, token, owner, repository, new GitReleaseManagerLabelSettings());
        }

        /// <summary>
        /// Deletes and creates labels using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerLabel("token", "owner", "repo", new GitReleaseManagerLabelSettings {
        ///     LogFilePath = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Label")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Label")]
        public static void GitReleaseManagerLabel(this ICakeContext context, string token, string owner, string repository, GitReleaseManagerLabelSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var labeller = new GitReleaseManagerLabeller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            labeller.Label(token, owner, repository, settings);
        }

        /// <summary>
        /// Opens the milestone associated with a release.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerOpen("token", "owner", "repo", "0.1.0");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Open")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Open")]
        public static void GitReleaseManagerOpen(this ICakeContext context, string token, string owner, string repository, string milestone)
        {
            GitReleaseManagerOpen(context, token, owner, repository, milestone, new GitReleaseManagerOpenMilestoneSettings());
        }

        /// <summary>
        /// Opens the milestone associated with a release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerOpen("token", "owner", "repo", "0.1.0", new GitReleaseManagerOpenMilestoneSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Open")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Open")]
        public static void GitReleaseManagerOpen(this ICakeContext context, string token, string owner, string repository, string milestone, GitReleaseManagerOpenMilestoneSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var milestoneOpener = new GitReleaseManagerMilestoneOpener(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            milestoneOpener.Open(token, owner, repository, milestone, settings);
        }

        /// <summary>
        /// Discards a Release.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerDiscard("token", "owner", "repo", "0.1.0");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Discard")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Discard")]
        public static void GitReleaseManagerDiscard(this ICakeContext context, string token, string owner, string repository, string milestone)
        {
            GitReleaseManagerDiscard(context, token, owner, repository, milestone, new GitReleaseManagerDiscardSettings());
        }

        /// <summary>
        /// Discards a Release using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="token">The token.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="milestone">The milestone.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// GitReleaseManagerDiscard("token", "owner", "repo", "0.1.0", new GitReleaseManagerDiscardSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// GitReleaseManagerDiscard("token", "owner", "repo", "0.1.0", new GitReleaseManagerDiscardSettings {
        ///     TargetDirectory   = "c:/repo",
        ///     LogFilePath       = "c:/temp/grm.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Discard")]
        [CakeNamespaceImport("Cake.Common.Tools.GitReleaseManager.Discard")]
        public static void GitReleaseManagerDiscard(this ICakeContext context, string token, string owner, string repository, string milestone, GitReleaseManagerDiscardSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var discarder = new GitReleaseManagerDiscarder(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            discarder.Discard(token, owner, repository, milestone, settings);
        }
    }
}
