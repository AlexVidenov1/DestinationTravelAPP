using System.Diagnostics;
using DestinationTravelAPP.Services;
using Microsoft.Extensions.Logging;

namespace DestinationTravelAPP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Debug.WriteLine(" STARTTTT");
            var builder = MauiApp.CreateBuilder();
            Debug.WriteLine(FileSystem.AppDataDirectory);

            builder.Services.AddSingleton<DatabaseService>(s =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "notes.db4");
                return new DatabaseService(dbPath);
            });

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<App>();
#if DEBUG

            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
