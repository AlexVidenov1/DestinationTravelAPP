using SQLite;

namespace DestinationTravelAPP.Models
{
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int DestinationId { get; set; }

        public string? Description { get; set; }
        public double Amount { get; set; }
    }
}
