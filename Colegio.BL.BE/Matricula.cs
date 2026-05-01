using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class Matricula
    {
        public int IdMatricula { get; set; }
        public int IdAlumno { get; set; }
        public DateTime FechaMatricula { get; set; }
        public string Periodo { get; set; }

        // Extras para la vista
        public List<int> CursosSeleccionados { get; set; }

        // Propiedades de navegación simples (para mostrar datos)
        public string NombreAlumno { get; set; }
        public string ApellidoAlumno { get; set; }
        public string DNIAlumno { get; set; }
    }
}
