using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class CursoBC
    {
        private readonly CursoDALC _dalc;
        public CursoBC(string connectionString) { _dalc = new CursoDALC(connectionString); }
        public int Crear(Curso c) => _dalc.Insert(c);
        public List<Curso> Listar() => _dalc.GetAll();
        public Curso Obtener(int id) => _dalc.GetById(id);
        public void Actualizar(Curso c) => _dalc.Update(c);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}
