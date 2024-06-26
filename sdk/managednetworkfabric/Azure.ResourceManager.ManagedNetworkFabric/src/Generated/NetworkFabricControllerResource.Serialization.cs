// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;

namespace Azure.ResourceManager.ManagedNetworkFabric
{
    public partial class NetworkFabricControllerResource : IJsonModel<NetworkFabricControllerData>
    {
        void IJsonModel<NetworkFabricControllerData>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options) => ((IJsonModel<NetworkFabricControllerData>)Data).Write(writer, options);

        NetworkFabricControllerData IJsonModel<NetworkFabricControllerData>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options) => ((IJsonModel<NetworkFabricControllerData>)Data).Create(ref reader, options);

        BinaryData IPersistableModel<NetworkFabricControllerData>.Write(ModelReaderWriterOptions options) => ModelReaderWriter.Write(Data, options);

        NetworkFabricControllerData IPersistableModel<NetworkFabricControllerData>.Create(BinaryData data, ModelReaderWriterOptions options) => ModelReaderWriter.Read<NetworkFabricControllerData>(data, options);

        string IPersistableModel<NetworkFabricControllerData>.GetFormatFromOptions(ModelReaderWriterOptions options) => ((IPersistableModel<NetworkFabricControllerData>)Data).GetFormatFromOptions(options);
    }
}
