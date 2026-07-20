using Cormei.Core.Global;
using Cormei.Core.Interfaces.Actividades;
using Cormei.Core.Models.Actividades;
using Cormei.Core.Services.Login;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Cormei.Core.Services.Actividades
{
    public class ActividadesService : IActividadesService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;
        private const string UrlMaster = "https://localhost:44305";

        public ActividadesService(HttpClient httpClient, AuthState authState)
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

        public async Task<List<ActividadCalendarioDto>> ObtenerCalendarioAsync(int anio, int mes, int quincena)
        {
            try
            {
                
                var mensaje = CrearMensaje(HttpMethod.Get, $"/Actividades/calendario?anio={anio}&mes={mes}&quincena={quincena}");

                env.FirmarPeticion(mensaje);

                HttpResponseMessage response = await _httpClient.SendAsync(mensaje);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<CalendarioActividadesResponse>();
                    return resultado?.Mensaje ?? new List<ActividadCalendarioDto>();
                }

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(string.IsNullOrWhiteSpace(errorContent) ? "No se pudo obtener el catálogo de actividades." : errorContent);
            }
            catch (Exception ex) when (ex is not HttpRequestException)
            {
                throw new HttpRequestException("No se pudo conectar con el servidor. Intenta nuevamente.", ex);
            }
        }

        public async Task<(bool ok, string mensaje, string? idRegistro)> RegistrarAsync(RegistrarActividadRequest dto)
        {
            try
            {
                var mensaje = CrearMensaje(HttpMethod.Post, "/Actividades/registrar");
                mensaje.Content = JsonContent.Create(dto);

                env.FirmarPeticion(mensaje);

                HttpResponseMessage response = await _httpClient.SendAsync(mensaje);
                var resultado = await response.Content.ReadFromJsonAsync<RegistrarActividadResponse>();

                return resultado is not null
                    ? (resultado.Ok, resultado.Mensaje, resultado.IdRegistro)
                    : (false, "No se pudo registrar la actividad.", null);
            }
            catch
            {
                return (false, "No se pudo conectar con el servidor. Intenta nuevamente.", null);
            }
        }
    }
}