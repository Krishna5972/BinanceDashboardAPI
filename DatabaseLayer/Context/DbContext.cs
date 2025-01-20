using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Repository.Context
{
   
        public class DbContext : IDisposable
        {
            private readonly string _connectionString;
            private SqlConnection _connection;

            public DbContext(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
                _connection = new SqlConnection(_connectionString);
            }

            public SqlConnection GetConnection()
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }

            public void Dispose()
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection?.Dispose();
            }
        }
    }
