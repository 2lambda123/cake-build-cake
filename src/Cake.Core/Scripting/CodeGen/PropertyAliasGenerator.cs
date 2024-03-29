﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Cake.Core.Annotations;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Responsible for generating script property aliases.
    /// </summary>
    public static class PropertyAliasGenerator
    {
        private static readonly System.Security.Cryptography.SHA256 SHA256 = System.Security.Cryptography.SHA256.Create();

        /// <summary>
        /// Generates a script property alias from the specified method.
        /// The provided method must be an extension method for <see cref="ICakeContext"/>
        /// and it must be decorated with the <see cref="CakePropertyAliasAttribute"/>.
        /// </summary>
        /// <param name="method">The method to generate the code for.</param>
        /// <returns>The generated code.</returns>
        public static string Generate(MethodInfo method) => Generate(method, out _);

        /// <summary>
        /// Generates a script property alias from the specified method.
        /// The provided method must be an extension method for <see cref="ICakeContext"/>
        /// and it must be decorated with the <see cref="CakePropertyAliasAttribute"/>.
        /// </summary>
        /// <param name="method">The method to generate the code for.</param>
        /// <param name="hash">The hash of property signature.</param>
        /// <returns>The generated code.</returns>
        public static string Generate(MethodInfo method, out string hash)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            // Perform validation.
            ValidateMethod(method);
            ValidateMethodParameters(method);

            // Get the property alias attribute.
            var attribute = method.GetCustomAttribute<CakePropertyAliasAttribute>();

            // Generate code.
            return attribute.Cache
                ? GenerateCachedCode(method, out hash)
                    : GenerateCode(method, out hash);
        }

        private static void ValidateMethod(MethodInfo method)
        {
            Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null"); // ReSharper

            if (!method.DeclaringType.IsStatic())
            {
                const string format = "The type '{0}' is not static.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.DeclaringType.FullName));
            }

            if (!method.IsDefined(typeof(ExtensionAttribute)))
            {
                const string format = "The method '{0}' is not an extension method.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }

            if (!method.IsDefined(typeof(CakePropertyAliasAttribute)))
            {
                const string format = "The method '{0}' is not a property alias.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }
        }

        private static void ValidateMethodParameters(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var parameterCorrect = false;
            if (parameters.Length == 1)
            {
                if (parameters[0].ParameterType == typeof(ICakeContext))
                {
                    parameterCorrect = true;
                }
            }

            if (!parameterCorrect)
            {
                const string format = "The property alias '{0}' has an invalid signature.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }

            if (method.IsGenericMethod)
            {
                const string format = "The property alias '{0}' cannot be generic.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }

            if (method.ReturnType == typeof(void))
            {
                const string format = "The property alias '{0}' cannot return void.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }
        }

        private static string GenerateCode(MethodInfo method, out string hash)
        {
            var builder = new StringBuilder();

            // Property is obsolete?
            var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
            if (obsolete != null)
            {
                AddObsoleteAttribute(builder, obsolete);
            }

            hash = GenerateCommonInitalCode(method, ref builder);
            if (obsolete != null)
            {
                builder.AppendLine("#pragma warning disable CS0618");
            }

            builder.AppendFormat("    => ", method.Name);
            builder.Append(method.GetFullName());
            builder.AppendLine("(Context);");

            if (obsolete != null)
            {
                builder.AppendLine("#pragma warning restore CS0618");
            }

            return builder.ToString();
        }

        private static string GenerateCommonInitalCode(MethodInfo method, ref StringBuilder builder)
        {
            string hash;
            var curPos = builder.Length;
            builder.Append("public ");
            builder.Append(GetReturnType(method));
            builder.Append(' ');
            builder.Append(method.Name);
            builder.AppendLine();

            hash = Convert.ToHexString(
                    SHA256
                        .ComputeHash(Encoding.UTF8.GetBytes(builder.ToString(curPos, builder.Length - curPos))));

            return hash;
        }

        private static string GenerateCachedCode(MethodInfo method, out string hash)
        {
            var builder = new StringBuilder();

            // Property is obsolete?
            var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
            if (obsolete != null && obsolete.IsError)
            {
                return GenerateCode(method, out hash);
            }

            // Backing field.
            builder.Append("private ");
            builder.Append(GetReturnType(method));
            if (method.ReturnType.GetTypeInfo().IsValueType)
            {
                builder.Append('?');
            }
            builder.Append(" _");
            builder.Append(method.Name);
            builder.Append(';');
            builder.AppendLine();

            // Property
            if (obsolete != null)
            {
                AddObsoleteAttribute(builder, obsolete);
            }
            hash = GenerateCommonInitalCode(method, ref builder);

            if (obsolete != null)
            {
                builder.AppendLine("#pragma warning disable CS0618");
            }

            builder.AppendFormat("    => _{0} ??= ", method.Name);
            builder.Append(method.GetFullName());
            builder.AppendLine("(Context);");

            if (obsolete != null)
            {
                builder.AppendLine("#pragma warning restore CS0618");
            }

            return builder.ToString();
        }

        private static void AddObsoleteAttribute(StringBuilder builder, ObsoleteAttribute obsolete)
        {
            builder.Append("[Obsolete");

            if (!string.IsNullOrEmpty(obsolete.Message))
            {
                builder.AppendFormat(
                    "(\"{0}\", {1})",
                    obsolete.Message,
                    obsolete.IsError ? "true" : "false");
            }

            builder.AppendLine("]");
        }

        private static string GetReturnType(MethodInfo method)
        {
            var isDynamic = method.ReturnTypeCustomAttributes.GetCustomAttributes(typeof(DynamicAttribute), true).Any();
            var isNullable = method.ReturnTypeCustomAttributes.GetCustomAttributes(true).Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");
            return string.Concat(
                isDynamic ? "dynamic" : method.ReturnType.GetFullName(),
                isNullable ? "?" : string.Empty);
        }
    }
}