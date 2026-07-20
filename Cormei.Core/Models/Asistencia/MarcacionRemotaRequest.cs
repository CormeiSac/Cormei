using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Asistencia
{
    public class MarcacionRemotaRequest
    {
        public int Sede { get; set; }
        public int IdBiometrico { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string? Observacion { get; set; }
        public string? Ruta { get; set; }
    }

    public class SubirFotoResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("ruta")]
        public string Ruta { get; set; } = string.Empty;

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;
    }

    public class RegistrarMarcacionResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [JsonPropertyName("idMarcacion")]
        public int? IdMarcacion { get; set; }
    }
}
