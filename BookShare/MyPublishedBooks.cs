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
    public partial class MyPublishedBooks : Form
    {
        private int userID;
        public MyPublishedBooks(int user)
        {
            InitializeComponent();
            this.userID = user;
        }

        private void MyPublishedBooks_Load(object sender, EventArgs e)
        {
            string query = "SELECT ProductID, ProductPhoto, ProductName, Description, CreateDate FROM Product WHERE UserID=@userID";

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    command.ExecuteNonQuery();

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        using (DataTable dataTable = new DataTable())
                        {
                            dataAdapter.Fill(dataTable);
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
        }
    }
}
