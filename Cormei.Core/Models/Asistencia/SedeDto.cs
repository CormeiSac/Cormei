using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Asistencia
{
    public class SedeDto
    {
        [JsonPropertyName("idBiometrico")]
        public int IdBiometrico { get; set; }

        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("nombreBiometrico")]
        public string NombreBiometrico { get; set; } = string.Empty;

        [JsonPropertyName("sede")]
        public string Sede { get; set; } = string.Empty;

        [JsonPropertyName("idSede")]
        public int IdSede { get; set; }

        [JsonPropertyName("estado")]
        public int Estado { get; set; }
    }

    public class ListaSedesResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("mensaje")]
        public List<SedeDto> Mensaje { get; set; } = new();
    }
}
