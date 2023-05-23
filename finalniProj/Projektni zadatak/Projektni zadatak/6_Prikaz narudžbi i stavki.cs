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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        public string kon = Login.konekcioniString;
        public string korisnickiID = Login.korisnickiID;

        private void PrikazNarudzbi()
        {
            try
            {
                string kveri = "SELECT narudzbenica_id as 'ID narudžbe', datum_narudzbe as 'Datum narudžbe' FROM narudzbenica WHERE kupac_id = " + korisnickiID ;

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

        private void PrikazStavkeNarudzbe() {
            try
            {
                string kveri = "SELECT a.naziv_artikla AS 'Naziv artikla', sn.kolicina AS 'Kolicina', a.cijena * sn.kolicina as 'Ukupna cijena' FROM stavka_narudzbenice sn, artikal a WHERE a.artikal_id = sn.artikal_id AND sn.narudzbenica_id = " + textBoxIdNarudzbe.Text ;

                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(kveri, konekcija);

                DataTable tabela = new DataTable();

                dataAdapter.Fill(tabela);
                dataGridView2.DataSource = tabela;
                dataAdapter.Dispose();
                konekcija.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Total()
        {
            String kveri = "SELECT a.cijena, sn.kolicina FROM stavka_narudzbenice sn, artikal a, narudzbenica n WHERE n.narudzbenica_id=sn.narudzbenica_id AND sn.artikal_id=a.artikal_id AND " +
                "sn.narudzbenica_id='" + textBoxIdNarudzbe.Text + "'";
            double total = 0;

            try
            {
                MySqlConnection konekcija = new MySqlConnection(kon);
                konekcija.Open();
                
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    double cijena = Convert.ToDouble(reader[0]);
                    double kolicina = Convert.ToDouble(reader[1]);
                    
                    double racun = cijena * kolicina;
                    total += racun;
                }

                reader.Close();
                konekcija.Close();
                
                textBoxTotal.Text = total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void prikazIBrisanjeNarudžbiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 fr5 = new Form5();
            this.Hide();
            fr5.Show();
        }

        private void prikazNarudžbiIStavkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Na odabranom ste prozoru.");
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            PrikazNarudzbi();
        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrikazStavkeNarudzbe();
            Total();
        }
        
    }
}
