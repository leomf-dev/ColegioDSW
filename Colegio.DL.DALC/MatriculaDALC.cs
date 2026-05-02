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
                cmd.Parameters.AddWithValue("@Estado", m.Estado);
                cmd.Parameters.AddWithValue("@Observacion", (object)m.Observacion ?? DBNull.Value);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open(); cmd.ExecuteNonQuery(); 
                return (int)p.Value;
            }
        }

        public void UpdateEstado(int idMatricula, string estado, string observacion = null)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Matricula_UpdateEstado", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", idMatricula);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@Observacion", (object)observacion ?? DBNull.Value);
                cn.Open(); cmd.ExecuteNonQuery();
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
                            Estado = dr["Estado"].ToString(),
                            FechaEstado = dr["FechaEstado"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaEstado"]),
                            Observacion = dr["Observacion"] == DBNull.Value ? null : dr["Observacion"].ToString(),
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
                            Periodo = dr["Periodo"].ToString(),
                            Estado = dr["Estado"].ToString(),
                            FechaEstado = dr["FechaEstado"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaEstado"]),
                            Observacion = dr["Observacion"] == DBNull.Value ? null : dr["Observacion"].ToString(),
                            NombreAlumno = dr["Nombre"].ToString(),
                            ApellidoAlumno = dr["Apellido"].ToString(),
                            DNIAlumno = dr["DNI"].ToString()
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
