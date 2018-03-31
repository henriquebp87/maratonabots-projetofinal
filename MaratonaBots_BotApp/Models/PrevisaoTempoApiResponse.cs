using Newtonsoft.Json;

namespace MaratonaBots_BotApp.Models
{
    public class PrevisaoTempoApiResponse
    {
        public PrevisaoTempoApiResponseMain main { get; set; }

        public Rain rain { get; set; }
    }

    public class PrevisaoTempoApiResponse2
    {
        public PrevisaoTempoApiForecastResponse[] list { get; set; }
    }

    public class PrevisaoTempoApiForecastResponse
    {
        public PrevisaoTempoApiResponseMain main { get; set; }

        public string dt_txt { get; set; }

        public Rain rain { get; set; }
    }

    public class PrevisaoTempoApiResponseMain
    {
        public decimal temp { get; set; }

        public decimal temp_min { get; set; }

        public decimal temp_max { get; set; }
    }

    public class Rain
    {
        [JsonProperty("3h")]
        public decimal? _3h { get; set; }
    }
}