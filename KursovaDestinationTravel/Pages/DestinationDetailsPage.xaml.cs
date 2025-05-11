using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using DestinationTravelAPP.Models;
using DestinationTravelAPP.Services;

namespace DestinationTravelAPP.Pages;

public partial class DestinationDetailsPage : ContentPage, INotifyPropertyChanged
{
    public Destination Destination { get; set; }
    private List<string> countries = new();
    private readonly DatabaseService _db;
    public ObservableCollection<WeatherForecast> Forecast { get; set; } = new();

    public DestinationDetailsPage(Destination destination, DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        destination.Status = "draft";
        destination.StartDate = DateTime.Now;

        Destination = destination;
        BindingContext = this;
        //_ = LoadCountriesAsync();
        _ = LoadWeather();
    }

    private async Task LoadWeather()
    {
        var city = Destination.City;

        var data = await LoadWeatherAsync(city);

        Forecast.Clear();
        foreach (var item in data)
            Forecast.Add(item);

        OnPropertyChanged(nameof(Forecast));
    }

    //private async Task LoadCountriesAsync()
    //{
    //    CountryPicker.ItemsSource = new List<string> { "Loading..." };
    //    try
    //    {
    //        using HttpClient client = new();

    //        string json = await client.GetStringAsync("https://restcountries.com/v3.1/all");

    //        var options = new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        };

    //        //convert a JSON string into a .NET object. It belongs to the System.Text.Json namespace, which provides high-performance JSON serialization and deserialization.
    //        var c = JsonSerializer.Deserialize<List<Country>>(json, options); //JsonConvert.DeserializeObject<List<Country>>(json);
    //        countries = c?.ConvertAll(x => x.Name.Common) ?? new List<string> { "No data" };

    //    }
    //    catch (Exception ex)
    //    {
    //        countries = new List<string> { "Error loading countries" + ex.Message };

    //    }


    //    CountryPicker.ItemsSource = countries;

    //}


    private async void AddDestination(object sender, EventArgs e)
    {
        await _db.SaveDestinationAsync(Destination);

        await DisplayAlert("Запазено", "Дестинацията е запазена", "OK");

        await Navigation.PopAsync();
    }

    private async void ViewExpenses(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ExpensesPage(Destination, _db));
    }

    private async Task<List<WeatherForecast>> LoadWeatherAsync(string city)
    {
        var forecasts = new List<WeatherForecast>();

        try
        {

            var apiKey = "10de1aaeb395e588bf992459fe4b2a48";
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={apiKey}";

            using var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var list = root.GetProperty("list");

            foreach (var item in list.EnumerateArray().Where(i => i.GetProperty("dt_txt").GetString().Contains("12:00")))
            {
                var dt = item.GetProperty("dt_txt").GetString();
                var date = DateTime.Parse(dt);

                var temp = item.GetProperty("main").GetProperty("temp").GetDouble();
                var desc = item.GetProperty("weather")[0].GetProperty("description").GetString();

                forecasts.Add(new WeatherForecast
                {
                    Date = date,
                    Temp = temp,
                    Description = desc
                });
            }
        }
        catch (Exception ex)
        {
            forecasts.Add(new WeatherForecast
            {
                Date = DateTime.Today,
                Temp = 0,
                Description = "Error: " + ex.Message
            });
        }

        return forecasts;
    }
}