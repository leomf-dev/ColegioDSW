using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class NotaBC
    {
        private readonly NotaDALC _dalc;
        public NotaBC(string connectionString) { _dalc = new NotaDALC(connectionString); }
        public int Crear(Nota n) => _dalc.Insert(n);
        public List<Nota> Listar() => _dalc.GetAll();
        public Nota Obtener(int id) => _dalc.GetById(id);
        public void Actualizar(Nota n) => _dalc.Update(n);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}
