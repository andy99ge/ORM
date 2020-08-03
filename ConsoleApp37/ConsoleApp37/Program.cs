using Microsoft.EntityFrameworkCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleApp37
{
    class Program
    {

        static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
               .UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.1.245;Initial Catalog=freesqlTest;User Id=sa;Password=p@ssw0rd;")
               // .UseConnectionString(FreeSql.DataType.MySql, "Data Source=127.0.0.1;Port=3306;User ID=root;Password=root;Initial Catalog=cccddd;Charset=utf8;SslMode=none;Max pool size=20")
               //.UseConnectionString(FreeSql.DataType.PostgreSQL, "Host=192.168.164.10;Port=5432;Username=postgres;Password=123456;Database=tedb;Pooling=true;Maximum Pool Size=20")
               .UseAutoSyncStructure(false)
               .UseNoneCommandParameter(true)
               //.UseConfigEntityFromDbFirst(true)
               .Build();

        static SqlSugarClient sugar
        {
            get => new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Data Source=192.168.1.245;Initial Catalog=freesqlTest;User Id=sa;Password=p@ssw0rd;",
                DbType = DbType.SqlServer,
                //ConnectionString = "Data Source=127.0.0.1;Port=3306;User ID=root;Password=root;Initial Catalog=cccddd;Charset=utf8;SslMode=none;Min Pool Size=20;Max Pool Size=20",
                //DbType = DbType.MySql,
                //ConnectionString = "Host=192.168.164.10;Port=5432;Username=postgres;Password=123456;Database=tedb;Pooling=true;Maximum Pool Size=21",
                //DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        class SongContext : DbContext
        {
            public DbSet<Song> Songs { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Data Source=192.168.1.245;Initial Catalog=freesqlTest;User Id=sa;Password=p@ssw0rd;");
                //optionsBuilder.UseMySql("Data Source=127.0.0.1;Port=3306;User ID=root;Password=root;Initial Catalog=cccddd;Charset=utf8;SslMode=none;Min Pool Size=21;Max Pool Size=21");
                //optionsBuilder.UseNpgsql("Host=192.168.164.10;Port=5432;Username=postgres;Password=123456;Database=tedb;Pooling=true;Maximum Pool Size=21");
            }
        }


        static StringBuilder sb = new StringBuilder();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            fsql.CodeFirst.SyncStructure(typeof(Song), "freesql_song");
            fsql.CodeFirst.SyncStructure(typeof(Song), "sugar_song");
            fsql.CodeFirst.SyncStructure(typeof(Song), "efcore_song");

            fsql.CodeFirst.SyncStructure(typeof(Song_tag), "freesql_song_tag");
            fsql.CodeFirst.SyncStructure(typeof(Song_tag), "sugar_song_tag");
            fsql.CodeFirst.SyncStructure(typeof(Song_tag), "efcore_song_tag");

            fsql.CodeFirst.SyncStructure(typeof(Tag), "freesql_tag");
            fsql.CodeFirst.SyncStructure(typeof(Tag), "sugar_tag");
            fsql.CodeFirst.SyncStructure(typeof(Tag), "efcore_tag");

            var sb = new StringBuilder();
            var time = new Stopwatch();

            var sql222 = fsql.Select<Song>().Where(a => DateTime.Now.Subtract(a.create_time.Value).TotalHours > 0).ToSql();



            var testlist1 = fsql.Select<Song>().OrderBy(a => a.id).ToList();
            var testlist2 = new List<Song>();
            fsql.Select<Song>().OrderBy(a => a.id).ToChunk(2, fetch =>
            {
                testlist2.AddRange(fetch.Object);
            });

            //sugar.Aop.OnLogExecuted = (s, e) =>
            //{
            //    Trace.WriteLine(s);
            //};
            //测试前清空数据
            fsql.Delete<Song>().Where(a => a.id > 0).ExecuteAffrows();
            sugar.Deleteable<Song>().Where(a => a.id > 0).ExecuteCommand();
            fsql.Ado.ExecuteNonQuery("delete from efcore_song");

            Console.WriteLine("插入性能：");
            Insert(sb, 100, 1);
            Console.Write(sb.ToString());
            sb.Clear();
            Insert(sb, 100, 10);
            Console.Write(sb.ToString());
            sb.Clear();

            Insert(sb, 1, 1000);
            Console.Write(sb.ToString());
            sb.Clear();
            Insert(sb, 1, 10000);
            Console.Write(sb.ToString());
            sb.Clear();
            Insert(sb, 1, 50000);
            Console.Write(sb.ToString());
            sb.Clear();
            Insert(sb, 1, 100000);
            Console.Write(sb.ToString());
            sb.Clear();

            Console.WriteLine("查询性能：");
            Select(sb, 100, 1);
            Console.Write(sb.ToString());
            sb.Clear();
            Select(sb, 100, 10);
            Console.Write(sb.ToString());
            sb.Clear();

            Select(sb, 1, 1000);
            Console.Write(sb.ToString());
            sb.Clear();
            Select(sb, 1, 10000);
            Console.Write(sb.ToString());
            sb.Clear();
            Select(sb, 1, 50000);
            Console.Write(sb.ToString());
            sb.Clear();
            Select(sb, 1, 100000);
            Console.Write(sb.ToString());
            sb.Clear();

            Console.WriteLine("更新：");
            Update(sb, 100, 1);
            Console.Write(sb.ToString());
            sb.Clear();
            Update(sb, 100, 10);
            Console.Write(sb.ToString());
            sb.Clear();

            Update(sb, 1, 1000);
            Console.Write(sb.ToString());
            sb.Clear();
            Update(sb, 1, 10000);
            Console.Write(sb.ToString());
            sb.Clear();
            Update(sb, 1, 50000);
            Console.Write(sb.ToString());
            sb.Clear();
            Update(sb, 1, 100000);
            Console.Write(sb.ToString());
            sb.Clear();

            Console.WriteLine("测试结束，按任意键退出...");
            Console.ReadKey();

        }

        static void Select(StringBuilder sb, int forTime, int size)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            for (var a = 0; a < forTime; a++)
                fsql.Select<Song>().Limit(size).ToList();
            sw.Stop();
            sb.AppendLine($"FreeSql Select {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (var a = 0; a < forTime; a++)
                sugar.Queryable<Song>().Take(size).ToList();
            sw.Stop();
            sb.AppendLine($"SqlSugar Select {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (var a = 0; a < forTime; a++)
            {
                using (var db = new SongContext())
                {
                    db.Songs.Take(size).AsNoTracking().ToList();
                }
            }
            sw.Stop();
            sb.AppendLine($"EFCore Select {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms");
        }

        static void Insert(StringBuilder sb, int forTime, int size)
        {
            var songs = Enumerable.Range(0, size).Select(a => new Song
            {
                create_time = DateTime.Now,
                is_deleted = false,
                title = $"Insert_{a}",
                url = $"Url_{a}"
            });

            //预热
            fsql.Insert(songs.First()).ExecuteAffrows();
            sugar.Insertable(songs.First()).ExecuteCommand();
            using (var db = new SongContext())
            {
                //db.Configuration.AutoDetectChangesEnabled = false;
                db.Songs.AddRange(songs.First());
                db.SaveChanges();
            }
            Stopwatch sw = new Stopwatch();

            sw.Restart();
            for (var a = 0; a < forTime; a++)
            {
                fsql.Insert(songs).ExecuteAffrows();
                //using (var db = new FreeSongContext()) {
                //	//db.Configuration.AutoDetectChangesEnabled = false;
                //	db.Songs.AddRange(songs.ToArray());
                //	db.SaveChanges();
                //}
            }
            sw.Stop();
            sb.AppendLine($"FreeSql Insert {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms");

            sw.Restart();
            Exception sugarEx = null;
            try
            {
                for (var a = 0; a < forTime; a++)
                    sugar.Insertable(songs.ToArray()).ExecuteCommand();
            }
            catch (Exception ex)
            {
                sugarEx = ex;
            }
            sw.Stop();
            sb.AppendLine($"SqlSugar Insert {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms" + (sugarEx != null ? $"成绩无效，错误：{sugarEx.Message}" : ""));

            sw.Restart();
            for (var a = 0; a < forTime; a++)
            {

                using (var db = new SongContext())
                {
                    //db.Configuration.AutoDetectChangesEnabled = false;
                    db.Songs.AddRange(songs.ToArray());
                    db.SaveChanges();
                }
            }
            sw.Stop();
            sb.AppendLine($"EFCore Insert {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms\r\n");
        }

        static void Update(StringBuilder sb, int forTime, int size)
        {
            Stopwatch sw = new Stopwatch();

            var songs = fsql.Select<Song>().Limit(size).ToList();
            sw.Restart();
            for (var a = 0; a < forTime; a++)
            {
                fsql.Update<Song>().SetSource(songs).ExecuteAffrows();
            }
            sw.Stop();
            sb.AppendLine($"FreeSql Update {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms");

            songs = sugar.Queryable<Song>().Take(size).ToList();
            sw.Restart();
            Exception sugarEx = null;
            try
            {
                for (var a = 0; a < forTime; a++)
                    sugar.Updateable(songs).ExecuteCommand();
            }
            catch (Exception ex)
            {
                sugarEx = ex;
            }
            sw.Stop();
            sb.AppendLine($"SqlSugar Update {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms" + (sugarEx != null ? $"成绩无效，错误：{sugarEx.Message}" : ""));

            using (var db = new SongContext())
            {
                songs = db.Songs.Take(size).AsNoTracking().ToList();
            }
            sw.Restart();
            for (var a = 0; a < forTime; a++)
            {

                using (var db = new SongContext())
                {
                    //db.Configuration.AutoDetectChangesEnabled = false;
                    db.Songs.UpdateRange(songs.ToArray());
                    db.SaveChanges();
                }
            }
            sw.Stop();
            sb.AppendLine($"EFCore Update {size}条数据，循环{forTime}次，耗时{sw.ElapsedMilliseconds}ms\r\n");
        }

    }
}