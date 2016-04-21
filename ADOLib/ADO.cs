using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOLib
{
    public class ADO
    {
        private readonly int connectionTimeOut;
        private readonly string connectionString;

        public ADO(string connection, int timeOut)
        {
            this.connectionString = connection;
            this.connectionTimeOut = timeOut;
        }

        public int Command(string query)
        {
            int i;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandTimeout = connectionTimeOut;
                    i = command.ExecuteNonQuery();
                }
                conn.Close();
            }
            return i;
        }

        public int CommandWithParameter<T>(string commandString, string parameterName, T t)
        {
            int i;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    command.Parameters.AddWithValue(parameterName, t);
                    i = command.ExecuteNonQuery();
                }
                conn.Close();
            }
            return i;
        }

        public DataTable StoredProcWithGuidsTableParameter(string procName, IEnumerable<Guid> guids)
        {
            DataTable parameter = new DataTable();
            parameter.Columns.Add("guid", typeof(Guid));

            DataRow dataRow;
            foreach (var g in guids)
            {
                dataRow = parameter.NewRow();
                dataRow["guid"] = g;
                parameter.Rows.Add(dataRow);
            }

            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(procName, connection)
                {
                        CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("@g", parameter);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.SelectCommand.CommandTimeout = connectionTimeOut;
                        dataAdapter.Fill(result);
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}
