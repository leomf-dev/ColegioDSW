using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class SeccionBE
    {
        public int IdSeccion { get; set; }
        public int IdCurso { get; set; }
        public int IdDocente { get; set; }
        public string CodigoSeccion { get; set; } = string.Empty; // ej. INF101-A, INF101-B
        public string PeriodoAcademico { get; set; } = string.Empty; // ej. 2026-I
        public int CapacidadMaxima { get; set; } = 30;
        public int CupoDisponible { get; set; } = 0;
        public string Estado { get; set; } = "Activa";

        // Propiedades de navegación
        public string? NombreCurso { get; set; }
        public string? NombreDocente { get; set; }
        public List<HorarioBE> Horarios { get; set; } = new List<HorarioBE>();
    }
}