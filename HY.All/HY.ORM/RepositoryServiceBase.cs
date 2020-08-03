using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DapperExtensions;
using HY.DataAccess;

namespace HY.ORM
{
    public class RepositoryServiceBase : IDataServiceRepository
    {
        public RepositoryServiceBase()
        {
        }

        public RepositoryServiceBase(IDBSession dbSession)
        {
            DBSession = dbSession;
        }

        public IDBSession DBSession { get; private set; }

        /// <summary>
        ///     根据Id获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById<T>(dynamic primaryId) where T : class
        {
            return DBSession.Connection.Get<T>(primaryId as object, databaseType: DBSession.DatabaseType);
        }

        /// <summary>
        ///     根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class
        {
            string tblName = string.Format("dbo.{0}", typeof (T).Name);
            string idsin = string.Join(",", ids.ToArray());
            string sql = "SELECT * FROM @table WHERE Id in (@ids)";
            IEnumerable<T> dataList = DBSession.Connection.Query<T>(sql, new {table = tblName, ids = idsin});
            return dataList;
        }
        

        /// <summary>
        ///     获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            return DBSession.Connection.GetList<T>(databaseType: DBSession.DatabaseType);
        }

        /// <summary>
        ///     统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public int Count<T>(object predicate, bool buffered = false) where T : class
        {
            return DBSession.Connection.Count<T>(predicate, databaseType: DBSession.DatabaseType);
        }

        /// <summary>
        ///     查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null,
            bool buffered = false) where T : class
        {
            return DBSession.Connection.GetList<T>(predicate, sort, null, null, buffered, DBSession.DatabaseType);
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out long allRowsCount,
            object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class
        {
            if (sort == null)
            {
                sort = new List<ISort>();
            }
            IEnumerable<T> entityList = DBSession.Connection.GetPage<T>(predicate, sort, pageIndex, pageSize, null, null,
                buffered, DBSession.DatabaseType);
            allRowsCount = DBSession.Connection.Count<T>(predicate, databaseType: DBSession.DatabaseType);
            return entityList;
        }

        /// <summary>
        ///     插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public dynamic Insert<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            dynamic result = DBSession.Connection.Insert(entity, transaction, databaseType: DBSession.DatabaseType);
            return result;
        }

        /// <summary>
        ///     更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool Update<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            bool isOk = DBSession.Connection.Update(entity, transaction, databaseType: DBSession.DatabaseType);
            return isOk;
        }

        /// <summary>
        ///     删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Delete<T>(dynamic primaryId, IDbTransaction transaction = null) where T : class
        {
            var entity = GetById<T>(primaryId);
            var obj = entity as T;
            int isOk = DBSession.Connection.Delete(obj, databaseType: DBSession.DatabaseType);
            return isOk;
        }

        /// <summary>
        ///     删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int DeleteList<T>(object predicate = null, IDbTransaction transaction = null) where T : class
        {
            return DBSession.Connection.Delete<T>(predicate, transaction, databaseType: DBSession.DatabaseType);
        }

        /// <summary>
        ///     批量插入功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="transaction"></param>
        public bool InsertBatch<T>(IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                Insert(item, transaction);
            }
            isOk = true;
            return isOk;
        }

        /// <summary>
        ///     批量更新（）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool UpdateBatch<T>(IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                Update(item, transaction);
            }
            isOk = true;
            return isOk;
        }

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool DeleteBatch<T>(IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class
        {
            bool isOk = false;
            foreach (dynamic id in ids)
            {
                Delete<T>(id, transaction);
            }
            isOk = true;
            return isOk;
        }

        public void SetDBSession(IDBSession dbSession)
        {
            DBSession = dbSession;
        }
    }
}