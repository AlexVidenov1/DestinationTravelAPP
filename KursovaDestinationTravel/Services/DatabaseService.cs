using DestinationTravelAPP.Models;
using SQLite;

namespace DestinationTravelAPP.Services
{
    public class DatabaseService
    {
        readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Destination>().Wait();
            _database.CreateTableAsync<Expense>().Wait();
        }

        public Task<List<Destination>> GetDestinationsAsync() =>
            _database.Table<Destination>().ToListAsync();
        public Task<int> SaveDestinationAsync(Destination dest)
        {
            if (string.IsNullOrWhiteSpace(dest.Country) || string.IsNullOrWhiteSpace(dest.City))
                return Task.FromResult(0);

            if (dest.Id == 0)
                return _database.InsertAsync(dest);
            else
                return _database.UpdateAsync(dest);
        }

        public Task<int> DeleteDestinationAsync(Destination dest) =>
            _database.DeleteAsync(dest);

        public Task<List<Expense>> GetExpensesForDestinationAsync(int destinationId) =>
    _database.Table<Expense>().Where(e => e.DestinationId == destinationId).ToListAsync();

        public Task<int> SaveExpenseAsync(Expense expense)
        {
            if (expense.Id == 0)
                return _database.InsertAsync(expense);
            else
                return _database.UpdateAsync(expense);
        }

        public Task<int> DeleteExpenseAsync(Expense expense) =>
            _database.DeleteAsync(expense);
    }
}
