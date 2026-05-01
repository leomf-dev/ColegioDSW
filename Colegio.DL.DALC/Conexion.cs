using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colegio.DL.DALC
{
    public static class Conexion
    {
        public static SqlConnection CreateConnection(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
