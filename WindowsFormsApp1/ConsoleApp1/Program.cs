using System;
using System.Data;
using System.Linq;
using Dapper;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataConnectUtils connection = new DataConnectUtils("dapperbase");




            connection.Test("Insert into Users values (@UserName, @Email, @Address)", new { UserName = "www", Email = "123456@qq.com", Address = "上海" });

            ////连接数据库

            ////插入数据connection.Execute()执行sql插入语句
            //var result = connection.Execute("Insert into Users values (@UserName, @Email, @Address)", new { UserName = "www", Email = "123456@qq.com", Address = "上海" });

            //// 插入的Users对象列表
            //var usersList = Enumerable.Range(0, 10).Select(i => new Users()
            //{
            //    Email = i + "qq.com",
            //    Address = "贵阳",
            //    UserName = i + "chao"
            //});


            ////返回插入数据条数。
            //var results = connection.Execute("Insert into Users values (@UserName, @Email, @Address)", usersList);



            ////2. Query操作，返回一个Users对象
            //var query = connection.Query<Users>("select * from Users where UserName=@UserName", new { UserName = "chao" });

            ////无参数查询，返回列表，带参数查询和之前的参数赋值法相同。
            //string query2 = "SELECT * FROM Users";
            //var ts = connection.Query<Users>(query2).ToList();

            ////返回单条信息
            //string query3 = "SELECT * FROM Users WHERE UserID = @UserID";
            //var book = connection.Query<Users>(query3, new { UserID = 3 }).SingleOrDefault();


            ////3.update操作，还是使用Execute方法来实现，和insert操作一样。
            //var _update = connection.Execute("update Users set UserName='王超' where UserName=@UserName", new { UserName = "www" });


            ////4.采用参数化的形式来删除用户记录
            //var delete = connection.Execute("delete  from Users where UserName=@UserName", new { UserName = "王超" });

            Console.WriteLine("Hello World!");
        }
    }
}