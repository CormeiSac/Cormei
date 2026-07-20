using Cormei.Core.Interfaces.Trabajadores;
using Cormei.Core.Models.Trabajadores;
using Microsoft.AspNetCore.Components;

namespace Cormei.Shared.Pages.Areas.Trabajadores;

public partial class Trabajador : ComponentBase
{
    [Inject] private IEmpleadosService EmpleadosService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private List<TrabajadorDto> empleados = new();
    private bool cargando = true;
    private string mensajeError = string.Empty;
    private string textoBusqueda = string.Empty;

    private List<TrabajadorDto> EmpleadosFiltrados => string.IsNullOrWhiteSpace(textoBusqueda)
        ? empleados
        : empleados.Where(e => e.Nombres.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)).ToList();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            empleados = await EmpleadosService.ListaEmpleadosAsync();
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

    private void AbrirDetalle(TrabajadorDto emp)
    {
        NavigationManager.NavigateTo($"empleados/detalle/{emp.IdTrabajador}");
    }

    private bool EsActivo(TrabajadorDto emp) => string.Equals(emp.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase);

      private enum EstadoVista { 
    
     
        Cargando,
        Error,
        NoEncontrado,
        Listo

    }

    private EstadoVista EstadoActual
    {
        get
        {
            if (cargando) return EstadoVista.Cargando;
            if (!string.IsNullOrEmpty(mensajeError)) return EstadoVista.Error;
            if (EmpleadosFiltrados.Count == 0) return EstadoVista.NoEncontrado;
            return EstadoVista.Listo;
        }
    }
}
