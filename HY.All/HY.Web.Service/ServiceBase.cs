using System;
using System.Collections.Generic;
using HY.DataAccess;
using HY.ORM;
using HY.Web.DAO;
using HY.Web.Entity;
namespace HY.Web.Service
{
    public abstract class ServiceBase : RepositoryServiceBase, IDisposable
    {
        public IList<IDisposable> DisposableObjects { get; private set; }
        public ServiceBase()
        {
            SetDBSession(Helper.CreateDBSession());
            DisposableObjects = new List<IDisposable>();
        }
        public ServiceBase(IDBSession dbSession)
            : base(dbSession)
        {
            DisposableObjects = new List<IDisposable>();
        }
        protected void AddDisposableObject(object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (null != disposable)
            {
                DisposableObjects.Add(disposable);
            }
        }
        public void Dispose()
        {
            foreach (IDisposable obj in DisposableObjects)
            {
                if (null != obj)
                {
                    obj.Dispose();
                }
            }
        }
    }
}