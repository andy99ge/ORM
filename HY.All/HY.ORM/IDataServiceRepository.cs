﻿using System.Collections.Generic;
using System.Data;
using DapperExtensions;
using HY.DataAccess;

namespace HY.ORM
{
    public interface IDataServiceRepository
    {
        IDBSession DBSession { get; }

        T GetById<T>(dynamic primaryId) where T : class;
        IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;


        int Count<T>(object predicate, bool buffered = false) where T : class;

        //lsit
        IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = false) where T : class;

        IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out long allRowsCount, object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class;


        dynamic Insert<T>(T entity, IDbTransaction transaction = null) where T : class;
        bool InsertBatch<T>(IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class;
        bool Update<T>(T entity, IDbTransaction transaction = null) where T : class;
        bool UpdateBatch<T>(IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class;
        int Delete<T>(dynamic primaryId, IDbTransaction transaction = null) where T : class;
        int DeleteList<T>(object predicate, IDbTransaction transaction = null) where T : class;
        bool DeleteBatch<T>(IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class;
    }
}