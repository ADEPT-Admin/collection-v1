using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ACTCore.CollectionService.Assignment
{
    public class DbHelper : IDisposable
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public DbHelper(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = new SqlCommand(sql, _connection, _transaction))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = new SqlCommand(sql, _connection, _transaction))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(sql, _connection, _transaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection); // Reader จะปิด connection ให้เมื่ออ่านเสร็จ
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Dispose();
        }
    }
}
