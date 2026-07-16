using Cormei.Core.Models.Login;

namespace Cormei.Core.Services.Login
{
    public class AuthState
    {
        public string? Usuario { get; private set; }
        public string? AccessToken { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);

        public event Action? OnChange;

        public void SetSession(LoginResult login)
        {
            Usuario = login.Usuario;
            AccessToken = login.AccessToken;
            OnChange?.Invoke();
        }

        public void ClearSession()
        {
            Usuario = null;
            AccessToken = null;
            OnChange?.Invoke();
        }
    }
}