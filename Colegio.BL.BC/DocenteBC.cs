using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class DocenteBC
    {
        private readonly DocenteDALC _dalc;
        public DocenteBC(string connectionString)
        {
            _dalc = new DocenteDALC(connectionString);
        }

        public int Crear(DocenteBE d) => _dalc.Insert(d);
        public List<DocenteBE> Listar() => _dalc.GetAll();
        public DocenteBE Consultar(int id) => _dalc.GetById(id);
        public void Actualizar(DocenteBE d) => _dalc.Update(d);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}