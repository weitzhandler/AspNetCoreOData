﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.OData.Formatter;

namespace Microsoft.AspNetCore.OData.Edm
{
    /// <summary>
    /// Encapsulates a self link factory and whether the link factory follows conventions or not.
    /// </summary>
    /// <typeparam name="T">The type of the self link generated. This should be <see cref="string"/> for ID links and <see cref="Uri"/> for read and edit links.</typeparam>
    public class SelfLinkBuilder<T>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="SelfLinkBuilder{T}"/>.
        /// </summary>
        /// <param name="linkFactory">The link factory.</param>
        /// <param name="followsConventions">Whether the factory follows odata conventions for link generation.</param>
        public SelfLinkBuilder(Func<ResourceContext, T> linkFactory, bool followsConventions)
        {
            Factory = linkFactory ?? throw new ArgumentNullException(nameof(linkFactory));
            FollowsConventions = followsConventions;
        }

        /// <summary>
        /// Gets the factory for generating links.
        /// </summary>
        public Func<ResourceContext, T> Factory { get; }

        /// <summary>
        /// Gets a boolean indicating whether the link factory follows OData conventions or not.
        /// </summary>
        public bool FollowsConventions { get; }
    }
}
