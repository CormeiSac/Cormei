using Cormei.Core.Interfaces.Actividades;
using Cormei.Core.Models.Actividades;
using Microsoft.AspNetCore.Components;

namespace Cormei.Shared.Pages.Areas.Actividades;

public partial class Actividades : ComponentBase
{
    private enum Pestana { MasUsadas, RestoDeLista, Favoritas }

    private const int MinutosMinimosLunesAViernes = 510;
    private const int MinutosMinimosSabado = 330;
    private const int VecesMinimoParaMasUsada = 2;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IActividadesService ActividadesService { get; set; } = default!;

    private Pestana pestanaActiva = Pestana.RestoDeLista;
    private string textoBusqueda = string.Empty;
    private bool cargando = true;
    private string? mensajeError;

    private List<ActividadDto> actividades = new();
    private decimal minutosRegistradosHoy;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var hoy = DateTime.Today;
            var quincena = hoy.Day <= 15 ? 1 : 2;
            var catalogo = await ActividadesService.ObtenerCalendarioAsync(hoy.Year, hoy.Month, quincena);

            actividades = catalogo.Select(a => new ActividadDto
            {
                IdActividad = a.IdActividad,
                Nombre = a.NombreActividad,
                Descripcion = a.Tipo,
                EsMasUsada = a.Dias.Count(d => d.Minutos is > 0) > VecesMinimoParaMasUsada,
                TiempoMinutos = (int)Math.Round(a.Dias.FirstOrDefault(d => d.Fecha.Date == hoy)?.Minutos ?? 0)
            }).ToList();

            minutosRegistradosHoy = catalogo
                .SelectMany(a => a.Dias)
                .Where(d => d.Fecha.Date == hoy)
                .Sum(d => d.Minutos ?? 0);
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

    private IEnumerable<ActividadDto> ActividadesFiltradas
    {
        get
        {
            IEnumerable<ActividadDto> lista = pestanaActiva switch
            {
                Pestana.MasUsadas => actividades.Where(a => a.EsMasUsada),
                Pestana.RestoDeLista => actividades.Where(a => !a.EsMasUsada),
                Pestana.Favoritas => actividades.Where(a => a.EsFavorito),
                _ => actividades
            };

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                lista = lista.Where(a =>
                    a.Nombre.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    a.Descripcion.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase));
            }

            return lista;
        }
    }

    private int MinutosTotales => (int)Math.Round(minutosRegistradosHoy);

    private double HorasTotales => Math.Round((double)minutosRegistradosHoy / 60.0, 1);

    private int MinutoMinimoHoy => DateTime.Today.DayOfWeek switch
    {
        DayOfWeek.Saturday => MinutosMinimosSabado,
        DayOfWeek.Sunday => 0,
        _ => MinutosMinimosLunesAViernes
    };

    private double PorcentajeCumplido => MinutoMinimoHoy <= 0 ? 100 : MinutosTotales * 100.0 / MinutoMinimoHoy;

    private string ClaseFooterTotales => PorcentajeCumplido switch
    {
        >= 100 => "flex flex-col items-center justify-center bg-green-600 px-4 py-3 text-center text-white",
        >= 50 => "flex flex-col items-center justify-center bg-amber-400 px-4 py-3 text-center text-slate-900",
        _ => "flex flex-col items-center justify-center bg-cormei px-4 py-3 text-center text-white"
    };

    private void SeleccionarPestana(Pestana pestana) => pestanaActiva = pestana;

    private void AlternarFavorito(ActividadDto actividad) => actividad.EsFavorito = !actividad.EsFavorito;

    private void AbrirDetalle(ActividadDto actividad) => NavigationManager.NavigateTo($"/actividades/detalle/{actividad.IdActividad}");

    private string ClaseTab(Pestana pestana) => pestana == pestanaActiva
        ? "flex-1 bg-cormei px-2 py-3 text-center text-sm font-bold text-white"
        : "flex-1 bg-slate-100 px-2 py-3 text-center text-sm font-semibold text-slate-700 hover:bg-slate-200";
}