# Cormei — Documentación corta

## ¿Qué es?
Aplicación cliente construida con **.NET MAUI Blazor Hybrid + Blazor Web App**, que consume la API `ApiCormei`. La misma UI (Razor Components) corre en móvil/desktop (MAUI) y en la web (Blazor Server + WebAssembly). El caso de uso principal hoy es el módulo de reclutamiento (subida y parseo automático de CVs).

## Estructura del repositorio
```
Cormei.slnx
├─ Cormei/            → app MAUI Blazor Hybrid (Android, iOS, MacCatalyst, Windows)
│  └─ MauiProgram.cs  → registra los servicios compartidos (AddCoreServices)
├─ Cormei.Core/       → capa de servicios compartida: los HttpClient que llaman a ApiCormei
│  ├─ Interfaces/RRHH/IPostulacion.cs + Services/RRHH/Postulacion.cs
│  │     → patrón ya usado para consumir la API (CargarDatos, ExtraerTextoDelCv, LLenarFormulario)
│  └─ DependencyInjection.cs (AddCoreServices) → único punto donde se registran los servicios/HttpClients
├─ Cormei.Shared/     → Razor Class Library con páginas y layout compartidos entre MAUI, Web y WASM
│  ├─ Pages/Postulacion.razor(.cs) → formulario de postulación ya integrado con la API
│  ├─ Pages/Login.razor            → login (hoy es solo un placeholder, ver abajo)
│  └─ Layout/MainLayout.razor, NavMenu.razor
├─ Cormei.Web/        → host Blazor Web App (Server + WASM interactivo)
└─ Cormei.Web.Client/ → proyecto Blazor WebAssembly puro
```

## Cómo se conecta con ApiCormei
Los servicios de `Cormei.Core` (p. ej. `Postulacion.cs`) llaman directo a la URL de la API — hoy hardcodeada como `https://localhost:44305` — usando un `HttpClient` inyectado vía `AddHttpClient` en `DependencyInjection.cs`.

## 🔑 Dónde empezar: login a nivel de usuario (para integrar con ApiCormei)
Hoy **no existe login funcional**, solo un placeholder. Siguiendo el mismo patrón que ya usa `Postulacion`, el punto de partida es:

1. **`Cormei.Shared/Pages/Login.razor`** — hoy solo tiene `<h3>Login</h3>`. Aquí va el formulario (usuario/contraseña) y la lógica de UI del login.
2. Crear **`Cormei.Core/Interfaces/Login/IAuthService.cs`** y **`Cormei.Core/Services/Login/AuthService.cs`** — un servicio `HttpClient` que llame a `POST {urlMaster}/Authentication` de ApiCormei, con el mismo contrato: `Usuario/Password` → `access_token`, `refresh_token`, `refresh_expires`.
3. Registrar ese nuevo servicio en **`Cormei.Core/DependencyInjection.cs`** (`AddCoreServices`), igual que está registrado `IPostulacion`.
4. Decidir dónde se guarda el token para reusarlo en las siguientes llamadas (`SecureStorage` en MAUI, `localStorage`/sessionStorage en WASM, o un servicio de estado en Blazor Server), y agregarlo como header `Authorization: Bearer` en los HttpClients existentes.
5. Proteger páginas/rutas que requieran sesión (`AuthorizeView` / `CascadingAuthenticationState`) y usar `POST /Authentication/AccesoApp` de ApiCormei para cargar los módulos visibles según el usuario logueado.

## Cómo correrlo
- **Web** (`Cormei.Web`): `dotnet run` desde esa carpeta.
- **App MAUI** (`Cormei`): seleccionar el proyecto `Cormei` y ejecutar sobre Windows/Android/iOS desde Visual Studio.



Propuesta para la capa "Home + menús" (sin tocar código todavía):

1. Qué dispara la carga del menú
Justo después del login exitoso (en ManejarLogin, antes de redirigir a /Home), se llama a POST /Authentication/AccesoApp con el CodUser que ya viene en LoginResult. Esa respuesta (lista de módulos vía el SP AccesosModulos) se guarda en un nuevo estado en memoria — por ejemplo MenuState en Cormei.Core, hermano de AuthState — con la lista de módulos habilitados para ese usuario. Así el menú no es fijo en el código: se arma según lo que el backend diga que ese usuario puede ver.

2. Layout autenticado (separado del login)
Home.razor y todo lo que venga después dejan de usar LoginLayout (que es a propósito "aislado", sin chrome). Se activa MainLayout.razor como layout real: barra superior con el logo/usuario actual + botón de cerrar sesión, y el sidebar (NavMenu.razor, hoy vacío/comentado) generado dinámicamente recorriendo MenuState en vez de links quemados.

3. Contenido de Home
Una vista simple de bienvenida: saludo con el usuario (AuthState.Usuario), y accesos rápidos/tarjetas hacia los módulos que trajo AccesoApp (los mismos que aparecen en el sidebar, pero como shortcuts grandes). Nada de formularios todavía — es la "pantalla de aterrizaje" tras el login.

4. Guard de ruta
Tanto Home como cualquier página futura dentro de MainLayout verifican AuthState.IsAuthenticated; si es falso (por ejemplo, entraron directo a /Home sin login o refrescaron y se perdió la sesión en memoria), redirige a /.

Queda fuera por ahora (según lo que dijiste): Postulación, y el escáner QR del botón flotante — ninguno de los dos se toca en esta capa.

¿Avanzo con esto tal cual, o ajustamos algo (por ejemplo, si el menú debe ser fijo por ahora y dejamos AccesoApp para después)?