namespace Cormei.Core.Interfaces.Escaneo
{
    public interface IQrScannerService
    {
        bool EstaDisponible { get; }

        Task<string?> EscanearAsync();
    }
}