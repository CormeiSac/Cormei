using Cormei.Core.Interfaces.Asistencia;
using Cormei.Core.Interfaces.Trabajadores;
using Cormei.Core.Models.Asistencia;
using Cormei.Core.Services.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cormei.Shared.Pages.Areas.Asistencia;

public partial class MarcacionRemota : ComponentBase
{
    [Inject] private AuthState AuthState { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private IAsistenciaService AsistenciaService { get; set; } = default!;
    [Inject] private IEmpleadosService EmpleadosService { get; set; } = default!;

    private const long MaxBytesFoto = 5 * 1024 * 1024;

    private List<SedeDto> sedes = new();
    private bool cargandoSedes = true;

    private DateTime fecha = DateTime.Today;
    private int campusSeleccionado;
    private int campusAutocompletado;
    private string numeroDocumento = string.Empty;
    private string nombre = string.Empty;
    private string apellido = string.Empty;
    private string ubicacion = string.Empty;
    private string razon = string.Empty;

    private IBrowserFile? fotoSeleccionada;
    private string? fotoPreviewUrl;

    private bool enviando;
    private bool? envioExitoso;
    private string? mensajeResultado;

    private bool FormularioValido =>
        campusSeleccionado > 0 &&
        !string.IsNullOrWhiteSpace(numeroDocumento) &&
        !string.IsNullOrWhiteSpace(nombre) &&
        !string.IsNullOrWhiteSpace(apellido) &&
        !string.IsNullOrWhiteSpace(ubicacion) &&
        !string.IsNullOrWhiteSpace(razon) &&
        fotoSeleccionada is not null;

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/login");
            return;
        }

        try
        {
            sedes = await AsistenciaService.ListaSedesAsync();
        }
        catch (Exception ex)
        {
            envioExitoso = false;
            mensajeResultado = ex.Message;
        }
        finally
        {
            cargandoSedes = false;
        }

        await AutocompletarDatosUsuarioAsync();
    }

    private async Task AutocompletarDatosUsuarioAsync()
    {
        try
        {
            var empleados = await EmpleadosService.ListaEmpleadosAsync();
            var trabajadorActual = empleados.FirstOrDefault(e => e.IdTrabajador == AuthState.CodTrabajador);
            if (trabajadorActual is null)
            {
                return;
            }

            numeroDocumento = trabajadorActual.Documento;
            nombre = trabajadorActual.Nombres;
            apellido = trabajadorActual.Apellidos;

            var sedeDelTrabajador = sedes.FirstOrDefault(s => trabajadorActual.IdSede.HasValue && s.IdSede == trabajadorActual.IdSede.Value);
            if (sedeDelTrabajador is not null)
            {
                campusSeleccionado = campusAutocompletado = sedeDelTrabajador.IdBiometrico;
            }
        }
        catch
        {
            // Autocompletado best-effort: si falla, el usuario completa los campos manualmente.
        }
    }

    private async Task Volver()
    {
        await JS.InvokeVoidAsync("history.back");
    }

    private async Task OnFotoSeleccionada(InputFileChangeEventArgs e)
    {
        fotoSeleccionada = e.File;

        using var stream = e.File.OpenReadStream(MaxBytesFoto);
        using var memoria = new MemoryStream();
        await stream.CopyToAsync(memoria);

        fotoPreviewUrl = $"data:{e.File.ContentType};base64,{Convert.ToBase64String(memoria.ToArray())}";
    }

    private async Task Registrar()
    {
        if (!FormularioValido || fotoSeleccionada is null || enviando)
        {
            return;
        }

        enviando = true;
        mensajeResultado = null;

        try
        {
            var sede = sedes.First(s => s.IdBiometrico == campusSeleccionado);

            await using var streamFoto = fotoSeleccionada.OpenReadStream(MaxBytesFoto);
            var (okFoto, ruta, mensajeFoto) = await AsistenciaService.SubirFotoAsync(streamFoto, fotoSeleccionada.Name, fotoSeleccionada.ContentType);

            if (!okFoto)
            {
                envioExitoso = false;
                mensajeResultado = mensajeFoto;
                return;
            }

            var dto = new MarcacionRemotaRequest
            {
                Sede = sede.IdSede,
                IdBiometrico = sede.IdBiometrico,
                Documento = numeroDocumento,
                Observacion = razon,
                Ruta = ruta
            };

            var (ok, mensaje, _) = await AsistenciaService.RegistrarMarcacionAsync(dto);
            envioExitoso = ok;
            mensajeResultado = mensaje;

            if (ok)
            {
                LimpiarFormulario();
            }
        }
        finally
        {
            enviando = false;
        }
    }

    private void LimpiarFormulario()
    {
        campusSeleccionado = campusAutocompletado;
        ubicacion = string.Empty;
        razon = string.Empty;
        fotoSeleccionada = null;
        fotoPreviewUrl = null;
    }
}
