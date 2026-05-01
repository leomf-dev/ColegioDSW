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
        public MatriculaBC(string connectionString) { _dalc = new MatriculaDALC(connectionString); }
        public int Crear(Matricula m) => _dalc.Insert(m);
        public List<Matricula> Listar() => _dalc.GetAll();
        public Matricula Obtener(int id) => _dalc.GetById(id);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}
