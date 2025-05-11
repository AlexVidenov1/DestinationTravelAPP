namespace DestinationTravelAPP.Pages;

using System.Text.Json;
using DestinationTravelAPP.Models;
using DestinationTravelAPP.Services;

public partial class NewDestinationPage : ContentPage
{
    public Destination Destination { get; set; }
    public List<string> Countries { get; set; }
    private readonly DatabaseService _db;

    public NewDestinationPage(DatabaseService db)
    {
        InitializeComponent();

        _db = db;

        Destination = new Destination
        {
            Status = "draft",
            StartDate = DateTime.Today
        };

        BindingContext = this;
        _ = LoadCountriesAsync();
    }

    private async Task LoadCountriesAsync()
    {
        CountryPicker.ItemsSource = new List<string> { "Loading..." };
        try
        {
            using HttpClient client = new();

            string json = await client.GetStringAsync("https://restcountries.com/v3.1/all");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            var c = JsonSerializer.Deserialize<List<Country>>(json, options);
            Countries = c?
     .Select(x => x.Name.Common)
     .OrderBy(name => name)
     .ToList()
     ?? new List<string> { "No data" };

        }
        catch (Exception ex)
        {
            Countries = new List<string> { "Error loading countries" + ex.Message };

        }


        CountryPicker.ItemsSource = Countries;

    }


    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Destination.Country) || string.IsNullOrWhiteSpace(Destination.City))
        {
            await DisplayAlert("Грешка", "ДОбавете държава и град.", "OK");
            return;
        }

        await _db.SaveDestinationAsync(Destination);
        await DisplayAlert("Запазено", $"Дестинация до {Destination.Country} запазена!", "OK");
        await Navigation.PopAsync();
    }
}