using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BookShare
{
    public partial class Home : Form
    {
        private int userID;
        public Home(int user)
        {
            InitializeComponent();
            this.userID = user;
        }
        private void Home_Load(object sender, EventArgs e)
        {
            
            string query = "SELECT P.ProductID AS İlanID, P.ProductPhoto AS Fotoğraf, P.ProductName AS [İlan Adı], P.Description AS Açıklama, P.CreateDate AS [Oluşturulma Tarihi], B.BookName AS [Kitap Adı], B.PageCount AS [Sayfa Sayısı], B.EditionNum AS [Baskı Sayısı], A.AuthorName AS [Yazar Adı], A.AuthorSurname AS [Soyadı], Pu.PublisherName AS Yayınevi, C.CategoryName AS Kategori FROM Product AS P INNER JOIN Book AS B ON P.BookID = B.BookID INNER JOIN Author AS A ON B.AuthorID = A.AuthorID INNER JOIN Publisher AS Pu ON B.PublisherID = Pu.PublisherID INNER JOIN Category AS C ON B.CategoryID = C.CategoryID";

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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

        private void myProfileButton_Click(object sender, EventArgs e)
        {
            MyProfile myProfile = new MyProfile(userID);
            myProfile.Show();
        }

        private void listBooksBtn_Click(object sender, EventArgs e)
        {
            string query = "SELECT P.ProductID AS İlanID, P.ProductPhoto AS Fotoğraf, P.ProductName AS [İlan Adı], P.Description AS Açıklama, P.CreateDate AS [Oluşturulma Tarihi], B.BookName AS [Kitap Adı], B.PageCount AS [Sayfa Sayısı], B.EditionNum AS [Baskı Sayısı], A.AuthorName AS [Yazar Adı], A.AuthorSurname AS [Soyadı], Pu.PublisherName AS Yayınevi, C.CategoryName AS Kategori FROM Product AS P INNER JOIN Book AS B ON P.BookID = B.BookID INNER JOIN Author AS A ON B.AuthorID = A.AuthorID INNER JOIN Publisher AS Pu ON B.PublisherID = Pu.PublisherID INNER JOIN Category AS C ON B.CategoryID = C.CategoryID";

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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

        private void publishABookBtn_Click(object sender, EventArgs e)
        {
            PublishBook publishBook = new PublishBook(userID);
            publishBook.Show();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void myPublishedBooksBtn_Click(object sender, EventArgs e)
        {
            MyPublishedBooks myPublishedBooks = new MyPublishedBooks(userID);
            myPublishedBooks.Show();
        }

        private void requestBookBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;

                int productID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[0].Value);

                SendRequestWithProductID(productID);
            }
            else
            {
                MessageBox.Show("Lütfen bir kitap seçiniz.");
            }
        }

        private void SendRequestWithProductID(int productID)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO Request(ProductID, UserID) VALUES(@productID, @userID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productID", productID);
                        command.Parameters.AddWithValue("@userID", userID);
                        
                        command.ExecuteNonQuery();

                        MessageBox.Show("Kitaba başarıyla istek atıldı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server'a Bağlanamadı: " + ex.Message);
                }
                finally
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void myRequestsBtn_Click(object sender, EventArgs e)
        {
            MyRequestedBooks myRequestedBooks = new MyRequestedBooks(userID);
            myRequestedBooks.Show();
        }
    }
}
