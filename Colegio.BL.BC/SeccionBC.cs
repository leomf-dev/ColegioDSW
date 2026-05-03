using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class SeccionBC
    {
        private readonly SeccionDALC _dalc;
        private readonly HorarioDALC _horarioDALC;
        public SeccionBC(string connectionString)
        {
            _dalc = new SeccionDALC(connectionString);
            _horarioDALC = new HorarioDALC(connectionString);
        }

        public int Crear(SeccionBE s) => _dalc.Insert(s);
        public List<SeccionBE> Listar() => _dalc.GetAll();
        public SeccionBE Consultar(int id)
        {
            var seccion = _dalc.GetById(id);
            if (seccion != null)
            {
                seccion.Horarios = _horarioDALC.GetBySeccion(id);
            }
            return seccion;
        }
        public void Actualizar(SeccionBE s) => _dalc.Update(s);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}