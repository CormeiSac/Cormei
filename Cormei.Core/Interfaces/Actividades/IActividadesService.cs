using Cormei.Core.Models.Actividades;

namespace Cormei.Core.Interfaces.Actividades
{
    public interface IActividadesService
    {
        Task<List<ActividadCalendarioDto>> ObtenerCalendarioAsync(int anio, int mes, int quincena);
        Task<(bool ok, string mensaje, string? idRegistro)> RegistrarAsync(RegistrarActividadRequest dto);
    }
}