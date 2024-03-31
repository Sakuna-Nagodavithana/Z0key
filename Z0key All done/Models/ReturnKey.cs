using System;
using System.Text.Json.Serialization;

namespace Z0key.Models
{
    public class ReturnKey
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("lastModifiedTime")]
        public DateTime LastModifiedTime { get; set; }
    }
}
