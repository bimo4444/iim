using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOLib
{
    public class ADO
    {
        private readonly string connection =
            "Data Source=SRVGALDB2;Initial Catalog=GalAMM_test;Integrated Security=True";
        private readonly int timeOut = 300;

        public ADO(string connection = null)
        {
            if (connection != null)
                this.connection = connection;
        }
        public ADO(int timeOut)
        {
            this.timeOut = timeOut;
        }
        public ADO(string connection, int timeOut)
        {
            this.connection = connection;
            this.timeOut = timeOut;
        }

        public int Command(string query)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                int i;
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandTimeout = timeOut;
                    i = command.ExecuteNonQuery();
                }
                conn.Close();
                return i;
            }
        }
    }
}
