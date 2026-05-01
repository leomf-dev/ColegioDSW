using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class AlumnoDALC
    {
        private readonly string _conn;

        public AlumnoDALC(string connectionString)
        {
            _conn = connectionString;
        }

        public int Insert(AlumnoBE a)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Alumno_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", a.Apellido);
                cmd.Parameters.AddWithValue("@DNI", a.DNI);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object)a.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)a.Direccion ?? DBNull.Value);

                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);

                cn.Open();
                cmd.ExecuteNonQuery();
                return (int)p.Value;
            }
        }

        public List<AlumnoBE> GetAll()
        {
            var list = new List<AlumnoBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Alumno_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new AlumnoBE
                        {
                            IdAlumno = Convert.ToInt32(dr["IdAlumno"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            DNI = dr["DNI"].ToString(),
                            FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"]),
                            Direccion = dr["Direccion"] == DBNull.Value ? null : dr["Direccion"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public AlumnoBE GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Alumno_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdAlumno", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new AlumnoBE
                        {
                            IdAlumno = Convert.ToInt32(dr["IdAlumno"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            DNI = dr["DNI"].ToString(),
                            FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"]),
                            Direccion = dr["Direccion"] == DBNull.Value ? null : dr["Direccion"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                        };
                    }
                }
            }
            return null;
        }

        public void Update(AlumnoBE a)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Alumno_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdAlumno", a.IdAlumno);
                cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", a.Apellido);
                cmd.Parameters.AddWithValue("@DNI", a.DNI);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object)a.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)a.Direccion ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Alumno_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdAlumno", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
