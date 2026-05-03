using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class Curso
    {
        public int IdCurso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }
        public int HorasSemanales { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
