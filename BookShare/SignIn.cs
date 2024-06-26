using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookShare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;

            string query = "SELECT UserID FROM [User] WHERE UserName=@username AND Password=@password";

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@password", password);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int userID = Convert.ToInt32(result);
                            MessageBox.Show("Giriş Başarılı");

                            Home home = new Home(userID);
                            home.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Hatalı şifre ya da kullanıcı adı girdiniz. Lütfen tekrar deneyiniz.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı ile bağlantıda problem yaşanmaktadır: " + ex.Message);
                    }
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
