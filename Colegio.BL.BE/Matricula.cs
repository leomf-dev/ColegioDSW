using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class Matricula
    {
        public int IdMatricula { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un alumno")]
        public int IdAlumno { get; set; }

        [Required(ErrorMessage = "La fecha de matrícula es obligatoria")]
        public DateTime FechaMatricula { get; set; }

        [Required(ErrorMessage = "El período es obligatorio")]
        [StringLength(50, ErrorMessage = "El período no puede exceder 50 caracteres")]
        public string Periodo { get; set; }

        public string Estado { get; set; } = "Activa";
        public DateTime? FechaEstado { get; set; }
        public string Observacion { get; set; }

        // Extras para la vista
        [Required(ErrorMessage = "Debe seleccionar al menos un curso")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un curso")]
        public List<int> CursosSeleccionados { get; set; }

        // Propiedades de navegación simples (para mostrar datos)
        public string NombreAlumno { get; set; }
        public string ApellidoAlumno { get; set; }
        public string DNIAlumno { get; set; }

        // Para mostrar cursos matriculados
        public List<Nota> Notas { get; set; } = new List<Nota>();
    }
}
