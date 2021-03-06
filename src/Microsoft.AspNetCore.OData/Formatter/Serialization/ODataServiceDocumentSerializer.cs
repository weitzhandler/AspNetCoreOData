// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.OData;

namespace Microsoft.AspNetCore.OData.Formatter.Serialization
{
    /// <summary>
    /// Represents an <see cref="ODataSerializer"/> for serializing <see cref="ODataServiceDocument" />'s for generating servicedoc's.
    /// </summary>
    public class ODataServiceDocumentSerializer : ODataSerializer
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ODataServiceDocumentSerializer"/>.
        /// </summary>
        public ODataServiceDocumentSerializer()
            : base(ODataPayloadKind.ServiceDocument)
        {
        }

        /// <inheritdoc/>
        public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (messageWriter == null)
            {
                throw new ArgumentNullException(nameof(messageWriter));
            }

            ODataServiceDocument serviceDocument = graph as ODataServiceDocument;
            if (serviceDocument == null)
            {
                throw new SerializationException(Error.Format(SRResources.CannotWriteType, GetType().Name, type?.Name));
            }

            // TODO: Call Async version?
            // messageWriter.WriteServiceDocumentAsync(serviceDocument);
            messageWriter.WriteServiceDocument(serviceDocument);
        }
    }
}
