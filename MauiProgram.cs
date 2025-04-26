using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using HospitalManagement.Data;

namespace HospitalManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
			var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            var config = new ConfigurationBuilder().AddJsonFile(configPath, optional: false, reloadOnChange: true).Build();
			builder.Configuration.AddConfiguration(config);

			// Database context
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

			builder.Services.AddScoped<IPatientRepository, PatientRepository>();
			builder.Services.AddScoped<PatientDataService>();

			return builder.Build();
		}
    }
}
