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
        private readonly NotaDALC _notaDALC;
        public MatriculaBC(string connectionString)
        {
            _dalc = new MatriculaDALC(connectionString);
            _notaDALC = new NotaDALC(connectionString);
        }

        public int Crear(Matricula m)
        {
            // Validar que se seleccionaron cursos
            if (m.CursosSeleccionados == null || !m.CursosSeleccionados.Any())
                throw new ArgumentException("Debe seleccionar al menos un curso");

            // Crear la matrícula
            int idMatricula = _dalc.Insert(m);

            // Crear notas para cada curso seleccionado (con calificación inicial 0)
            foreach (int idCurso in m.CursosSeleccionados)
            {
                var nota = new Nota
                {
                    IdMatricula = idMatricula,
                    IdCurso = idCurso,
                    Calificacion = 0 // Calificación inicial
                };
                _notaDALC.Insert(nota);
            }

            return idMatricula;
        }

        public List<Matricula> Listar() => _dalc.GetAll();

        public Matricula Consultar(int id)
        {
            var matricula = _dalc.GetById(id);
            if (matricula != null)
            {
                // Cargar las notas asociadas
                matricula.Notas = _notaDALC.GetByMatricula(id);
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

        public void RetirarCurso(int idMatricula, int idCurso)
        {
            var matricula = _dalc.GetById(idMatricula);
            if (matricula == null)
                throw new ArgumentException("Matrícula no encontrada");

            if (matricula.Estado != "Activa")
                throw new InvalidOperationException("Solo se pueden retirar cursos de matrículas activas");

            // Eliminar la nota del curso específico
            _notaDALC.DeleteByMatriculaAndCurso(idMatricula, idCurso);
        }

        public Matricula Obtener(int id) => _dalc.GetById(id);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}
