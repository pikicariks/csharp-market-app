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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string kon = Login.konekcioniString;
      

        private void PrikazKupaca()
        {
            try
            {
                string kveri = "SELECT kupac_id AS 'ID kupca', ime AS 'Ime', prezime AS 'Prezime', grad AS 'Grad', adresa AS 'Adresa', telefon AS 'Telefon', user as 'Username', pass as 'Šifra' FROM kupac WHERE kupac_id = kupac_id ";
               
                if (textBoxIme.Text != "" )
                {
                    kveri += " AND ime LIKE '%" + textBoxIme.Text + "%' ";
                }
             
                if (textBoxPrezime.Text != "" )
                {
                    kveri += " AND prezime LIKE '%" + textBoxPrezime.Text + "%' ";
                }
                

                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(kveri, konekcija);

                DataTable tabela = new DataTable();

                dataAdapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
                dataAdapter.Dispose();
                konekcija.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Prikaz()
        {
            try
            {
                string kveri = "SELECT  ime, prezime, grad, adresa, telefon, user, pass FROM kupac WHERE kupac_id = '"+textBox1.Text.ToString()+"' ";
                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);

                MySqlDataReader reader = cmd.ExecuteReader();
                
                reader.Read();
                if (textBox1.Text != "")
                {
                    if (reader.HasRows)
                    {
                        textBox3.Text = reader[0].ToString();
                        textBox6.Text = reader[1].ToString();
                        textBox10.Text = reader[2].ToString();
                        textBox7.Text = reader[3].ToString();
                        textBox5.Text = reader[4].ToString(); 
                        textBox4.Text = reader[5].ToString(); 
                        textBox2.Text = reader[6].ToString();
                    }
                }
                else
                {
                    textBox3.Text = "";
                    textBox6.Text = "";
                    textBox10.Text = "";
                    textBox7.Text = "";
                    textBox5.Text = "";
                    textBox4.Text = "";
                    textBox2.Text = "";
                }
                reader.Close();
                konekcija.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Kreiranje()
        {
            try
            {
                string kveri = "INSERT INTO kupac(ime, prezime, grad, adresa, telefon, user, pass) VALUES " +
                   " ('" + textBox3.Text + "', '" + textBox6.Text + "', '" + textBox10.Text + "', " +
                    " '" + textBox7.Text + "', '" + textBox5.Text + "', '" + textBox4.Text + "', '" + textBox2.Text + "') ";


                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Kreiran je novi kupac");


                konekcija.Close();
                PrikazKupaca();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void Ažuriranje()
        {
            try
            {
                string kveri = "UPDATE kupac SET " +
                      " ime='" + textBox3.Text + "', " +
                    " prezime='" + textBox6.Text + "', " +
                    " grad='" + textBox10.Text + "', " +
                    " adresa='" + textBox7.Text + "', " +
                    " telefon='" + textBox5.Text + "', " +
                    " user='" + textBox4.Text + "', " +
                    " pass='" + textBox2.Text + "' " +
                    " WHERE kupac_id='" + textBox1.Text + "' ";


                MySqlConnection konekcija = new MySqlConnection(kon);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Podaci za kupca ID=" + textBox1.Text + " su ažurirani!");

                konekcija.Close();
                PrikazKupaca();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            PrikazKupaca();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void artikliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 fr3 = new Form3();
            this.Hide();
            fr3.Show();
        }

        private void narudžbeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 fr4 = new Form4();
            this.Hide();
            fr4.Show();
        }

        private void textBoxIme_TextChanged(object sender, EventArgs e)
        {
            PrikazKupaca();
        }

        private void textBoxPrezime_TextChanged(object sender, EventArgs e)
        {
            PrikazKupaca();
        }

       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Prikaz();
        }

        private void buttonAzuriranje_Click_1(object sender, EventArgs e)
        {
            Ažuriranje();
            textBox3.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
        }

        private void buttonKreiranje_Click(object sender, EventArgs e)
        {
            Kreiranje();
            textBox3.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
        }

    }
}
