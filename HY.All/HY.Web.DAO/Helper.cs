using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HY.DataAccess;

namespace HY.Web.DAO
{
    public class Helper
    {
        public static DBSession CreateDBSession(string connName="Test")
        {
            var connection = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, connName);

            return new DBSession(new Database(connection));
        }

    }
}
