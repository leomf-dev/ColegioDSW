using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class DetalleMatriculaBC
    {
        private readonly DetalleMatriculaDALC _dalc;
        public DetalleMatriculaBC(string connectionString)
        {
            _dalc = new DetalleMatriculaDALC(connectionString);
        }

        public int Crear(DetalleMatriculaBE dm) => _dalc.Insert(dm);
        public List<DetalleMatriculaBE> Listar() => _dalc.GetAll();
        public DetalleMatriculaBE Consultar(int id) => _dalc.GetById(id);
        public List<DetalleMatriculaBE> ListarPorMatricula(int idMatricula) => _dalc.GetByMatricula(idMatricula);
        public void Actualizar(DetalleMatriculaBE dm) => _dalc.Update(dm);
        public void ActualizarEstado(int id, string estado) => _dalc.UpdateEstado(id, estado);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}