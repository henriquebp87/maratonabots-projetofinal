using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaratonaBots_BotApp
{
    public class PrevisaoTempoApiResponse
    {
        public PrevisaoTempoApiResponseMain Main { get; set; }
    }

    public class PrevisaoTempoApiForecastResponse
    {
        public PrevisaoTempoApiResponse2[] List { get; set; }
    }

    public class PrevisaoTempoApiResponse2
    {
        public PrevisaoTempoApiResponseMain Main { get; set; }

        public string Dt_txt { get; set; }
    }

    public class PrevisaoTempoApiResponseMain
    {
        public decimal Temp { get; set; }

        public decimal Temp_min { get; set; }

        public decimal Temp_max { get; set; }
    }
}