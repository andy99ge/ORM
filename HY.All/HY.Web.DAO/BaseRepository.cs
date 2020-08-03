using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HY.DataAccess;
using HY.DataAccess.SqlDBHelper;
using HY.ORM;

namespace HY.Web.DAO
{
    public abstract class BaseRepository : RepositoryBase, IDisposable
    {
        protected BaseRepository()
        {
            
            SetDBSession(Helper.CreateDBSession());
        }

        protected BaseRepository(IDBSession dbSession)
            : base(dbSession)
        {
        }

        public void Dispose()
        {
        }
    }
}