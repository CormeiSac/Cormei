using Cormei.Core.Interfaces.Escaneo;

namespace Cormei.Services
{
    public class QrScannerService : IQrScannerService
    {
        public bool EstaDisponible => MainPage.Current is not null;

        public Task<string?> EscanearAsync()
        {
            return MainPage.Current?.IniciarEscaneoAsync() ?? Task.FromResult<string?>(null);
        }
    }
}