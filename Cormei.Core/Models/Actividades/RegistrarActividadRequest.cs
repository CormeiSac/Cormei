using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Actividades
{
    public class RegistrarActividadRequest
    {
        public int IdActividad { get; set; }
        public int Minutos { get; set; }
        public int? Cantidad { get; set; }
        public string Comentario { get; set; } = string.Empty;
    }

    public class RegistrarActividadResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [JsonPropertyName("idRegistro")]
        public string? IdRegistro { get; set; }
    }
}