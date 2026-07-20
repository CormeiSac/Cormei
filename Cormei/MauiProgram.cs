
using Microsoft.Extensions.Logging;
using Cormei.Core;
using Cormei.Core.Interfaces.Escaneo;
using Cormei.Services;
using ZXing.Net.Maui.Controls;

namespace Cormei
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the Cormei.Shared project
            //builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddCoreServices();
            builder.Services.AddSingleton<IQrScannerService, QrScannerService>();

            try
            {
                return builder.Build();
            }
            catch (Exception ex)
            {
                // Pon un punto de interrupción (F9) en la línea de abajo para leer qué dice "ex"
                System.Diagnostics.Debug.WriteLine($"¡CRASH DE ARRANQUE!: {ex.Message}");
                throw;
            }
        }
    }
}
