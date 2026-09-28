using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MauiAppTempoAgora.Models;
using System.Text.Json.Nodes;


namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "faa0189c471f0459ef45d77291b6dda3";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&lang=pt_br&appid={chave}";

            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage resp = await Client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var rascunho = JsonObject.Parse(json);

                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime sunrise = epoch.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = epoch.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    };
                }
                else if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Trata o caso em que a cidade não é encontrada (HTTP 404)
                    throw new Exception("Cidade não encontrada. Verifique o nome digitado.");
                }
                else
                {
                    throw new Exception($"Erro na requisição. Código HTTP: {resp.StatusCode}");
                }
            }

            return t;
        }
    }
}
