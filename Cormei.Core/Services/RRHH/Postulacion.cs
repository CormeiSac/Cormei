using Cormei.Core.Interfaces.RRHH;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace Cormei.Core.Services.RRHH
{
    public class Postulacion : IPostulacion
    {

        private readonly HttpClient _httpClient;

        string urlMasater = "https://localhost:44305";
        public Postulacion(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CargarDatos(byte[] archivoBytes, string nombreArchivo)
        {
            try
            {
                string url = $"{urlMasater}/postulacion/upload-cv";
                using var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(archivoBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                content.Add(fileContent, "file", nombreArchivo);
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al subir el CV. Código: {response.StatusCode}. Detalle: {errorContent}");
                }
            }
            catch 
            {
                throw;
            }
        }

        // STEP 1: Captura el JSON completo de CvParseadoResponse de tu API

        public async Task<string> ExtraerTextoDelCv(byte[] archivoBytes, string nombreArchivo)
        {
            try
            {
                string url = $"{urlMasater}/postulacion/parse";
                using var content = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(archivoBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                content.Add(fileContent, "archivo", nombreArchivo);

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadFromJsonAsync<JsonNode>();

                    if (json is JsonObject jsonObj)
                    {
                        jsonObj.Remove("fotoBase64");
                        jsonObj.Remove("FotoBase64");
                    }
                    return json?.ToString() ?? "";
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al parsear. Código: {response.StatusCode}. Detalle: {errorContent}");
                }
            }
            catch
            {
                throw;
            }
        }



        public async Task<Dictionary<string, object>> LLenarFormulario(string jsonDelCv)
        {
            try
            {
                string url = $"{urlMasater}/ia/consultar-deepseek";

                string promptDeepSeek = $@"
                    Actúa como un reclutador experto. Analiza la siguiente información extraída de un CV (viene en formato JSON) y extrae los datos exactos en este formato JSON estricto (sin texto adicional ni markdown).
                    Calcula la edad y formatea la fecha de nacimiento como YYYY-MM-DD. Suma los años de experiencia totales.

                    {{
                        ""Nombres"": """",
                        ""Apellidos"": """",
                        ""DNI"": """",
                        ""FechaNacimiento"": """",
                        ""Celular"": """",
                        ""Email"": """",
                        ""CargoPrincipal"": """",
                        ""CargoSecundario"": """",
                        ""OtrosCargos"": """",
                        ""ConocimientosCursos"": """",
                        ""AniosExperienciaTotales"": 0
                    }}

                    Información del CV (JSON): {(jsonDelCv.Length > 6000 ? jsonDelCv.Substring(0, 6000) : jsonDelCv)}
                    ";

                var payload = new { prompt = promptDeepSeek };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                    return resultado ?? new Dictionary<string, object>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error en DeepSeek. Código: {response.StatusCode}. Detalle: {errorContent}");
                }
            }
            catch
            {
                throw;
            }
        }



    }
}
