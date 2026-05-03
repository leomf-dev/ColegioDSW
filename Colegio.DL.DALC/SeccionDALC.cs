using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class SeccionDALC
    {
        private readonly string _conn;
        public SeccionDALC(string connectionString) { _conn = connectionString; }

        public int Insert(SeccionBE s)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Seccion_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCurso", s.IdCurso);
                cmd.Parameters.AddWithValue("@IdDocente", s.IdDocente);
                cmd.Parameters.AddWithValue("@CodigoSeccion", s.CodigoSeccion);
                cmd.Parameters.AddWithValue("@CapacidadMaxima", s.CapacidadMaxima);
                cmd.Parameters.AddWithValue("@Estado", s.Estado);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open();
                cmd.ExecuteNonQuery();
                return (int)p.Value;
            }
        }

        public List<SeccionBE> GetAll()
        {
            var list = new List<SeccionBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Seccion_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new SeccionBE
                        {
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            IdDocente = Convert.ToInt32(dr["IdDocente"]),
                            CodigoSeccion = dr["CodigoSeccion"].ToString(),
                            CapacidadMaxima = Convert.ToInt32(dr["CapacidadMaxima"]),
                            Estado = dr["Estado"].ToString(),
                            NombreCurso = dr["NombreCurso"].ToString(),
                            NombreDocente = dr["NombreDocente"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public SeccionBE GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Seccion_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSeccion", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new SeccionBE
                        {
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            IdCurso = Convert.ToInt32(dr["IdCurso"]),
                            IdDocente = Convert.ToInt32(dr["IdDocente"]),
                            CodigoSeccion = dr["CodigoSeccion"].ToString(),
                            CapacidadMaxima = Convert.ToInt32(dr["CapacidadMaxima"]),
                            Estado = dr["Estado"].ToString(),
                            NombreCurso = dr["NombreCurso"].ToString(),
                            NombreDocente = dr["NombreDocente"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(SeccionBE s)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Seccion_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSeccion", s.IdSeccion);
                cmd.Parameters.AddWithValue("@IdCurso", s.IdCurso);
                cmd.Parameters.AddWithValue("@IdDocente", s.IdDocente);
                cmd.Parameters.AddWithValue("@CodigoSeccion", s.CodigoSeccion);
                cmd.Parameters.AddWithValue("@CapacidadMaxima", s.CapacidadMaxima);
                cmd.Parameters.AddWithValue("@Estado", s.Estado);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Seccion_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSeccion", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}