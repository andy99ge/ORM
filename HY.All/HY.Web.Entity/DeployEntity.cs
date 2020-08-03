//
//Created: 2016-04-17 13:18:13
//Author: 代码生成
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;

namespace HY.Web.Entity
{
    /// <summary>
    /// HY.Web：实体对象
    /// </summary>
    [Serializable]
    public class DeployEntity
    {
        /// <summary>
        ///应用程序标识
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///应用程序编码
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        ///部署编号(三位项目编码+两位应用编码)
        /// </summary>
        public string DeployCode { get; set; }

        /// <summary>
        ///部署包
        /// </summary>
        public string DeployPackage { get; set; }

        /// <summary>
        ///部署内容
        /// </summary>
        public string DeployContent { get; set; }

        /// <summary>
        ///上传者ID
        /// </summary>
        public int? UploadUserId { get; set; }

        /// <summary>
        ///上传时间
        /// </summary>
        public DateTime? UploadTime { get; set; }

        /// <summary>
        ///操作类型,1:上传 2:部署预上线 3:部署线上 4:回滚预上线 5:回滚线上
        /// </summary>
        public int? DeployType { get; set; }

        /// <summary>
        ///部署用户ID
        /// </summary>
        public int? DeployUserId { get; set; }

        /// <summary>
        ///部署时间
        /// </summary>
        public DateTime? DeployTime { get; set; }

    }

    /// <summary>
    /// Deploy：实体对象映射关系
    /// </summary>
    [Serializable]
    public sealed class DeployEntityORMMapper : ClassMapper<DeployEntity>
    {
        public DeployEntityORMMapper()
        {
            base.Table("Deploy");
            
            //Map(f => f.UserID).Ignore();//设置忽略
            //Map(f => f.Name).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}

/*
/// <summary>
/// 赋值代码
/// </summary>

DeployEntity entity = new DeployEntity;
    entity.ID=?;
    entity.AppId=?;
    entity.DeployCode=?;
    entity.DeployPackage=?;
    entity.DeployContent=?;
    entity.UploadUserId=?;
    entity.UploadTime=?;
    entity.DeployType=?;
    entity.DeployUserId=?;
    entity.DeployTime=?;
*/