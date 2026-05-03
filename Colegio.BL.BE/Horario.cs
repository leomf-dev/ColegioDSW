using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class HorarioBE
    {
        public int IdHorario { get; set; }
        public string Dia { get; set; } = string.Empty; // Lunes, Martes, etc.
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int IdSeccion { get; set; }

        // Propiedad de navegación
        public string? NombreSeccion { get; set; }
    }
}