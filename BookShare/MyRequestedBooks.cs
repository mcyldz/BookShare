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
    public partial class MyRequestedBooks : Form
    {
        private int userID;
        public MyRequestedBooks(int user)
        {
            InitializeComponent();
            this.userID = user;
        }

        private void MyRequestedBooks_Load(object sender, EventArgs e)
        {
            string query = "SELECT Request.RequestID, Request.ProductID, Product.ProductName, Product.Description, Product.ProductPhoto FROM Request RIGHT OUTER JOIN Product ON Request.ProductID = Product.ProductID WHERE Request.UserID = @userID";

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
