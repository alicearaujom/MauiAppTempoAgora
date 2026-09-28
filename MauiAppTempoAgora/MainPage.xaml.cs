using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Descrição: {t.description}\n" +
                                                $"Velocidade do Vento: {t.speed} m/s\n" +
                                                $"Visibilidade: {t.visibility} m\n" +
                                                $"Latitude: {t.lat}\n" +
                                                $"Longitude: {t.lon}\n" +
                                                $"Nascer do Sol: {t.sunrise}\n" +
                                                $"Por do Sol: {t.sunset}\n" +
                                                $"Temp Máx: {t.temp_max} °C\n" +
                                                $"Temp Min: {t.temp_min} °C\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade!";
                }
            }
            catch (HttpRequestException)
            {
                // Alerta para falta de conexão com a internet
                await DisplayAlertAsync("Erro de Conexão", "Você está sem conexão com a internet. Verifique sua rede e tente novamente.", "OK");
            }
            catch (Exception ex)
            {
                // Alerta para cidade não encontrada ou outros erros
                await DisplayAlertAsync("Ops", ex.Message, "OK");
            }
        }
    }
}
