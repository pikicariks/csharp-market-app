using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Projektni_zadatak
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static String konekcioniString = "Server=localhost; Port=3306; Database=flam; Uid=root; Pwd=1234";
        public static string korisnickiID;

        private void Prijava()
        {
            errorProvider1.Clear();
            String korisnickoIme = textBox1.Text;
            String sifra = textBox2.Text;
            String kveri = "SELECT ime, kupac_id, pass FROM kupac WHERE user = '" + korisnickoIme + "'";


            try
            {
                MySqlConnection konekcija = new MySqlConnection(konekcioniString);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();

                if (!reader.HasRows)
                {
                    errorProvider1.SetError(textBox1, "Pogrešno korisničko ime.");
                }
                else
                {
                   string ime = reader[0].ToString();
                   korisnickiID = reader[1].ToString();
                   string pass = reader[2].ToString();
                   

                    if (sifra == pass)
                    {
                        if (korisnickiID == "1")
                        {
                            MessageBox.Show("Dobrodošli " + ime);
                            Form2 fr2 = new Form2();
                            this.Hide();
                            fr2.Show();
                            
                        }
                        else
                        {
                            MessageBox.Show("Uspješno ste se logovali " + ime);
                            Form5 fr5 = new Form5();
                            this.Hide();
                            fr5.Show();
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(textBox2, "Pogrešan password.");
                    }


                }
                reader.Close();
                konekcija.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prijava();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
