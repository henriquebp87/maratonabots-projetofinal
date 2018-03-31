using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MaratonaBots_BotApp.Dialogs
{
    [Serializable]
    public class PrevisaoTempoDialog : LuisDialog<object>
    {
        public PrevisaoTempoDialog(ILuisService service) : base(service) { }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não consegui entender sua frase {result.Query}");
        }

        [LuisIntent("sobre")]
        public async Task Sobre(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Meu nome é Henribot e estou aqui para te ajudar a não pegar chuva quando sair de casa!");
        }

        [LuisIntent("saudar")]
        public async Task Saudar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Olá, tudo bem? Eu sou um bot que informa a previsão do tempo!");
        }

        [LuisIntent("previsao")]
        public async Task PrevisaoTempo(IDialogContext context, LuisResult result)
        {
            try
            {
                var cidade = result.Entities?.FirstOrDefault(e => e.Type == "cidade");
                if (cidade != null)
                {
                    string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
                    string endpoint = $"{apiUrl}GetForecast/{cidade.Entity}";

                    await context.PostAsync($"Aguarde um momento enquanto eu obtenho a previsão do tempo para {cidade.Entity}...");

                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(endpoint);
                        if (!response.IsSuccessStatusCode)
                            await context.PostAsync($"Ocorreu algum erro ao buscar sua previsão do tempo. Tente mais tarde :(");
                        else
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var resultado = JsonConvert.DeserializeObject<Models.PrevisaoTempoApiResponse2>(json);
                            Dictionary<DateTime, Tuple<int, int, decimal>> temperaturas = new Dictionary<DateTime, Tuple<int, int, decimal>>();
                            foreach (var item in resultado?.list)
                            {
                                var date = Convert.ToDateTime(item?.dt_txt, new CultureInfo("pt-BR")).Date;
                                if (!temperaturas.ContainsKey(date))
                                {
                                    int min = int.MaxValue, max = int.MinValue;
                                    decimal rain = decimal.MinValue;
                                    temperaturas.Add(date, new Tuple<int, int, decimal>(min, max, rain));
                                }
                                
                                var minTempDate = temperaturas[date].Item1;
                                var maxTempDate = temperaturas[date].Item2;
                                var maxRainDate = temperaturas[date].Item3;
                                var minJson = Convert.ToInt32(item?.main?.temp_min);
                                var maxJson = Convert.ToInt32(item?.main?.temp_max);
                                var maxRainJson = item?.rain?._3h ?? 0;

                                if (minJson < minTempDate)
                                    minTempDate = minJson;

                                if (maxJson > maxTempDate)
                                    maxTempDate = maxJson;

                                if (maxRainJson > maxRainDate)
                                    maxRainDate = maxRainJson;

                                temperaturas[date] = new Tuple<int, int, decimal>(minTempDate, maxTempDate, maxRainDate);
                            }

                            var retornoBot = temperaturas.Select(t => $@"{t.Key.ToString("dd/MM/yyyy")}({RetornaDiaSemana(t.Key.DayOfWeek)}): 
                                                                          Mín.: {t.Value.Item1}ºC / 
                                                                          Máx.: {t.Value.Item2}ºC /
                                                                          Chuva: {t.Value.Item3}mm" );
                            await context.PostAsync(string.Join("<br />", retornoBot));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Ocorreu algum erro ao buscar sua previsão do tempo. Tente mais tarde :(");
            }
        }        

        [LuisIntent("clima atual")]
        public async Task ClimaAtual(IDialogContext context, LuisResult result)
        {
            try
            {
                var cidade = result.Entities?.FirstOrDefault(e => e.Type == "cidade");
                if (cidade != null)
                {
                    string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
                    string endpoint = $"{apiUrl}GetCurrentWeather/{cidade.Entity}";

                    await context.PostAsync($"Aguarde um momento enquanto eu obtenho o clima atual para {cidade.Entity}...");

                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(endpoint);
                        if (!response.IsSuccessStatusCode)
                            await context.PostAsync($"Ocorreu algum erro ao buscar os dados do clima atual. Tente mais tarde :(");
                        else
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var resultado = JsonConvert.DeserializeObject<Models.PrevisaoTempoApiResponse>(json);
                            var retornoBot = $"Temperatura atual: {resultado?.main?.temp}ºC<br />Chuva: {resultado?.rain?._3h ?? 0}mm";

                            await context.PostAsync(retornoBot);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Ocorreu algum erro ao buscar os dados do clima atual. Tente mais tarde :(");
            }
        }

        private string RetornaDiaSemana(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Dom";

                case DayOfWeek.Monday:
                    return "Seg";

                case DayOfWeek.Tuesday:
                    return "Ter";

                case DayOfWeek.Wednesday:
                    return "Qua";

                case DayOfWeek.Thursday:
                    return "Qui";

                case DayOfWeek.Friday:
                    return "Sex";

                case DayOfWeek.Saturday:
                    return "Sáb";

                default:
                    return string.Empty;
            }
        }
    }
}