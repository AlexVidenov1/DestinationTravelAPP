using System.Collections.ObjectModel;
using DestinationTravelAPP.Models;
using DestinationTravelAPP.Services;

namespace DestinationTravelAPP.Pages;

public partial class MainPage : ContentPage
{
    public ObservableCollection<Destination> Destinations { get; set; } = new();
    private readonly DatabaseService _db;

    public MainPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        BindingContext = this;


    }

    public MainPage() : this(null)
    {
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Destinations.Clear();
        if (_db != null)
        {
            var items = await _db.GetDestinationsAsync();
            foreach (var item in items)
                Destinations.Add(item);
        }
        else
        {
            Destinations.Add(new Destination { Country = "Bulgaria", City = "Sofia", StartDate = DateTime.Now, Duration = 5, Status = "draft" });
            Destinations.Add(new Destination { Country = "England", City = "London", StartDate = DateTime.Now, Duration = 7, Status = "draft" });
        }


    }

    private async void AddNewDestination(object sender, EventArgs e)
    {
        var newDestination = new Destination();
        newDestination.Status = "draft";
        await Navigation.PushAsync(new NewDestinationPage(_db));
    }

    private async void EditDestination(object sender, EventArgs e)
    {
        var destination = (sender as Button)?.BindingContext as Destination;
        if (destination != null)
            await Navigation.PushAsync(new DestinationDetailsPage(destination, _db));
    }

    private async void ActivateDestination(object sender, EventArgs e)
    {
        var destination = (sender as Button)?.BindingContext as Destination;

        if (destination != null)
        {
            if (!destination.Status.Equals("complete"))
            {
                destination.Status = "active";
                await _db.SaveDestinationAsync(destination);
            }
            else
            {
                await DisplayAlert("Внимание", "Статусът не може да бъде променен", "OK");
            }
        }
    }

    private async void CompletedDestination(object sender, EventArgs e)
    {
        var button = sender as Button;
        var destination = button?.BindingContext as Destination;

        if (destination != null)
        {
            if (destination.Status == "active")
            {
                destination.Status = "complete";
                await _db.SaveDestinationAsync(destination);
            }
            else
            {
                await DisplayAlert("Внимание", "Статусът не може да бъде променен", "OK");
            }
        }
    }

    private async void DeleteDestination(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var destination = (Destination)button.BindingContext;

        bool confirm = await DisplayAlert("Confirm", "Delete this destination?", "Yes", "No");
        if (confirm)
        {
            await _db.DeleteDestinationAsync(destination);
            Destinations.Remove(destination);
        }
    }
}