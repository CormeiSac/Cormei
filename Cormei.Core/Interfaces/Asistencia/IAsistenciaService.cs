using Cormei.Core.Models.Asistencia;

namespace Cormei.Core.Interfaces.Asistencia
{
    public interface IAsistenciaService
    {
        Task<List<SedeDto>> ListaSedesAsync();

        Task<(bool ok, string ruta, string mensaje)> SubirFotoAsync(Stream contenido, string nombreArchivo, string contentType);

        Task<(bool ok, string mensaje, int? idMarcacion)> RegistrarMarcacionAsync(MarcacionRemotaRequest dto);
    }
}
