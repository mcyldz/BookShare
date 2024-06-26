using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net;
using System.Xml.Linq;

namespace BookShare
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string ilSorgu = "Select ProvinceName From Province";
            FillComboBox(ilSorgu, "ProvinceName", comboBox1);

            string ulkeSorgu = "Select CountryName From Country";
            FillComboBox(ulkeSorgu, "CountryName", comboBox3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
            if (!IsValueAvailable("UserName",username))
            {
                MessageBox.Show("Bu kullanıcı adı zaten mevcut. Lütfen farklı bir kullanıcı adı seçin.");
                return;
            }

            string email = emailTextBox.Text;
            if (emailTextBox.Text.Length > 100)
            {
                MessageBox.Show("Email adresiniz 100 karakterden uzun olamaz!");
                return;
            }
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

            string role = "user";

            int gender = comboBox4.SelectedIndex == 0 ? 0 : 1;
            if (comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen cinsiyetinizi seçin.");
                return;
            }

            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookShare;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO [User](UserName, Email, Password, Name, Surname, TC, Phone, Birthdate, Photo, ProvinceID, DistrictID, Address, PostalCode, CountryID, Role, Gender) Values(@username, @email, @password, @name, @surname, @tcKimlik, @phone, @birthdate, @photo, @il, @ilce, @address, @postalCode, @countryID, @role, @gender)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
                        command.Parameters.AddWithValue("@role", role);
                        command.Parameters.AddWithValue("@gender", gender);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Kayıt Oldunuz.");

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

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Lütfen bir resim dosyası seçiniz.";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Image Files(*.JPG;*.JPEG;*.PNG)|*.JPG;*.JPEG;*.PNG";
            openFileDialog1.InitialDirectory = "DosyaYolu";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.ImageLocation = openFileDialog1.FileName;
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {          
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

        private void postalCodeTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Lütfen posta koduna rakam yazınız.");
            }
        }
    }
}
