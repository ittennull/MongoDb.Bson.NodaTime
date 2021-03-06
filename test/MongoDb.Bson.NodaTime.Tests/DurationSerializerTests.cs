﻿using System;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using NodaTime;
using Xunit;

namespace MongoDb.Bson.NodaTime.Tests
{
    public class DurationSerializerTests
    {
        static DurationSerializerTests()
        {
            BsonSerializer.RegisterSerializer(new DurationSerializer());
        }

        [Fact]
        public void CanConvertValue()
        {
            var obj = new Test { Duration = Duration.FromSeconds(34) };
            obj.ToTestJson().Should().Contain("'Duration' : '0:00:00:34'");

            var deserialized = BsonSerializer.Deserialize<Test>(obj.ToBson());
            deserialized.Duration.Should().Be(obj.Duration);
        }


        [Fact]
        public void ThrowsWhenValueIsInvalid()
        {
            Assert.Throws<FormatException>(() => BsonSerializer.Deserialize<Test>(new BsonDocument(new BsonElement("Duration", "bleh"))));
        }

        [Fact]
        public void CanParseNullable()
        {
            BsonSerializer.Deserialize<Test>(new BsonDocument(new BsonElement("DurationNullable", BsonNull.Value))).DurationNullable.Should().BeNull();
        }

        private class Test
        {
            public Duration Duration { get; set; }
            public Duration? DurationNullable { get; set; }
        }
    }
}