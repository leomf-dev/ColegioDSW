using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class Nota
    {
        public int IdNota { get; set; }
        public int IdMatricula { get; set; }
        public int IdCurso { get; set; }
        public decimal Calificacion { get; set; }

        // Propiedad de navegacion
        public string CursoNombre { get; set; }
        // Para consultas
        public int IdAlumno { get; set; }
    }
}
