using Cormei.Core.Interfaces.Actividades;
using Cormei.Core.Models.Actividades;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cormei.Shared.Pages.Areas.Actividades.ActividadDetalle;

public partial class ActividadDetalle : ComponentBase
{
    [Parameter] public int IdActividad { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private IActividadesService ActividadesService { get; set; } = default!;

    private ActividadDto? actividad;
    private int? minutos;
    private int? cantidad;
    private string comentario = string.Empty;
    private bool cargando = true;
    private string? mensajeError;
    private string? mensajeResultado;
    private bool enviando;

    private bool FormularioValido => minutos is > 0 && cantidad is > 0 && comentario.Trim().Length >= 5;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var hoy = DateTime.Today;
            var quincena = hoy.Day <= 15 ? 1 : 2;
            var catalogo = await ActividadesService.ObtenerCalendarioAsync(hoy.Year, hoy.Month, quincena);
            var encontrada = catalogo.FirstOrDefault(a => a.IdActividad == IdActividad);

            if (encontrada is not null)
            {
                actividad = new ActividadDto
                {
                    IdActividad = encontrada.IdActividad,
                    Nombre = encontrada.NombreActividad,
                    Descripcion = encontrada.Tipo
                };
            }
        }
        catch (Exception ex)
        {
            mensajeError = ex.Message;
        }
        finally
        {
            cargando = false;
        }
    }

    private async Task Registrar()
    {
        if (actividad is null || !FormularioValido || enviando)
        {
            return;
        }

        enviando = true;
        mensajeResultado = null;

        try
        {
            var dto = new RegistrarActividadRequest
            {
                IdActividad = actividad.IdActividad,
                Minutos = minutos ?? 0,
                Cantidad = cantidad,
                Comentario = comentario.Trim()
            };

            var (ok, mensaje, _) = await ActividadesService.RegistrarAsync(dto);

            if (ok)
            {
                await Volver();
                return;
            }

            mensajeResultado = mensaje;
        }
        finally
        {
            enviando = false;
        }
    }

    private async Task Volver()
    {
        await JS.InvokeVoidAsync("history.back");
    }
}
