using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions;
using HY.Web.Entity;
using HY.Web.Service;

namespace HY.Web.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var entity = new DeployEntity();
            entity.AppId = 100;
            entity.DeployCode = "100";
            entity.DeployPackage = "100";
            entity.DeployContent = "100";
            entity.UploadUserId = 100;
            entity.UploadTime = DateTime.Now;
            entity.DeployType = 100;

            var service = new DeployService();
            //插入
            service.Insert(entity);

            //查询所有
            var allList = service.GetAll<DeployEntity>();

            //多条件查询
            var pgMain = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

            var pga = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pga.Predicates.Add(Predicates.Field<DeployEntity>(f => f.DeployCode, Operator.Eq, "100"));
            pga.Predicates.Add(Predicates.Field<DeployEntity>(f => f.ID, Operator.Ge, 47));
            pga.Predicates.Add(Predicates.Field<DeployEntity>(f => f.ID, Operator.Le, 48));
            pgMain.Predicates.Add(pga);

            var pgb = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pgb.Predicates.Add(Predicates.Field<DeployEntity>(f => f.DeployCode, Operator.Eq, "10000"));
            pgMain.Predicates.Add(pgb);

            var specialList = service.GetList<DeployEntity>(pgMain).ToList();

            //分页查询
            long allRowsCount = 0;
            var pageList = service.GetPageList<DeployEntity>(1, 2, out allRowsCount);





        }
    }
}
