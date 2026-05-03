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
        public string Periodo { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activa";
        public DateTime? FechaEstado { get; set; }
        public string? Observacion { get; set; }

        // Extras para la vista
        [Required(ErrorMessage = "Debe seleccionar al menos una sección")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una sección")]
        public List<int> SeccionesSeleccionadas { get; set; } = new List<int>();

        // Propiedades de navegación simples (para mostrar datos)
        public string? NombreAlumno { get; set; }
        public string? ApellidoAlumno { get; set; }
        public string? DNIAlumno { get; set; }

        // Para mostrar detalles de matrícula
        public List<DetalleMatriculaBE> DetallesMatricula { get; set; } = new List<DetalleMatriculaBE>();
    }
}
