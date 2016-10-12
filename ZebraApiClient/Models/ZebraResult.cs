using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZebraApiClient.Models
{
    public class ZebraResult
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "move")]
        public string Move { get; set; }
    }

    public class ZebraResults
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "result")]
        public ZebraScore[] Result { get; set; }
        
    }
}

    public class ZebraScore
    {
        [JsonProperty(PropertyName = "white")]
        public int White { get; set; }
        [JsonProperty(PropertyName = "black")]
        public int Black { get; set; }
        [JsonProperty(PropertyName = "move")]
        public string Move { get; set; }
    }