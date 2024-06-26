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
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BookShare
{
    public partial class MyProfile : Form
    {
        private int userID;
        private string _userName, _email, _tc;
        public MyProfile(int user)
        {
            InitializeComponent();
            this.userID = user;
        }

        private void MyProfile_Load(object sender, EventArgs e)
        {
            string ilSorgu = "Select ProvinceName From Province";
            FillComboBox(ilSorgu, "ProvinceName", comboBox1);

            string ulkeSorgu = "Select CountryName From Country";
            FillComboBox(ulkeSorgu, "CountryName", comboBox3);

            FillProfile();

            _userName = usernameTextBox.Text;
            _email = emailTextBox.Text;
            _tc = tcTextBox.Text;

        }

        private void FillProfile()
        {
            int userId = userID;

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM [User] WHERE UserID=@userId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nameTextBox.Text = reader["Name"].ToString();
                                surnameTextBox.Text = reader["Surname"].ToString();
                                usernameTextBox.Text = reader["UserName"].ToString();
                                emailTextBox.Text = reader["Email"].ToString();
                                passwordTextBox.Text = reader["Password"].ToString();
                                phoneTextBox.Text = reader["Phone"].ToString();
                                dateTimePicker1.Value = Convert.ToDateTime(reader["Birthdate"]);
                                tcTextBox.Text = reader["TC"].ToString();
                                comboBox4.SelectedItem = reader["Gender"].ToString();
                                comboBox1.SelectedItem = reader["ProvinceID"].ToString();
                                comboBox2.SelectedItem = reader["DistrictID"].ToString();
                                comboBox3.SelectedItem = reader["CountryID"].ToString();
                                postalCodeTB.Text = reader["PostalCode"].ToString();
                                addressTextBox.Text = reader["Address"].ToString();

                            }
                        }
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int userId = userID;

            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(surnameTextBox.Text) || string.IsNullOrWhiteSpace(usernameTextBox.Text) || string.IsNullOrWhiteSpace(emailTextBox.Text) || string.IsNullOrWhiteSpace(passwordTextBox.Text) || string.IsNullOrWhiteSpace(phoneTextBox.Text))
            {
                MessageBox.Show("Lütfen zorunlu alanları doldurunuz.");
                return;
            }

            string username = usernameTextBox.Text;
            if (usernameTextBox.Text.Length > 25)
            {
                MessageBox.Show("Kullanıcı Adı 25 karakterden uzun olamaz!");
                return;
            }
            if (usernameTextBox.Text != _userName)
            {
                if (!IsValueAvailable("UserName", username))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten mevcut. Lütfen farklı bir kullanıcı adı seçin.");
                    return;
                }
            }  

            string email = emailTextBox.Text;
            if (emailTextBox.Text.Length > 100)
            {
                MessageBox.Show("Email adresiniz 100 karakterden uzun olamaz!");
                return;
            }
            if (emailTextBox.Text != _email)
            {
                if (!IsValueAvailable("Email", email))
                {
                    MessageBox.Show("Bu e-mail adresi kullanılıyor. Lütfen farklı bir email adresi yazınız.");
                    return;
                }
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Geçersiz email adresi!");
                    return;
                }
            }
            
            string password = passwordTextBox.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Şifre alanı boş bırakılamaz.");
                return;
            }
            string name = nameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("İsim alanı boş bırakılamaz.");
                return;
            }
            string surname = surnameTextBox.Text;
            if (string.IsNullOrEmpty(surname))
            {
                MessageBox.Show("Soyad alanı boş bırakılamaz.");
                return;
            }

            string tcKimlik = tcTextBox.Text;
            if (string.IsNullOrEmpty(tcKimlik))
            {
                MessageBox.Show("TC alanı boş bırakılamaz.");
                return;
            }
            if (tcTextBox.Text != _tc)
            {
                if (!IsValueAvailable("TC", tcKimlik))
                {
                    MessageBox.Show("Bu TC kimlik numarası sistemde kayıtlıdır. Lütfen farklı bir TC kimlik numarası yazınız.");
                    return;
                }
                if (!IsValidTC(tcKimlik))
                {
                    MessageBox.Show("Geçersiz TC kimlik numarası!");
                    return;
                }
            }

            string phone = phoneTextBox.Text;
            if (phoneTextBox.Text.Length > 11)
            {
                MessageBox.Show("Telefon numarası 11 karakterden uzun olamaz!");
                phoneTextBox.Text = phoneTextBox.Text.Substring(0, 11);
                return;
            }

            DateTime birthdate = dateTimePicker1.Value;
            if (dateTimePicker1.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Lütfen bir tarih seçiniz.");
                return;
            }

            string photo = pictureBox1.ImageLocation != null ? pictureBox1.ImageLocation : @"photoAddress";

            int il = SelectProvince();
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen ilinizi seçiniz.");
                return;
            }

            int ilce = SelectDistrict();
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen ilçenizi seçiniz.");
                return;
            }

            string address = addressTextBox.Text;
            if (addressTextBox.Text.Length > 100)
            {
                MessageBox.Show("Adresiniz 100 karakterden uzun olamaz!");
                return;
            }

            int? postalCode = 0;
            try
            {
                postalCode = Convert.ToInt32(postalCodeTB.Text);
            }
            catch
            {
                MessageBox.Show("Posta kodu alanı boş bırakılamaz.");
                return;
            }

            int countryID = SelectCountry();
            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen ülkenizi seçiniz.");
                return;
            }

            int gender = comboBox4.SelectedIndex == 0 ? 0 : 1;
            if (comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen cinsiyetinizi seçiniz.");
                return;
            }

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE [User] SET UserName=@username, Email=@email, Password=@password, Name=@name, Surname=@surname, TC=@tcKimlik, Phone=@phone, Birthdate=@birthdate, Photo=@photo, ProvinceID=@il, DistrictID=@ilce, Address=@address, PostalCode=@postalCode, CountryID=@countryID WHERE UserID=@userId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@surname", surname);
                        command.Parameters.AddWithValue("@tcKimlik", tcKimlik);
                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@birthdate", birthdate);
                        command.Parameters.AddWithValue("@photo", photo);
                        command.Parameters.AddWithValue("@il", il);
                        command.Parameters.AddWithValue("@ilce", ilce);
                        command.Parameters.AddWithValue("@address", address);
                        command.Parameters.AddWithValue("@postalCode", postalCode);
                        command.Parameters.AddWithValue("@countryID", countryID);
                        command.Parameters.AddWithValue("@gender", gender);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Bilgileriniz Başarıyla Güncellendi.");
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
                                if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                                {
                                    string value = reader[columnName].ToString();
                                    comboBox.Items.Add(value);
                                }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedProvince = comboBox1.SelectedItem.ToString();

                        string query = "SELECT ProvinceID FROM Province WHERE ProvinceName=@selectedProvince";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedProvince", selectedProvince);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                result = (int)result;

                                string ilceSorgu = "SELECT DistrictName FROM District WHERE ProvinceID=@result";

                                using (SqlCommand command2 = new SqlCommand(ilceSorgu, connection))
                                {
                                    command2.Parameters.AddWithValue("@result", result);

                                    object ilceResult = command2.ExecuteScalar();

                                    comboBox2.Items.Clear();

                                    using (SqlDataReader reader = command2.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            if (!reader.IsDBNull(reader.GetOrdinal("DistrictName")))
                                            {
                                                string value = reader["DistrictName"].ToString();
                                                comboBox2.Items.Add(value);
                                            }
                                        }
                                        comboBox2.SelectedIndex = 0;
                                    }

                                }

                            }
                            else
                            {
                                MessageBox.Show("Hata");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Server'a Bağlanamadı: " + ex.Message);

                    }
                }
            }
            else
            {
                MessageBox.Show("Başka şehir seçiniz.");
            }
        }

        private bool IsValueAvailable(string columnName, string value)
        {
            bool isAvailable = false;

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT COUNT(*) FROM [User] WHERE {columnName} = @value";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        int count = (int)command.ExecuteScalar();
                        isAvailable = count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }

            return isAvailable;
        }
        private bool IsValidTC(string tcKimlik)
        {
            Regex regex = new Regex(@"^\d{11}$");

            return regex.IsMatch(tcKimlik);
        }

        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return regex.IsMatch(email);
        }
        private int SelectProvince()
        {
            if (comboBox1.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedProvince = comboBox1.SelectedItem.ToString();

                        string query = "SELECT ProvinceID FROM Province WHERE ProvinceName=@selectedProvince";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedProvince", selectedProvince);

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
                MessageBox.Show("Lütfen il seçiniz.");
                return 0;
            }
        }

        private int SelectDistrict()
        {
            if (comboBox2.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedDistrict = comboBox2.SelectedItem.ToString();

                        string query = "SELECT DistrictID FROM District WHERE DistrictName=@selectedDistrict";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedDistrict", selectedDistrict);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                return (int)result;
                            }
                            else
                            {
                                MessageBox.Show("Hata");
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
                MessageBox.Show("Lütfen ilçe seçiniz.");
                return 0;
            }
        }
        private int SelectCountry()
        {
            if (comboBox3.SelectedItem != null)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
                {
                    try
                    {
                        connection.Open();
                        string selectedCountry = comboBox3.SelectedItem.ToString();

                        string query = "SELECT CountryID FROM Country WHERE CountryName=@selectedCountry";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@selectedCountry", selectedCountry);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                return (int)result;
                            }
                            else
                            {
                                MessageBox.Show("Lütfen ülke seçiniz.");
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
                MessageBox.Show("Lütfen ülke seçiniz.");
                return 0;
            }
        }
    }  
}
