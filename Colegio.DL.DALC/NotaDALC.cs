using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class NotaDALC
    {
        private readonly string _conn;
        public NotaDALC(string connectionString) { _conn = connectionString; }

        public int Insert(Nota n)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", n.IdMatricula);
                cmd.Parameters.AddWithValue("@IdCurso", n.IdCurso);
                cmd.Parameters.AddWithValue("@Calificacion", n.Calificacion);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open(); cmd.ExecuteNonQuery(); return (int)p.Value;
            }
        }

        public List<Nota> GetAll()
        {
            var list = new List<Nota>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Nota
                        {
                            IdNota = Convert.ToInt32(dr["IdNota"]),
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            Calificacion = Convert.ToDecimal(dr["Calificacion"]),
                            CursoNombre = dr["CursoNombre"] == DBNull.Value ? null : dr["CursoNombre"].ToString(),
                            IdAlumno = dr["IdAlumno"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdAlumno"])
                        });
                    }
                }
            }
            return list;
        }

        public Nota GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdNota", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Nota
                        {
                            IdNota = Convert.ToInt32(dr["IdNota"]),
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            Calificacion = Convert.ToDecimal(dr["Calificacion"])
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Nota n)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdNota", n.IdNota);
                cmd.Parameters.AddWithValue("@Calificacion", n.Calificacion);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdNota", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Nota> GetByMatricula(int idMatricula)
        {
            var list = new List<Nota>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_GetByMatricula", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", idMatricula);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Nota
                        {
                            IdNota = Convert.ToInt32(dr["IdNota"]),
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            Calificacion = Convert.ToDecimal(dr["Calificacion"]),
                            CursoNombre = dr["CursoNombre"] == DBNull.Value ? null : dr["CursoNombre"].ToString(),
                            IdAlumno = dr["IdAlumno"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdAlumno"])
                        });
                    }
                }
            }
            return list;
        }

        public void DeleteByMatriculaAndCurso(int idMatricula, int idCurso)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Nota_DeleteByMatriculaAndCurso", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", idMatricula);
                cmd.Parameters.AddWithValue("@IdCurso", idCurso);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
