using Cormei.Core.Interfaces.RRHH;
using Cormei.Core.Services.RRHH;
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Text;

namespace Cormei.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            //services.AddScoped<IYourService, YourService>();

            #region RRHH 
            services.AddHttpClient<IPostulacion, Postulacion>();            

            #endregion
            return services;
        }
    }
}