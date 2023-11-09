using Newtonsoft.Json;

namespace noteblog.Models
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string ip { get; set; }

        [JsonProperty("hostname")]
        public string hostname { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("region")]
        public string region { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("loc")]
        public string loc { get; set; }

        [JsonProperty("org")]
        public string org { get; set; }

        [JsonProperty("postal")]
        public string postal { get; set; }
    }
}