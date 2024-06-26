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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Security.Policy;

namespace BookShare
{
    public partial class PublishBook : Form
    {
        private int userID;
        public PublishBook(int userID)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void PublishBook_Load(object sender, EventArgs e)
        {
            string authorQuery = "SELECT AuthorName, AuthorSurname FROM Author";
            FillComboBox2(authorQuery, "AuthorName", "AuthorSurname", comboBox1);

            string publisherQuery = "SELECT PublisherName FROM Publisher";
            FillComboBox(publisherQuery, "PublisherName", comboBox2);

            string categoryQuery = "SELECT CategoryName FROM Category";
            FillComboBox(categoryQuery, "CategoryName", comboBox3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTb.Text) || string.IsNullOrWhiteSpace(descriptionTb.Text) || string.IsNullOrWhiteSpace(bookNameTb.Text) || string.IsNullOrWhiteSpace(pageCountTb.Text) || string.IsNullOrWhiteSpace(countTb.Text) || comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.");
                return;
            }

            string title = titleTb.Text;
            string description = descriptionTb.Text;
            string bookName = bookNameTb.Text;
            int pageCount = Convert.ToInt32(pageCountTb.Text);
            int count = Convert.ToInt32(countTb.Text);
            int authorID = SelectAuthor();
            int publisherID = SelectPublisher();
            int categoryID = SelectCategory();
            string productPhoto = "photoAddress";

            int bookID = AddBook(bookName, pageCount, count, authorID, publisherID, categoryID);

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO Product(UserID, BookID, ProductName, Description, ProductPhoto) Values(@userID, @bookID, @title, @description, @productPhoto)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        command.Parameters.AddWithValue("@bookID", bookID);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@productPhoto", productPhoto);

                        command.ExecuteNonQuery();

                        MessageBox.Show("İlanınız Başarıyla Eklendi.");

                        Close();
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

        private void FillComboBox(string sorgu, string columnName, System.Windows.Forms.ComboBox comboBox)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand komut = new SqlCommand(sorgu, connection))
                    {
                        using (SqlDataReader reader = komut.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value = reader[columnName].ToString();
                                comboBox.Items.Add(value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("FillComboBox, Veritabanına Bağlanamadı: " + ex.Message);
            }
        }

        private void FillComboBox2(string sorgu, string columnName1, string columnName2, System.Windows.Forms.ComboBox comboBox)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand komut = new SqlCommand(sorgu, connection))
                    {
                        using (SqlDataReader reader = komut.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value = reader[columnName1].ToString() + " " + reader[columnName2].ToString();
                                comboBox.Items.Add(value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ComboBox, Veritabanına Bağlanamadı: " + ex.Message);
            }
        }

        private int SelectAuthor()
        {
            if (comboBox1.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedAuthor = comboBox1.SelectedItem.ToString();

                        int index = selectedAuthor.IndexOf(' ');
                        string column1 = selectedAuthor.Substring(0, index);
                        string column2 = selectedAuthor.Substring(index + 1);

                        string query = "SELECT AuthorID FROM Author WHERE AuthorName=@column1 AND AuthorSurname=@column2";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@column1", column1);
                            command.Parameters.AddWithValue("@column2", column2);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                return (int)result;
                            }
                            else
                            {
                                MessageBox.Show("Bilinmeyen Hata.");
                                return 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Server'a Bağlanamadı: " + ex.Message);
                        return 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen yazar seçiniz.");
                return 0;
            }
        }

        private int SelectCategory()
        {
            if (comboBox1.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedCategory = comboBox3.SelectedItem.ToString();

                        string query = "SELECT CategoryID FROM Category WHERE CategoryName=@selectedCategory";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedCategory", selectedCategory);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                return (int)result;
                            }
                            else
                            {
                                MessageBox.Show("Bilinmeyen Hata.");
                                return 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Server'a Bağlanamadı: " + ex.Message);
                        return 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen kategori seçiniz.");
                return 0;
            }
        }

        private int SelectPublisher()
        {
            if (comboBox1.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedPublisher = comboBox2.SelectedItem.ToString();

                        string query = "SELECT PublisherID FROM Publisher WHERE PublisherName=@selectedPublisher";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedPublisher", selectedPublisher);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                return (int)result;
                            }
                            else
                            {
                                MessageBox.Show("Bilinmeyen Hata.");
                                return 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Server'a Bağlanamadı: " + ex.Message);
                        return 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen yayınevi seçiniz.");
                return 0;
            }
        }

        private int AddBook(string bookName, int pageCount, int count, int authorID, int publisherID, int categoryID)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO Book(BookName, PageCount, EditionNum, AuthorID, PublisherID, CategoryID) Values(@bookName, @pageCount, @count, @authorID, @publisherID, @categoryID); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookName", bookName);
                        command.Parameters.AddWithValue("@pageCount", pageCount);
                        command.Parameters.AddWithValue("@count", count);
                        command.Parameters.AddWithValue("@authorID", authorID);
                        command.Parameters.AddWithValue("@publisherID", publisherID);
                        command.Parameters.AddWithValue("@categoryID", categoryID);

                        int sonEklenenID = Convert.ToInt32(command.ExecuteScalar());
                        
                        return sonEklenenID;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server'a Bağlanamadı: " + ex.Message);
                    return 0;
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
    }
}
