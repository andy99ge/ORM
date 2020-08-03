using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dapper;
using DapperDemp;
using System.Data.SqlClient;

namespace DapperDemp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public readonly string connectionString = "Data Source=.;Initial Catalog=DapperExtensionsDemo;User Id=sa;Password=P@ssw0rd;";
        private void Form1_Load(object sender, EventArgs e)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                dataGridView1.DataSource = connection.Query<Users>("select * from users").ToList();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            List<Users> list = dataGridView1.DataSource as List<Users>;
            Users user = list[dataGridView1.SelectedRows[0].Index];
            textBox1.Text = user.UserId + "";
            textBox2.Text = user.LoginName;
            textBox3.Text = user.Password;
            textBox4.Text = user.Status + "";
            textBox5.Text = user.Remark;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.LoginName = "Test"; users.Password = "hao123"; users.Status = 1; users.Remark = "娃哈哈";
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("insert into Users(loginName,Password,Status,Remark) values(@LoginName,@Password,@Status,@Remark)", users);
                dataGridView1.DataSource = connection.Query<Users>("select * from users").ToList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Users> list = dataGridView1.DataSource as List<Users>;
            Users user = list[dataGridView1.SelectedRows[0].Index];
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("delete from users where UserId=@UserId", user);
                dataGridView1.DataSource = connection.Query<Users>("select * from users").ToList();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Users user = new Users();
            user.UserId = Convert.ToInt32(textBox1.Text);
            user.LoginName = textBox2.Text;
            user.Password = textBox3.Text;
            user.Status = Convert.ToInt32(textBox4.Text);
            user.Remark = textBox5.Text;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("update users set LoginName=@LoginName,Password=@Password,Status=@Status,Remark=@Remark where UserId=@UserId", user);
                dataGridView1.DataSource = connection.Query<Users>("select * from users").ToList();
            }
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
            }
        }
    }
}
