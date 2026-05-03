using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class DetalleMatriculaDALC
    {
        private readonly string _conn;
        public DetalleMatriculaDALC(string connectionString) { _conn = connectionString; }

        public int Insert(DetalleMatriculaBE dm)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", dm.IdMatricula);
                cmd.Parameters.AddWithValue("@IdSeccion", dm.IdSeccion);
                cmd.Parameters.AddWithValue("@Estado", dm.Estado);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open();
                cmd.ExecuteNonQuery();
                return (int)p.Value;
            }
        }

        public List<DetalleMatriculaBE> GetAll()
        {
            var list = new List<DetalleMatriculaBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new DetalleMatriculaBE
                        {
                            IdDetalleMatricula = Convert.ToInt32(dr["IdDetalleMatricula"]),
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            Estado = dr["Estado"].ToString(),
                            NombreCurso = dr["NombreCurso"].ToString(),
                            CodigoSeccion = dr["CodigoSeccion"].ToString(),
                            NombreDocente = dr["NombreDocente"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public DetalleMatriculaBE GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDetalleMatricula", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DetalleMatriculaBE
                        {
                            IdDetalleMatricula = Convert.ToInt32(dr["IdDetalleMatricula"]),
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            Estado = dr["Estado"].ToString(),
                            NombreCurso = dr["NombreCurso"].ToString(),
                            CodigoSeccion = dr["CodigoSeccion"].ToString(),
                            NombreDocente = dr["NombreDocente"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public List<DetalleMatriculaBE> GetByMatricula(int idMatricula)
        {
            var list = new List<DetalleMatriculaBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_GetByMatricula", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdMatricula", idMatricula);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new DetalleMatriculaBE
                        {
                            IdDetalleMatricula = Convert.ToInt32(dr["IdDetalleMatricula"]),
                            IdMatricula = Convert.ToInt32(dr["IdMatricula"]),
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            Estado = dr["Estado"].ToString(),
                            NombreCurso = dr["NombreCurso"].ToString(),
                            CodigoSeccion = dr["CodigoSeccion"].ToString(),
                            NombreDocente = dr["NombreDocente"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public void UpdateEstado(int idDetalleMatricula, string estado)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_UpdateEstado", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDetalleMatricula", idDetalleMatricula);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(DetalleMatriculaBE dm)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDetalleMatricula", dm.IdDetalleMatricula);
                cmd.Parameters.AddWithValue("@IdMatricula", dm.IdMatricula);
                cmd.Parameters.AddWithValue("@IdSeccion", dm.IdSeccion);
                cmd.Parameters.AddWithValue("@Estado", dm.Estado);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_DetalleMatricula_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDetalleMatricula", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}