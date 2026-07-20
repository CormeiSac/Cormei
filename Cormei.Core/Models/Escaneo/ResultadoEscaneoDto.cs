namespace Cormei.Core.Models.Escaneo
{
    public class ResultadoEscaneoDto
    {
        public int CodTrabajador { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;
        public string? Iperc { get; set; }
        public string? Politica { get; set; }
        public string? MatrizAmbiente { get; set; }
        public string? RespuestaEmergencia { get; set; }
        public string? HojaSds { get; set; }

        public static bool TryParse(string? textoQr, out ResultadoEscaneoDto? resultado)
        {
            resultado = null;

            if (string.IsNullOrWhiteSpace(textoQr))
            {
                return false;
            }

            var campos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var parte in textoQr.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var separador = parte.IndexOf(':');
                if (separador <= 0)
                {
                    continue;
                }

                var clave = parte[..separador].Trim();
                var valor = parte[(separador + 1)..].Trim();
                campos[clave] = valor;
            }

            if (!campos.TryGetValue("Documento", out var documento) || string.IsNullOrWhiteSpace(documento))
            {
                return false;
            }

            resultado = new ResultadoEscaneoDto
            {
                CodTrabajador = campos.TryGetValue("CodTrabajador", out var codTrabajador) && int.TryParse(codTrabajador, out var cod) ? cod : 0,
                Documento = documento,
                Nombres = campos.GetValueOrDefault("Nombres", string.Empty),
                Area = campos.GetValueOrDefault("Area", string.Empty),
                Categoria = campos.GetValueOrDefault("Categoria", string.Empty),
                Cargo = campos.GetValueOrDefault("Cargo", string.Empty),
                Sede = campos.GetValueOrDefault("Sede", string.Empty),
                Iperc = campos.GetValueOrDefault("IPERC"),
                Politica = campos.GetValueOrDefault("Politica"),
                MatrizAmbiente = campos.GetValueOrDefault("MatrizAmbiente"),
                RespuestaEmergencia = campos.GetValueOrDefault("RespuestaEmergencia"),
                HojaSds = campos.GetValueOrDefault("HojaSDS")
            };

            return true;
        }
    }
}