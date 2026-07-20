using Cormei.Core.Global;
using Cormei.Core.Interfaces.Asistencia;
using Cormei.Core.Models.Asistencia;
using Cormei.Core.Services.Login;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Cormei.Core.Services.Asistencia
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;
        private const string UrlMaster = "https://localhost:44305"; 
        // private const string UrlMaster = "http://190.102.151.108/ApiCormei"; 

        public AsistenciaService(HttpClient httpClient, AuthState authState)
        {
            _httpClient = httpClient;
            _authState = authState;
        }

        private HttpRequestMessage CrearMensaje(HttpMethod metodo, string ruta)
        {
            var mensaje = new HttpRequestMessage(metodo, $"{UrlMaster}{ruta}");
            if (!string.IsNullOrEmpty(_authState.AccessToken))
            {
                mensaje.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.AccessToken);
            }
            return mensaje;
        }

        public async Task<List<SedeDto>> ListaSedesAsync()
        {
            try
            {
                var mensaje = CrearMensaje(HttpMethod.Get, "/MarcacionRemota/sedes");
                
                env.FirmarPeticion(mensaje); 

                HttpResponseMessage response = await _httpClient.SendAsync(mensaje);
                
                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ListaSedesResponse>();
                    return resultado?.Mensaje ?? new List<SedeDto>();
                }

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(string.IsNullOrWhiteSpace(errorContent) ? "No se pudo obtener la lista de sedes." : errorContent);
            }
            catch (Exception ex) when (ex is not HttpRequestException)
            {
                throw new HttpRequestException("No se pudo conectar con el servidor. Intenta nuevamente.", ex);
            }
        }

        public async Task<(bool ok, string ruta, string mensaje)> SubirFotoAsync(Stream contenido, string nombreArchivo, string contentType)
        {
            try
            {
                var mensaje = CrearMensaje(HttpMethod.Post, "/MarcacionRemota/subir-foto");

                using var contenidoForm = new MultipartFormDataContent();
                using var streamContent = new StreamContent(contenido);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                contenidoForm.Add(streamContent, "foto", nombreArchivo);
                mensaje.Content = contenidoForm;

                env.FirmarPeticion(mensaje);

                HttpResponseMessage response = await _httpClient.SendAsync(mensaje);
                var resultado = await response.Content.ReadFromJsonAsync<SubirFotoResponse>();

                return resultado is not null
                    ? (resultado.Ok, resultado.Ruta, resultado.Mensaje)
                    : (false, "", "No se pudo subir la foto.");
            }
            catch
            {
                return (false, "", "No se pudo conectar con el servidor. Intenta nuevamente.");
            }
        }

        public async Task<(bool ok, string mensaje, int? idMarcacion)> RegistrarMarcacionAsync(MarcacionRemotaRequest dto)
        {
            try
            {
                var mensaje = CrearMensaje(HttpMethod.Post, "/MarcacionRemota/registrar");
                mensaje.Content = JsonContent.Create(dto);

                // Este ya estaba en la posición correcta (Antes del SendAsync)
                env.FirmarPeticion(mensaje);

                HttpResponseMessage response = await _httpClient.SendAsync(mensaje);
                var resultado = await response.Content.ReadFromJsonAsync<RegistrarMarcacionResponse>();

                return resultado is not null
                    ? (resultado.Ok, resultado.Mensaje, resultado.IdMarcacion)
                    : (false, "No se pudo registrar la marcación.", null);
            }
            catch
            {
                return (false, "No se pudo conectar con el servidor. Intenta nuevamente.", null);
            }
        }
    }
}