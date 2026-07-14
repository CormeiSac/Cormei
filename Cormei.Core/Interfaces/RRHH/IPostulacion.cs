using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cormei.Core.Interfaces.RRHH
{
    public interface IPostulacion 
    {
        Task CargarDatos(byte[] archivoBytes, string nombreArchivo);

        Task<string> ExtraerTextoDelCv(byte[] archivoBytes, string nombreArchivo);

        Task<Dictionary<string, object>> LLenarFormulario(string textoDelCv);


    }
}
