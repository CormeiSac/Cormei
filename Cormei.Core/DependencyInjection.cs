using Cormei.Core.Interfaces.Actividades;
using Cormei.Core.Interfaces.Asistencia;
using Cormei.Core.Interfaces.Escaneo;
using Cormei.Core.Interfaces.Login;
using Cormei.Core.Interfaces.RRHH;
using Cormei.Core.Interfaces.Trabajadores;
using Cormei.Core.Services.Actividades;
using Cormei.Core.Services.Asistencia;
using Cormei.Core.Services.Escaneo;
using Cormei.Core.Services.Login;
using Cormei.Core.Services.RRHH;
using Cormei.Core.Services.Trabajadores;
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

            #region Login
            services.AddScoped<AuthState>();
            services.AddHttpClient<IAuthService, AuthService>();
            #endregion

            #region RRHH
            services.AddHttpClient<IPostulacion, Postulacion>();

            #endregion

            #region Trabajadores
            services.AddHttpClient<IEmpleadosService, EmpleadosService>();
            #endregion

            #region Asistencia
            services.AddHttpClient<IAsistenciaService, AsistenciaService>();
            #endregion

            #region Actividades
            services.AddHttpClient<IActividadesService, ActividadesService>();
            #endregion

            #region Escaneo QR
            services.AddScoped<EscaneoState>();
            services.AddSingleton<IQrScannerService, QrScannerNoDisponibleService>();
            #endregion

            return services;
        }
    }
}