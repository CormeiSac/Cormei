using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Actividades
{
    public class ActividadCalendarioDto
    {
        [JsonPropertyName("idActividad")]
        public int IdActividad { get; set; }

        [JsonPropertyName("nombreActividad")]
        public string NombreActividad { get; set; } = string.Empty;

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("dias")]
        public List<TareaDiaDto> Dias { get; set; } = new();
    }

    public class TareaDiaDto
    {
        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("minutos")]
        public decimal? Minutos { get; set; }
    }
}