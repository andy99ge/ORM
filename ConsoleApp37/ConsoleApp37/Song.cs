
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ConsoleApp37
{
    [FreeSql.DataAnnotations.Table(Name = "freesql_song")]
    [SugarTable("sugar_song")]
    [Table("efcore_song")]
    public class Song
    {
        [FreeSql.DataAnnotations.Column(IsIdentity = true)]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime? create_time { get; set; }
        public bool? is_deleted { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        [SugarColumn(IsIgnore = true)]
        [NotMapped]
        public virtual ICollection<Tag> Tags { get; set; }
    }
    [FreeSql.DataAnnotations.Table(Name = "freesql_song_tag")]
    [SugarTable("sugar_song_tag")]
    [Table("efcore_song_tag")]
    public class Song_tag
    {
        public int song_id { get; set; }
        [SugarColumn(IsIgnore = true)]
        [NotMapped]
        public virtual Song Song { get; set; }

        public int tag_id { get; set; }
        [SugarColumn(IsIgnore = true)]
        [NotMapped]
        public virtual Tag Tag { get; set; }
    }
    [FreeSql.DataAnnotations.Table(Name = "freesql_tag")]
    [SugarTable("sugar_tag")]
    [Table("efcore_tag")]
    public class Tag
    {
        [FreeSql.DataAnnotations.Column(IsIdentity = true)]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? parent_id { get; set; }
        [SugarColumn(IsIgnore = true)]
        [NotMapped]
        public virtual Tag Parent { get; set; }

        public decimal? ddd { get; set; }
        public string name { get; set; }

        [SugarColumn(IsIgnore = true)]
        [NotMapped]
        public virtual ICollection<Song> Songs { get; set; }
        [SugarColumn(IsIgnore = true)]
        [NotMapped]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}