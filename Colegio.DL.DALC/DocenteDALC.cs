using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class DocenteDALC
    {
        private readonly string _conn;
        public DocenteDALC(string connectionString) { _conn = connectionString; }

        public int Insert(DocenteBE d)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Docente_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombres", d.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", d.Apellidos);
                cmd.Parameters.AddWithValue("@Especialidad", d.Especialidad);
                cmd.Parameters.AddWithValue("@Estado", d.Estado);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open();
                cmd.ExecuteNonQuery();
                return (int)p.Value;
            }
        }

        public List<DocenteBE> GetAll()
        {
            var list = new List<DocenteBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Docente_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new DocenteBE
                        {
                            IdDocente = Convert.ToInt32(dr["IdDocente"]),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Especialidad = dr["Especialidad"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public DocenteBE GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Docente_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDocente", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DocenteBE
                        {
                            IdDocente = Convert.ToInt32(dr["IdDocente"]),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Especialidad = dr["Especialidad"].ToString(),
                            Estado = dr["Estado"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(DocenteBE d)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Docente_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDocente", d.IdDocente);
                cmd.Parameters.AddWithValue("@Nombres", d.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", d.Apellidos);
                cmd.Parameters.AddWithValue("@Especialidad", d.Especialidad);
                cmd.Parameters.AddWithValue("@Estado", d.Estado);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Docente_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdDocente", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}