using Cormei.Core.Interfaces.Trabajadores;
using Cormei.Core.Models.Trabajadores;
using Microsoft.AspNetCore.Components;

namespace Cormei.Shared.Pages.Areas.Trabajadores.TrabajarDetalle;

public partial class TrabajadorDetalle : ComponentBase
{
    [Parameter] public int IdTrabajador { get; set; }

    [Inject] private IEmpleadosService EmpleadosService { get; set; } = default!;

    private TrabajadorDto? empleado;
    private bool cargando = true;
    private string mensajeError = string.Empty;

    private bool mostrarModalPerfil = false;
    private bool mostrarModalCargo = false;
    private bool mostrarModalFechaNacimiento = false;

    private bool EsActivo => string.Equals(empleado?.Estado, "ACTIVO", StringComparison.OrdinalIgnoreCase);

    private int? Edad
    {
        get
        {
            if (empleado?.FechaNacimiento is not DateTime nacimiento)
            {
                return null;
            }

            var hoy = DateTime.Today;
            var edad = hoy.Year - nacimiento.Year;
            if (nacimiento.Date > hoy.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var lista = await EmpleadosService.ListaEmpleadosAsync();
            empleado = lista.FirstOrDefault(e => e.IdTrabajador == IdTrabajador);
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
            if (empleado is null) return EstadoVista.NoEncontrado;
            return EstadoVista.Listo;
        }
    }
}