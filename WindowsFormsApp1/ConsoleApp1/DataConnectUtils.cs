using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConsoleApp1
{
    /// <summary>
    /// v1.0
    /// </summary>
    public partial class DataConnectUtils
    {
        string ConnectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        public DataConnectUtils(string dbName)
        {
            ConnectionString = $"server=.;database={dbName};uid=sa;pwd=P@ssw0rd;";
        }

        IDbConnection OpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        public int Execute(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.Execute(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.ExecuteScalar<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.Query<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public IEnumerable<T> Query<T>(string sql, int page, int limit, object param = null, IDbTransaction transaction = null)
        {
            int offset = (page - 1) * limit;
            sql += $" limit {limit} offset {offset}";
            return Query<T>(sql, param);
        }

        public T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.QueryFirst<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.QueryFirstOrDefault<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        //public static T QueryMultiple<T>(string sql, object param = null, IDbTransaction transaction = null)
        //{
        //    using (var conn = OpenConnection())
        //    {
        //        var result = conn.QueryMultiple(sql, param, transaction);
        //        conn.Close();
        //        conn.Dispose();
        //        return result;
        //    }
        //}


        public void Test(string sql, object param = null)
        {
            using (var conn = OpenConnection())
            {
                IDbTransaction transaction = conn.BeginTransaction();

                conn.Execute(sql, param, transaction, null, null);
                //提交事务
                transaction.Commit();

            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = await conn.ExecuteAsync(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = await conn.ExecuteScalarAsync<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = await conn.QueryAsync<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, int page, int limit, object param = null, IDbTransaction transaction = null)
        {
            int offset = (page - 1) * limit;
            sql += $" limit {limit} offset {offset}";
            return await QueryAsync<T>(sql, param);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = await conn.QueryFirstAsync<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            using (var conn = OpenConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }
    }
}
