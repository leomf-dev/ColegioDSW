using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class MatriculaBC
    {
        private readonly MatriculaDALC _dalc;
        private readonly DetalleMatriculaDALC _detalleDALC;
        public MatriculaBC(string connectionString)
        {
            _dalc = new MatriculaDALC(connectionString);
            _detalleDALC = new DetalleMatriculaDALC(connectionString);
        }

        public int Crear(Matricula m)
        {
            // Validar que se seleccionaron secciones
            if (m.SeccionesSeleccionadas == null || !m.SeccionesSeleccionadas.Any())
                throw new ArgumentException("Debe seleccionar al menos una sección");

            // Crear la matrícula
            int idMatricula = _dalc.Insert(m);

            // Crear detalles de matrícula para cada sección seleccionada
            foreach (int idSeccion in m.SeccionesSeleccionadas)
            {
                var detalle = new DetalleMatriculaBE
                {
                    IdMatricula = idMatricula,
                    IdSeccion = idSeccion,
                    Estado = "Activo"
                };
                _detalleDALC.Insert(detalle);
            }

            return idMatricula;
        }

        public List<Matricula> Listar() => _dalc.GetAll();

        public Matricula Consultar(int id)
        {
            var matricula = _dalc.GetById(id);
            if (matricula != null)
            {
                // Cargar los detalles de matrícula
                matricula.DetallesMatricula = _detalleDALC.GetByMatricula(id);
            }
            return matricula;
        }

        public void AnularMatricula(int idMatricula, string motivo = null)
        {
            _dalc.UpdateEstado(idMatricula, "Anulada", motivo);
        }

        public void RetirarMatricula(int idMatricula, string motivo = null)
        {
            _dalc.UpdateEstado(idMatricula, "Retirada", motivo);
        }

        public void CancelarMatricula(int idMatricula, string motivo = null)
        {
            _dalc.UpdateEstado(idMatricula, "Cancelada", motivo);
        }

        public void RetirarSeccion(int idMatricula, int idSeccion)
        {
            var matricula = _dalc.GetById(idMatricula);
            if (matricula == null)
                throw new ArgumentException("Matrícula no encontrada");

            if (matricula.Estado != "Activa")
                throw new InvalidOperationException("Solo se pueden retirar secciones de matrículas activas");

            // Buscar el detalle de matrícula correspondiente
            var detalle = _detalleDALC.GetByMatricula(idMatricula)
                .FirstOrDefault(d => d.IdSeccion == idSeccion);

            if (detalle == null)
                throw new ArgumentException("La sección no está matriculada");

            if (detalle.Estado != "Activo")
                throw new InvalidOperationException("La sección ya está retirada");

            // Actualizar el estado del detalle
            _detalleDALC.UpdateEstado(detalle.IdDetalleMatricula, "Retirado");
        }

        public Matricula Obtener(int id) => _dalc.GetById(id);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}
