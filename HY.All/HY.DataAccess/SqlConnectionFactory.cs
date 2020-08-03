using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace HY.DataAccess
{

    public enum DatabaseType
    {    
        SqlServer,
        MySql,
        Oracle,
        DB2
    }

    public class SqlConnectionFactory
    {
        public static IDbConnection CreateSqlConnection(DatabaseType dbType, string strKey)
        {
            IDbConnection connection = null;
            string strConn = ConfigurationManager.ConnectionStrings[strKey].ConnectionString;

            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(strConn);
                    break;
                case DatabaseType.MySql:
                    //connection = new MySql.Data.MySqlClient.MySqlConnection(strConn);
                    //break;
                case DatabaseType.Oracle:
                    //connection = new Oracle.DataAccess.Client.OracleConnection(strConn);
                    connection = new System.Data.OracleClient.OracleConnection(strConn);
                    break;
                case DatabaseType.DB2:
                    connection = new System.Data.OleDb.OleDbConnection(strConn);
                    break;
            }
            return connection;
        }
    }
}
