using Cormei.Core.Models.Trabajadores;

namespace Cormei.Core.Interfaces.Trabajadores
{
    public interface IEmpleadosService
    {
        Task<List<TrabajadorDto>> ListaEmpleadosAsync();
    }
}
