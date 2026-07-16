using Cormei.Core.Interfaces.Trabajadores;
using Cormei.Core.Models.Trabajadores;
using Cormei.Core.Services.Login;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Cormei.Core.Services.Trabajadores
{
    public class EmpleadosService : IEmpleadosService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;

        string urlMaster = "https://localhost:44305";

        public EmpleadosService(HttpClient httpClient, AuthState authState)
        {
            _httpClient = httpClient;
            _authState = authState;
        }

        public async Task<List<TrabajadorDto>> ListaEmpleadosAsync()
        {
            try
            {
                string url = $"{urlMaster}/Empleados/ListaEmpleados";

                var mensaje = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(_authState.AccessToken))
                {
                    mensaje.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authState.AccessToken);
                }

                HttpResponseMessage response = await _httpClient.SendAsync(mensaje);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ListaEmpleadosResponse>();
                    return resultado?.Mensaje ?? new List<TrabajadorDto>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(string.IsNullOrWhiteSpace(errorContent) ? "No se pudo obtener la lista de empleados." : errorContent);
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch
            {
                throw new HttpRequestException("No se pudo conectar con el servidor. Intenta nuevamente.");
            }
        }
    }
}
