using Cormei.Core.Interfaces.Escaneo;

namespace Cormei.Core.Services.Escaneo
{
    public class QrScannerNoDisponibleService : IQrScannerService
    {
        public bool EstaDisponible => false;

        public Task<string?> EscanearAsync() => Task.FromResult<string?>(null);
    }
}