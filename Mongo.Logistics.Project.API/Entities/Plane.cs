﻿using Mongo.Logistics.Project.API.DAL;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Mongo.Logistics.Project.API.Entities
{
    [BsonCollection("planes")]
    public class Plane
    {
        [BsonElement("_id")]
        [BsonId]
        public string Id { get; set; }

        [BsonElement("route")]
        public string[] Route { get; set; }

        [BsonElement("heading")]
        public int Heading { get; set; }

        [BsonElement("currentLocation")]
        public double[] CurrentLocation { get; set; }

        [BsonElement("landed")]
        public string Landed { get; set; }
    }
}
