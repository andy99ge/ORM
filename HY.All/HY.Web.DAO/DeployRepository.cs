//
//Created: 2016-04-17 13:18:13
//Author: 代码生成
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HY.Web.Entity;
using HY.ORM;
using HY.DataAccess;
using Dapper;
using DapperExtensions;

namespace HY.Web.DAO
{
    /// <summary>
    /// Deploy：数据访问对象
    /// </summary>
    public class DeployRepository : BaseRepository, IDisposable
    {
        public static readonly DeployRepository Instance = new DeployRepository();

        private DeployRepository()
        {
        }

        public DeployRepository(IDBSession dbSession)
            : base(dbSession)
        {
        }
    }
}
