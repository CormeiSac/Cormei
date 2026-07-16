using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Trabajadores
{
    public class ListaEmpleadosResponse
    {
        [JsonPropertyName("mensaje")]
        public List<TrabajadorDto> Mensaje { get; set; } = new();
    }
}
