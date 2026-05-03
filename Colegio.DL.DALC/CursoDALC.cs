using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class CursoDALC
    {
        private readonly string _conn;
        public CursoDALC(string connectionString) { _conn = connectionString; }

        public int Insert(Curso c)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Curso_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", c.Codigo);
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Creditos", c.Creditos);
                cmd.Parameters.AddWithValue("@HorasSemanales", c.HorasSemanales);
                cmd.Parameters.AddWithValue("@Estado", c.Estado);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open();
                cmd.ExecuteNonQuery();
                return (int)p.Value;
            }
        }

        public List<Curso> GetAll()
        {
            var list = new List<Curso>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Curso_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Curso
                        {
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            Codigo = dr["Codigo"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Creditos = Convert.ToInt32(dr["Creditos"]),
                            HorasSemanales = Convert.ToInt32(dr["HorasSemanales"]),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public Curso GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Curso_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCurso", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Curso
                        {
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            Codigo = dr["Codigo"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Creditos = Convert.ToInt32(dr["Creditos"]),
                            HorasSemanales = Convert.ToInt32(dr["HorasSemanales"]),
                            Estado = dr["Estado"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Curso c)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Curso_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCurso", c.IdCurso);
                cmd.Parameters.AddWithValue("@Codigo", c.Codigo);
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Creditos", c.Creditos);
                cmd.Parameters.AddWithValue("@HorasSemanales", c.HorasSemanales);
                cmd.Parameters.AddWithValue("@Estado", c.Estado);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Curso_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCurso", id);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }
    }
}
