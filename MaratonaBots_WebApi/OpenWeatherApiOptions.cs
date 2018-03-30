using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaratonaBots_WebApi
{
    public interface IOpenWeatherApiOptions
    {
        string BaseUrl { get; set; }
        string ApiKey { get; set; }
        string CurrentWeatherPath { get; set; }
        string ForecastPath { get; set; }
    }

    public class OpenWeatherApiOptions : IOpenWeatherApiOptions
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string CurrentWeatherPath { get; set; }
        public string ForecastPath { get; set; }
    }
}
