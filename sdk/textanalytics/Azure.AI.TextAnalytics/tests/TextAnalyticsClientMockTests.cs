﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.Pipeline;
using Azure.Core.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Azure.AI.TextAnalytics.Tests
{
    public class TextAnalyticsClientMockTests : ClientTestBase
    {
        private static readonly string s_endpoint = "https://contoso-textanalytics.cognitiveservices.azure.com/";
        private static readonly string s_apiKey = "FakeapiKey";

        public TextAnalyticsClientMockTests(bool isAsync) : base(isAsync)
        {
        }

        private TextAnalyticsClient CreateTestClient(HttpPipelineTransport transport)
        {
            var options = new TextAnalyticsClientOptions
            {
                Transport = transport
            };

            var client = InstrumentClient(new TextAnalyticsClient(new Uri(s_endpoint), new TextAnalyticsApiKeyCredential(s_apiKey), options));

            return client;
        }

        [Test]
        public async Task RecognizeEntitiesResultsSorted_NoErrors()
        {
            var mockResults = new List<RecognizeEntitiesResult>()
            {
                new RecognizeEntitiesResult("1", new TextDocumentStatistics(), new List<CategorizedEntity>()
                {
                    new CategorizedEntity("EntityText0", "EntityCategory0", "EntitySubCategory0", 0, 1, 0.5),
                    new CategorizedEntity("EntityText1", "EntityCategory1", "EntitySubCategory1", 0, 1, 0.5),
                }),
                new RecognizeEntitiesResult("2", new TextDocumentStatistics(), new List<CategorizedEntity>()
                {
                    new CategorizedEntity("EntityText0", "EntityCategory0", "EntitySubCategory0", 0, 1, 0.5),
                    new CategorizedEntity("EntityText1", "EntityCategory1", "EntitySubCategory1", 0, 1, 0.5),
                }),
            };
            var mockResultCollection = new RecognizeEntitiesResultCollection(mockResults,
                new TextDocumentBatchStatistics(2, 2, 0, 2),
                "modelVersion");

            var mockResponse = new MockResponse(200);
            mockResponse.SetContent(SerializationHelpers.Serialize(mockResultCollection, SerializeRecognizeEntitiesResultCollection));

            var mockTransport = new MockTransport(mockResponse);
            TextAnalyticsClient client = CreateTestClient(mockTransport);

            var inputs = new List<TextDocumentInput>()
            {
                new TextDocumentInput("1", "TextDocument1"),
                new TextDocumentInput("2", "TextDocument2"),
            };

            var response = await client.RecognizeEntitiesBatchAsync(inputs, new TextAnalyticsRequestOptions());
            var resultCollection = response.Value;

            Assert.AreEqual("1", resultCollection[0].Id);
            Assert.AreEqual("2", resultCollection[1].Id);
        }

        [Test]
        public async Task RecognizeEntitiesResultsSorted_WithErrors()
        {
            var mockResults = new List<RecognizeEntitiesResult>()
            {
                new RecognizeEntitiesResult("2", new TextDocumentStatistics(), new List<CategorizedEntity>()
                {
                    new CategorizedEntity("EntityText0", "EntityCategory0", "EntitySubCategory0", 0, 1, 0.5),
                    new CategorizedEntity("EntityText1", "EntityCategory1", "EntitySubCategory1", 0, 1, 0.5),
                }),
                new RecognizeEntitiesResult("3", new TextDocumentStatistics(), new List<CategorizedEntity>()
                {
                    new CategorizedEntity("EntityText0", "EntityCategory0", "EntitySubCategory0", 0, 1, 0.5),
                    new CategorizedEntity("EntityText1", "EntityCategory1", "EntitySubCategory1", 0, 1, 0.5),
                }),
                new RecognizeEntitiesResult("4", "Document is invalid."),
                new RecognizeEntitiesResult("5", "Document is invalid."),
            };
            var mockResultCollection = new RecognizeEntitiesResultCollection(mockResults,
                new TextDocumentBatchStatistics(2, 2, 2, 2),
                "modelVersion");

            var mockResponse = new MockResponse(200);
            mockResponse.SetContent(SerializationHelpers.Serialize(mockResultCollection, SerializeRecognizeEntitiesResultCollection));

            var mockTransport = new MockTransport(mockResponse);
            TextAnalyticsClient client = CreateTestClient(mockTransport);

            var inputs = new List<TextDocumentInput>()
            {
                new TextDocumentInput("4", "TextDocument1"),
                new TextDocumentInput("5", "TextDocument2"),
                new TextDocumentInput("2", "TextDocument3"),
                new TextDocumentInput("3", "TextDocument4"),
            };

            var response = await client.RecognizeEntitiesBatchAsync(inputs, new TextAnalyticsRequestOptions());
            var resultCollection = response.Value;

            Assert.AreEqual("4", resultCollection[0].Id);
            Assert.AreEqual("5", resultCollection[1].Id);
            Assert.AreEqual("2", resultCollection[2].Id);
            Assert.AreEqual("3", resultCollection[3].Id);
        }

        private void SerializeRecognizeEntitiesResultCollection(ref Utf8JsonWriter json, RecognizeEntitiesResultCollection resultCollection)
        {
            json.WriteStartObject();
            json.WriteStartArray("documents");
            if (resultCollection.FirstOrDefault(r => r.CategorizedEntities.Count > 0) != default)
            {
                foreach (var result in resultCollection)
                {
                    if (result.CategorizedEntities.Count > 0)
                    {
                        json.WriteStartObject();
                        json.WriteString("id", result.Id);
                        json.WriteStartArray("entities");
                        foreach (var entity in result.CategorizedEntities)
                        {
                            json.WriteStartObject();
                            json.WriteString("text", entity.Text);
                            json.WriteString("type", JsonSerializer.Serialize(entity.Category));
                            json.WriteString("subtype", JsonSerializer.Serialize(entity.SubCategory));
                            json.WriteNumber("offset", entity.Offset);
                            json.WriteNumber("length", entity.Length);
                            json.WriteNumber("score", entity.Score);
                            json.WriteEndObject();
                        }
                        json.WriteEndArray();
                        json.WriteEndObject();
                    }
                }
            }
            json.WriteEndArray();

            json.WriteStartArray("errors");
            if (resultCollection.FirstOrDefault(r => r.ErrorMessage != default) != default)
            {
                foreach (var result in resultCollection)
                {
                    if (result.ErrorMessage != null)
                    {
                        json.WriteStartObject();
                        json.WriteString("id", result.Id);
                        json.WriteStartObject("error");
                        json.WriteStartObject("innerError");
                        json.WriteString("message", result.ErrorMessage);
                        json.WriteEndObject();
                        json.WriteEndObject();
                        json.WriteEndObject();
                    }
                }
            }
            json.WriteEndArray();

            json.WriteString("modelVersion", resultCollection.ModelVersion);
            json.WriteEndObject();

            // TODO: add statistics if needed
        }
    }
}
