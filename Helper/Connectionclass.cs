using System;
using System.Data;
using System.Data.SqlClient;

namespace IQMania.Helper
{
    public class Connectionclass
    {
        private SqlConnection? _connection;
        private string? constr { get; set; }
        public IConfiguration configuration;


        public Connectionclass(IConfiguration _configuration)
        {
            configuration = _configuration;
            Init();
        }
        private void Init()
        {
            
            constr = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(constr);
        }
      
        private void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(constr);
            }
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Open();
        }
        private void CloseConnection()
        {
            if (_connection == null)
            {
                // Nothing to close, as there is no active connection.
                return;
            }
            if (_connection.State == ConnectionState.Open)
                this._connection.Close();
        }
        

    }
}
