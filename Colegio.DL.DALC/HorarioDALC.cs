using Colegio.BL.BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public class HorarioDALC
    {
        private readonly string _conn;
        public HorarioDALC(string connectionString) { _conn = connectionString; }

        public int Insert(HorarioBE h)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Horario_Insert", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Dia", h.Dia);
                cmd.Parameters.AddWithValue("@HoraInicio", h.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", h.HoraFin);
                cmd.Parameters.AddWithValue("@IdSeccion", h.IdSeccion);
                var p = new SqlParameter("@NewId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open();
                cmd.ExecuteNonQuery();
                return (int)p.Value;
            }
        }

        public List<HorarioBE> GetAll()
        {
            var list = new List<HorarioBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Horario_GetAll", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new HorarioBE
                        {
                            IdHorario = Convert.ToInt32(dr["IdHorario"]),
                            Dia = dr["Dia"].ToString(),
                            HoraInicio = TimeSpan.Parse(dr["HoraInicio"].ToString()),
                            HoraFin = TimeSpan.Parse(dr["HoraFin"].ToString()),
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            NombreSeccion = dr["NombreSeccion"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public HorarioBE GetById(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Horario_GetById", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdHorario", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new HorarioBE
                        {
                            IdHorario = Convert.ToInt32(dr["IdHorario"]),
                            Dia = dr["Dia"].ToString(),
                            HoraInicio = TimeSpan.Parse(dr["HoraInicio"].ToString()),
                            HoraFin = TimeSpan.Parse(dr["HoraFin"].ToString()),
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            NombreSeccion = dr["NombreSeccion"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public List<HorarioBE> GetBySeccion(int idSeccion)
        {
            var list = new List<HorarioBE>();
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Horario_GetBySeccion", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSeccion", idSeccion);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new HorarioBE
                        {
                            IdHorario = Convert.ToInt32(dr["IdHorario"]),
                            Dia = dr["Dia"].ToString(),
                            HoraInicio = TimeSpan.Parse(dr["HoraInicio"].ToString()),
                            HoraFin = TimeSpan.Parse(dr["HoraFin"].ToString()),
                            IdSeccion = Convert.ToInt32(dr["IdSeccion"]),
                            NombreSeccion = dr["NombreSeccion"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public void Update(HorarioBE h)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Horario_Update", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdHorario", h.IdHorario);
                cmd.Parameters.AddWithValue("@Dia", h.Dia);
                cmd.Parameters.AddWithValue("@HoraInicio", h.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", h.HoraFin);
                cmd.Parameters.AddWithValue("@IdSeccion", h.IdSeccion);
                cn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = Conexion.CreateConnection(_conn))
            using (var cmd = new SqlCommand("sp_Horario_Delete", cn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdHorario", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}