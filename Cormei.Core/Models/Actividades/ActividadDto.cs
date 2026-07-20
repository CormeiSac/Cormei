using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Actividades
{
    public class ActividadDto
    {
        [JsonPropertyName("idActividad")]
        public int IdActividad { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("repeticiones")]
        public int Repeticiones { get; set; }

        [JsonPropertyName("tiempoMinutos")]
        public int TiempoMinutos { get; set; }

        [JsonPropertyName("comentario")]
        public string Comentario { get; set; } = string.Empty;

        [JsonPropertyName("esFavorito")]
        public bool EsFavorito { get; set; }

        [JsonPropertyName("esMasUsada")]
        public bool EsMasUsada { get; set; }
    }
}