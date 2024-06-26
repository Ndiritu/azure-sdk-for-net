// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;

namespace Azure.ResourceManager.StreamAnalytics
{
    public partial class StreamAnalyticsClusterResource : IJsonModel<StreamAnalyticsClusterData>
    {
        void IJsonModel<StreamAnalyticsClusterData>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options) => ((IJsonModel<StreamAnalyticsClusterData>)Data).Write(writer, options);

        StreamAnalyticsClusterData IJsonModel<StreamAnalyticsClusterData>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options) => ((IJsonModel<StreamAnalyticsClusterData>)Data).Create(ref reader, options);

        BinaryData IPersistableModel<StreamAnalyticsClusterData>.Write(ModelReaderWriterOptions options) => ModelReaderWriter.Write(Data, options);

        StreamAnalyticsClusterData IPersistableModel<StreamAnalyticsClusterData>.Create(BinaryData data, ModelReaderWriterOptions options) => ModelReaderWriter.Read<StreamAnalyticsClusterData>(data, options);

        string IPersistableModel<StreamAnalyticsClusterData>.GetFormatFromOptions(ModelReaderWriterOptions options) => ((IPersistableModel<StreamAnalyticsClusterData>)Data).GetFormatFromOptions(options);
    }
}
