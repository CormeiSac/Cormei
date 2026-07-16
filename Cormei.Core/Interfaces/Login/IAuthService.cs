using Cormei.Core.Models.Login;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Cormei.Core.Interfaces.Login
{
    public interface IAuthService
    {
        Task<LoginResult> Login(string usuario, string password);
    }
}
