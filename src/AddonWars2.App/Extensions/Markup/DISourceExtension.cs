// ==================================================================================================
// <copyright file="DISourceExtension.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.Markup
{
    using System;
    using System.Windows.Markup;

    /// <summary>
    /// Provides a XAML markup extension to avoid setting data context in code behind.
    /// Returns a view model instance based on the provided type.
    /// </summary>
    public class DISourceExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets a view model resolver.
        /// </summary>
        public static Func<Type?, object>? Resolver { get; set; }

        /// <summary>
        /// Gets or sets a view model type to be requested.
        /// </summary>
        public Type? Type { get; set; }

        /// <inheritdoc/>
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            return Resolver?.Invoke(Type);
        }
    }
}