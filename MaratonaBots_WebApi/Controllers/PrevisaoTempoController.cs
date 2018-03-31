using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MaratonaBots_WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/PrevisaoTempo")]
    public class PrevisaoTempoController : Controller
    {
        private IOpenWeatherApiOptions _openWeatherApiOptions;

        public PrevisaoTempoController(IOpenWeatherApiOptions openWeatherApiOptions)
        {
            _openWeatherApiOptions = openWeatherApiOptions;
        }

        [Produces("application/json")]
        [HttpGet("GetCurrentWeather/{cityName}")]
        public async Task<IActionResult> GetCurrentWeather(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
                return null;

            using (var c = new HttpClient())
            {
                var url = $"{ _openWeatherApiOptions.BaseUrl}{_openWeatherApiOptions.CurrentWeatherPath}?q={cityName}&units=metric&APPID={_openWeatherApiOptions.ApiKey}";
                var response = await c.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Content(json);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();

                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        [Produces("application/json")]
        [HttpGet("GetForecast/{cityName}")]
        public async Task<IActionResult> GetForecast(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
                return null;

            using (var c = new HttpClient())
            {
                var url = $"{ _openWeatherApiOptions.BaseUrl}{_openWeatherApiOptions.ForecastPath}?q={cityName}&units=metric&APPID={_openWeatherApiOptions.ApiKey}";
                var response = await c.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Content(json);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();

                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}