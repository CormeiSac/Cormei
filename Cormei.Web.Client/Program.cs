
using Cormei.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the Cormei.Shared project
//builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddCoreServices();
await builder.Build().RunAsync();
