using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class MatriculaDALC
    {
        private readonly string _conn;
        public MatriculaDALC(string connectionString) { _conn = connectionString; }

        public int Insert(Matricula m)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Matricula_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdAlumno", m.IdAlumno);
                cmd.Parameters.AddWithValue("@Periodo", m.Periodo);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open(); cmd.ExecuteNonQuery(); 
                return (int)p.Value;
            }
        }

        public List<Matricula> GetAll()
        {
            var list = new List<Matricula>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Matricula_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Matricula
                        {
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdAlumno = Convert.ToInt32(dr["IdAlumno"]),
                            FechaMatricula = Convert.ToDateTime(dr["FechaMatricula"]),
                            Periodo = dr["Periodo"].ToString(),
                            NombreAlumno = dr["Nombre"].ToString(),
                            ApellidoAlumno = dr["Apellido"].ToString(),
                            DNIAlumno = dr["DNI"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public Matricula GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Matricula_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Matricula
                        {
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdAlumno = Convert.ToInt32(dr["IdAlumno"]),
                            FechaMatricula = Convert.ToDateTime(dr["FechaMatricula"]),
                            Periodo = dr["Periodo"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Matricula_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", id);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }
    }
}
