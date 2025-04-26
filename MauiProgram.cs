using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
			return builder.Build();

			//builder.Services.AddDbContext<AppDbContext>(options =>
			//options.UseSqlServer(connectionString));

			//builder.Services.AddScoped<IPatientRepository, PatientRepository>();
			//builder.Services.AddScoped<PatientService>(); 
		}
    }
}
