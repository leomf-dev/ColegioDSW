using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class HorarioBC
    {
        private readonly HorarioDALC _dalc;
        public HorarioBC(string connectionString)
        {
            _dalc = new HorarioDALC(connectionString);
        }

        public int Crear(HorarioBE h) => _dalc.Insert(h);
        public List<HorarioBE> Listar() => _dalc.GetAll();
        public HorarioBE Consultar(int id) => _dalc.GetById(id);
        public List<HorarioBE> ListarPorSeccion(int idSeccion) => _dalc.GetBySeccion(idSeccion);
        public void Actualizar(HorarioBE h) => _dalc.Update(h);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}