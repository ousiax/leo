﻿using Leo.Data.Domain.Entities;
using System.Text.Json.Serialization;

namespace Leo.Data.Domain.Dtos
{
    public class CustomerDto
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string? CardNo { get; set; }
    }
}
