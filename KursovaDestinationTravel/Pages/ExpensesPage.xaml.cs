using System.Collections.ObjectModel;
using DestinationTravelAPP.Models;
using DestinationTravelAPP.Services;

namespace DestinationTravelAPP.Pages;

public partial class ExpensesPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly Destination _destination;

    public ObservableCollection<Expense> Expenses { get; set; } = new();

    public ExpensesPage(Destination destination, DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        _destination = destination;

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Expenses.Clear();
        var list = await _db.GetExpensesForDestinationAsync(_destination.Id);
        foreach (var e in list)
            Expenses.Add(e);
    }

    private async void AddExpense_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(AmountEntry.Text, out double amount) && !string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            var expense = new Expense
            {
                DestinationId = _destination.Id,
                Description = DescriptionEntry.Text,
                Amount = amount
            };

            await _db.SaveExpenseAsync(expense);
            Expenses.Add(expense);

            AmountEntry.Text = "";
            DescriptionEntry.Text = "";
        }
        else
        {
            await DisplayAlert("Error", "??????? ??? ???? ??? ????????", "OK");
        }
    }
}