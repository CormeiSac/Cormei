using Cormei.Core.Interfaces.Login;
using Cormei.Core.Models.Login;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Cormei.Core.Services.Login
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        string urlMaster = "https://localhost:44305";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> Login(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("El nombre de usuario y la contraseña no pueden estar vacíos.");
            }

            try
            {
                string url = $"{urlMaster}/Authentication";
                var payload = new { Usuario = usuario, Password = password };

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<LoginResult>();
                    return resultado ?? throw new HttpRequestException("El servidor no devolvió datos de sesión.");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(string.IsNullOrWhiteSpace(errorContent) ? "No se pudo iniciar sesión." : errorContent);
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