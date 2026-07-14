using Cormei.Core.Interfaces.RRHH;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cormei.Shared.Pages
{
    public partial class Postulacion
    {
        [Inject]
        private IPostulacion PostulacionService { get; set; } = default!;

        private string mensajeEstado = "";
        private bool estaCargando = false;
        private List<ArchivoAdjunto> archivosProcesados = new();

        #region Campos del Formulario (Para el Auto-llenado)
        private string txtNombres { get; set; } = string.Empty;
        private string txtApellidos { get; set; } = string.Empty;
        private string txtDni { get; set; } = string.Empty;
        private DateTime? txtFechaNacimiento { get; set; }
        private string txtCelular { get; set; } = string.Empty;
        private string txtEmail { get; set; } = string.Empty;
        private string txtCargoPrincipal { get; set; } = string.Empty;
        private string txtCargoSecundario { get; set; } = string.Empty;
        private string txtOtrosCargos { get; set; } = string.Empty;
        private string txtExperienciaGeneral { get; set; } = string.Empty;
        private string txtConocimientosCursos { get; set; } = string.Empty;
        #endregion

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            // Límite de 10 MB asignado para el flujo en memoria
            long maxBytesPermitidos = 50 * 1024 * 1024;

            // CORREGIDO: Se usa el parámetro directo (10) para definir el máximo de archivos permitidos de golpe
            foreach (var file in e.GetMultipleFiles(10))
            {
                var nuevoArchivo = new ArchivoAdjunto
                {
                    Nombre = file.Name,
                    TamanoBytes = file.Size,
                    Estado = "Leyendo documento..."
                };

                archivosProcesados.Add(nuevoArchivo);
                StateHasChanged();

                if (file.Size > maxBytesPermitidos)
                {
                    nuevoArchivo.Estado = "Error: Supera los 10MB";
                    StateHasChanged();
                    continue;
                }

                try
                {
                    using var stream = file.OpenReadStream(maxBytesPermitidos);
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    byte[] archivoBytes = memoryStream.ToArray();

                    nuevoArchivo.Estado = "Procesando con IA...";
                    StateHasChanged();

                    string jsonCv = await PostulacionService.ExtraerTextoDelCv(archivoBytes, file.Name);

                    if (string.IsNullOrWhiteSpace(jsonCv))
                    {
                        nuevoArchivo.Estado = "Error: La API no devolvió datos válidos.";
                        StateHasChanged();
                        continue;
                    }

                    Dictionary<string, object>? contenedorEnvoltorio = await PostulacionService.LLenarFormulario(jsonCv);

                    if (contenedorEnvoltorio != null && contenedorEnvoltorio.TryGetValue("data", out var dataObjeto) && dataObjeto != null)
                    {
                        string jsonInternoRaw = dataObjeto.ToString() ?? "";
                        jsonInternoRaw = jsonInternoRaw.Replace("```json", "").Replace("```", "");
                        jsonInternoRaw = jsonInternoRaw.Replace("\"\"\"json", "").Replace("\"\"\"", "");
                        jsonInternoRaw = jsonInternoRaw.Trim();

                        var datosIaReales = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonInternoRaw);

                        if (datosIaReales != null && datosIaReales.Count > 0)
                        {
                            MapearDatosFormulario(datosIaReales);
                            nuevoArchivo.Estado = "Listo";
                        }
                        else
                        {
                            nuevoArchivo.Estado = "Error: No se pudo estructurar el perfil.";
                        }
                    }
                    else
                    {
                        nuevoArchivo.Estado = "Error: La respuesta no contiene la propiedad 'data'.";
                    }
                }
                catch (Exception ex)
                {
                    nuevoArchivo.Estado = $"Error: {ex.Message}";
                }
                finally
                {
                    StateHasChanged();
                }
            }
        }

        private void MapearDatosFormulario(Dictionary<string, object> datos)
        {
            txtNombres = datos.TryGetValue("Nombres", out var n) ? n?.ToString() ?? "" : "";
            txtApellidos = datos.TryGetValue("Apellidos", out var a) ? a?.ToString() ?? "" : "";
            txtDni = datos.TryGetValue("DNI", out var d) ? d?.ToString() ?? "" : "";

            string fechaTexto = datos.TryGetValue("FechaNacimiento", out var f) ? f?.ToString() ?? "" : "";
            if (DateTime.TryParse(fechaTexto, out var fechaParseada))
            {
                txtFechaNacimiento = fechaParseada;
            }
            else
            {
                txtFechaNacimiento = null;
            }

            txtCelular = datos.TryGetValue("Celular", out var c) ? c?.ToString() ?? "" : "";
            txtEmail = datos.TryGetValue("Email", out var em) ? em?.ToString() ?? "" : "";
            txtCargoPrincipal = datos.TryGetValue("CargoPrincipal", out var cp) ? cp?.ToString() ?? "" : "";
            txtCargoSecundario = datos.TryGetValue("CargoSecundario", out var cs) ? cs?.ToString() ?? "" : "";
            txtOtrosCargos = datos.TryGetValue("OtrosCargos", out var oc) ? oc?.ToString() ?? "" : "";
            txtConocimientosCursos = datos.TryGetValue("ConocimientosCursos", out var cc) ? cc?.ToString() ?? "" : "";

            if (datos.TryGetValue("AniosExperienciaTotales", out var expObj) && double.TryParse(expObj?.ToString(), out double anios))
            {
                if (anios < 1) txtExperienciaGeneral = "Menos de 1 año";
                else if (anios >= 1 && anios <= 3) txtExperienciaGeneral = "1 a 3 años";
                else if (anios > 3 && anios <= 5) txtExperienciaGeneral = "3 a 5 años";
                else if (anios > 5) txtExperienciaGeneral = "Más de 5 años";
            }
        }

        private void EliminarArchivo(ArchivoAdjunto archivo)
        {
            archivosProcesados.Remove(archivo);
            StateHasChanged();
        }

        public class ArchivoAdjunto
        {
            public string Nombre { get; set; } = string.Empty;
            public long TamanoBytes { get; set; }
            public string Estado { get; set; } = string.Empty;
            public string TamanoMegas => $"{(TamanoBytes / 1024.0 / 1024.0):0.##} MB";
        }
    }
}