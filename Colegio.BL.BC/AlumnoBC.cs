using Colegio.BL.BE;
using Colegio.DL.DALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.BL.BC
{
    public class AlumnoBC
    {
        private readonly AlumnoDALC _dalc;
        public AlumnoBC(string connectionString)
        {
            _dalc = new AlumnoDALC(connectionString);
        }

        public int CrearAlumno(AlumnoBE a)
        {
            // Validaciones basicas
            if (string.IsNullOrWhiteSpace(a.Nombre) || string.IsNullOrWhiteSpace(a.Apellido))
                throw new System.ArgumentException("Nombre y Apellido son obligatorios");

            return _dalc.Insert(a);
        }

        public List<AlumnoBE> ListarAlumnos() => _dalc.GetAll();
        public AlumnoBE Obtener(int id) => _dalc.GetById(id);
        public void Actualizar(AlumnoBE a) => _dalc.Update(a);
        public void Eliminar(int id) => _dalc.Delete(id);
    }
}
