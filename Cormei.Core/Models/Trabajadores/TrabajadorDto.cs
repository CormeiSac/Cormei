using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Trabajadores
{
    public class TrabajadorDto
    {
        [JsonPropertyName("idTrabajador")]
        public int? IdTrabajador { get; set; }

        [JsonPropertyName("documento")]
        public string Documento { get; set; } = string.Empty;

        [JsonPropertyName("nombres")]
        public string Nombres { get; set; } = string.Empty;

        [JsonPropertyName("apeParterno")]
        public string ApePaterno { get; set; } = string.Empty;

        [JsonPropertyName("apeMaterno")]
        public string ApeMaterno { get; set; } = string.Empty;

        [JsonIgnore]
        public string Apellidos => string.Join(' ', new[] { ApePaterno, ApeMaterno }.Where(a => !string.IsNullOrWhiteSpace(a)));

        [JsonPropertyName("tipoTrabajador")]
        public string TipoTrabajador { get; set; } = string.Empty;

        [JsonPropertyName("idSede")]
        public int? IdSede { get; set; }

        [JsonPropertyName("sede")]
        public string Sede { get; set; } = string.Empty;

        [JsonPropertyName("tipodoc")]
        public string TipoDoc { get; set; } = string.Empty;

        [JsonPropertyName("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [JsonPropertyName("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [JsonPropertyName("remuneracion")]
        public decimal? Remuneracion { get; set; }

        [JsonPropertyName("bonoFijo")]
        public decimal? BonoFijo { get; set; }

        [JsonPropertyName("cargo")]
        public string Cargo { get; set; } = string.Empty;

        [JsonPropertyName("nroHijos")]
        public int? NroHijos { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("fechainicio")]
        public DateTime? FechaInicio { get; set; }

        [JsonPropertyName("fechafin")]
        public DateTime? FechaFin { get; set; }

        [JsonPropertyName("sisPension")]
        public string SisPension { get; set; } = string.Empty;

        [JsonPropertyName("nombreBanco")]
        public string NombreBanco { get; set; } = string.Empty;

        [JsonPropertyName("cuentaBancaria")]
        public string CuentaBancaria { get; set; } = string.Empty;

        [JsonPropertyName("correo")]
        public string Correo { get; set; } = string.Empty;

        [JsonPropertyName("telefono")]
        public string Telefono { get; set; } = string.Empty;
    }
}
