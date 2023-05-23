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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public string kon = Login.konekcioniString;
        private void PrikazArtikala()
        {
            try
            {
                string kveri = "SELECT a.artikal_id, a.naziv_artikla, a.vrsta_artikla, a.cijena, s.kolicina FROM artikal a, skladiste s " +
                    " WHERE a.artikal_id=s.artikal_id ";

                if (textBox8.Text != "")
                {
                    kveri += " AND a.artikal_id = '" + textBox8.Text + "' ";
                }
                if (textBox9.Text != "")
                {
                    kveri += " AND a.naziv_artikla LIKE  '" + textBox9.Text + "%' ";
                }

                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(kveri, konekcija);

                DataTable tabela = new DataTable();

                dataAdapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
                dataAdapter.Dispose();
                konekcija.Dispose();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void DodavanjeArtikla()
        {
            try
            {
                string kveri1 = " INSERT INTO artikal (naziv_artikla,vrsta_artikla,cijena) VALUES " +
                    " ('" + textBox3.Text + "','" + textBox6.Text + "','" + textBox10.Text + "') ";
                string kveri3 = " SELECT artikal_id from artikal where naziv_artikla='" + textBox3.Text + "' ";
                string kveri2 = " INSERT INTO skladiste (kolicina,artikal_id) VALUES ( '" + textBox7.Text + "',  ";
                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd1 = new MySqlCommand(kveri1, konekcija);
                MySqlCommand cmd3 = new MySqlCommand(kveri3, konekcija);

                cmd1.ExecuteNonQuery();
                MySqlDataReader reader = cmd3.ExecuteReader();

                reader.Read();
                string id = reader[0].ToString();
                kveri2 += " '" + id + "') ";
                MySqlCommand cmd2 = new MySqlCommand(kveri2, konekcija);
                reader.Close();
                cmd2.ExecuteNonQuery();



                konekcija.Close();
                PrikazArtikala();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void AžuriranjeArtikla()
        {

            try
            {
                string kveri = "UPDATE artikal SET cijena='" + textBox10.Text + "', naziv_artikla='" + textBox3.Text + "', vrsta_artikla='" + textBox6.Text + "'  WHERE artikal_id='" + textBox5.Text + "' ";
                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);
                cmd.ExecuteNonQuery();
                konekcija.Close();
                PrikazArtikala();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PostaviKolicinu()
        {
            try
            {
                string kveri = "UPDATE skladiste,artikal SET skladiste.kolicina='" + textBox7.Text + "' WHERE  artikal.artikal_id='" + textBox5.Text + "' AND artikal.artikal_id=skladiste.artikal_id ";
                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);
                cmd.ExecuteNonQuery();
                konekcija.Close();
                PrikazArtikala();

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
                string kveri = "SELECT  a.naziv_artikla,a.vrsta_artikla,a.cijena,s.kolicina FROM artikal a, skladiste s " +
                   " WHERE a.artikal_id=s.artikal_id AND a.artikal_id='" + textBox5.Text + "' ";
                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);

                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                if (textBox5.Text != "")
                {
                    if (reader.HasRows)
                    {
                        textBox3.Text = reader[0].ToString();
                        textBox6.Text = reader[1].ToString();
                        textBox10.Text = reader[2].ToString();
                        textBox7.Text = reader[3].ToString();
                    }
                }
                else
                {
                    textBox3.Text = "";
                    textBox6.Text = "";
                    textBox10.Text = "";
                    textBox7.Text = "";
                }
                reader.Close();
                konekcija.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void DodajKolicinu()
        {
            int num = Convert.ToInt32(numericUpDown1.Value);
            try
            {
                string kveri = "SELECT kolicina from skladiste WHERE artikal_id='" + textBox5.Text + "'";

                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                string tr = reader[0].ToString();
                reader.Close();
                int ukupno = Convert.ToInt32(tr) + num;
                string kveri1 = "UPDATE skladiste" +
               " SET kolicina='" + ukupno.ToString() +
                   "' WHERE artikal_id='" + textBox5.Text + "'";
                MySqlCommand cmd1 = new MySqlCommand(kveri1, konekcija);
                cmd1.ExecuteNonQuery();
                konekcija.Close();
                PrikazArtikala();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrikazArtikala();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DodavanjeArtikla();
            textBox3.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AžuriranjeArtikla();
            textBox5.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";
        }

        
 private void Form3_Load(object sender, EventArgs e)
        {
            PrikazArtikala();
        }

 private void Form3_FormClosed(object sender, FormClosedEventArgs e)
 {
     Application.Exit();
 }

 private void kupciToolStripMenuItem_Click(object sender, EventArgs e)
 {
     Form2 fr2 = new Form2();
     this.Hide();
     fr2.Show();
 }

 private void narudžbeToolStripMenuItem_Click(object sender, EventArgs e)
 {
     Form4 fr4 = new Form4();
     this.Hide();
     fr4.Show();
 }

 private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
 {

 }

 private void button4_Click(object sender, EventArgs e)
 {
     if (textBox7.Text != "" && numericUpDown1.Value > 0)
     {
         MessageBox.Show("Provjerite unos količine");
     }
     else if (textBox7.Text != "")
     {
         PostaviKolicinu();

     }
     else DodajKolicinu();
     numericUpDown1.Value = 0;
     textBox5.Text = "";
 }

 private void textBox5_TextChanged(object sender, EventArgs e)
 {
     Prikaz();
 }

 private void textBox8_TextChanged(object sender, EventArgs e)
 {
     PrikazArtikala();
 }

 private void textBox9_TextChanged(object sender, EventArgs e)
 {
     PrikazArtikala();
 }
    }
}
