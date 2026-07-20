using Cormei.Core.Interfaces.Escaneo;
using Cormei.Core.Models.Escaneo;
using Cormei.Core.Services.Escaneo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cormei.Shared.Pages.Escaneo;

public partial class ResultadoEscaneo : ComponentBase
{
    private const string BaseDocumentos = "http://190.102.151.108";

    [Inject] private EscaneoState EscaneoState { get; set; } = default!;
    [Inject] private IQrScannerService QrScannerService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private ResultadoEscaneoDto? resultado;

    protected override void OnInitialized()
    {
        resultado = EscaneoState.Resultado;
    }

    private async Task EscanearOtroAsync()
    {
        var textoQr = await QrScannerService.EscanearAsync();

        // Console.WriteLine(textoQr);
        if (ResultadoEscaneoDto.TryParse(textoQr, out var nuevoResultado))
        {
            resultado = nuevoResultado;
            EscaneoState.Resultado = nuevoResultado;
        }
    }

    private async Task AbrirDocumentoAsync(string? rutaRelativa)
    {
        if (string.IsNullOrWhiteSpace(rutaRelativa))
        {
            return;
        }

        await JS.InvokeVoidAsync("open", $"{BaseDocumentos}{rutaRelativa}", "_blank");
    }

    private async Task CerrarAsync()
    {
        EscaneoState.Resultado = null;
        await JS.InvokeVoidAsync("history.back");
    }
}