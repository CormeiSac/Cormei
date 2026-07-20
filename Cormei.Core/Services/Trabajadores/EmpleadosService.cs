using Cormei.Core.Global;
using Cormei.Core.Interfaces.Trabajadores;
using Cormei.Core.Models.Trabajadores;
using Cormei.Core.Services.Login;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Cormei.Core.Services.Trabajadores
{
    public class EmpleadosService : IEmpleadosService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthState _authState;
        private const string SecretKeyCompartida = "i7ldYzs8k65b|6:U=q2%b=Sl)gqn}0NC;.Cormei";
        // Mantenemos HTTP puro (puerto 80)
        string urlMaster = "https://localhost:44305";
        // string urlMaster = "http://190.102.151.108/ApiCormei";

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


                env.FirmarPeticion(mensaje);
                
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
            catch (Exception ex)
            {
                throw new HttpRequestException("Error de conexión: " + ex.Message);
            }
        }
    }
}