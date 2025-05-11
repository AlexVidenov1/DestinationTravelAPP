using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace DestinationTravelAPP.Models
{
    public class Destination : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public DateTime StartDate { get; set; }

        public int Duration { get; set; }
        public DateTime EndDate => StartDate.AddDays(Duration);
        public string? Purpose { get; set; }
        public double Rating { get; set; }
        private string? _status;
        public string Status
        {
#pragma warning disable CS8603 // Possible null reference return.
            get => _status;
#pragma warning restore CS8603 // Possible null reference return.
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
