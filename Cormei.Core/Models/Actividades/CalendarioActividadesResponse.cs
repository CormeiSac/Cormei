using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Actividades
{
    public class CalendarioActividadesResponse
    {
        [JsonPropertyName("mensaje")]
        public List<ActividadCalendarioDto> Mensaje { get; set; } = new();
    }
}