using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BE
{
    public class DetalleMatriculaBE
    {
        public int IdDetalleMatricula { get; set; }
        public int IdMatricula { get; set; }
        public int IdSeccion { get; set; }
        public string Estado { get; set; } = "Activo";

        // Propiedades de navegación
        public string? NombreCurso { get; set; }
        public string? CodigoSeccion { get; set; }
        public string? NombreDocente { get; set; }
        public List<HorarioBE> Horarios { get; set; } = new List<HorarioBE>();
    }
}