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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        public string kon = Login.konekcioniString;
        public string korisnickiID = Login.korisnickiID;
        public string zadnjiID = "t";
        public int maxArtikalID;
        public int kolicinaSkladiste;
        public int kolicinaKorpa;

        private void PregledArtikala()
        {
            try
            {
                string kveri = "SELECT a.artikal_id AS 'ID artikla', s.naziv_artikla AS 'Naziv artikla', s.kolicina as 'Kolicina', a.cijena as 'Cijena u KM' FROM skladiste s, artikal a WHERE a.artikal_id = s.artikal_id";

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

        private void PrikazKorpe() {
            try
            {
                string kveri = "SELECT sn.artikal_id AS 'ID artikla', a.naziv_artikla as 'Naziv artikla', sn.kolicina as 'Kolicina', a.cijena * sn.kolicina as 'Ukupna cijena' FROM stavka_narudzbenice sn, artikal a WHERE sn.narudzbenica_id = "+ zadnjiID +" AND sn.artikal_id = a.artikal_id ";

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

        private void KreirajNarudzbu()
        {
            try
            {
                //Uzimam ID od zadnje narudzbenice da bih ga mogao u sljedećem koraku iskoristiti
                try
                {
                    string kvera = "SELECT narudzbenica_id FROM narudzbenica WHERE narudzbenica_id = narudzbenica_id ORDER BY narudzbenica_id Desc ";
                    MySqlConnection konekcija = new MySqlConnection(kon);

                    konekcija.Open();
                    MySqlCommand cmd = new MySqlCommand(kvera, konekcija);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Read();
                    zadnjiID = (Convert.ToInt32(reader[0]) + 1).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //Insertujem u bazu novi narudzbenicaID klikom na Kreiraj

                string kver = "INSERT INTO narudzbenica VALUES (" + zadnjiID + ", " + korisnickiID + ", '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";

                MySqlConnection konekcijaDva = new MySqlConnection(kon);
                konekcijaDva.Open();

                MySqlCommand cmdDva = new MySqlCommand(kver, konekcijaDva);
                cmdDva.ExecuteNonQuery();
                konekcijaDva.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KorpaDodaj()
        {
            if (Convert.ToInt32(textBoxID.Text) > NepostojeciArtikalID())
            {
                MessageBox.Show("Artikal sa upisanim ID ne postoji.");
            }
            else if (provjeriKolicinuSkladiste() < Convert.ToInt32(textBoxKolicina.Text)){
                MessageBox.Show("Količina koju ste unijeli nije dostupna u skladištu.");
            }
            else {
                string kveriSkladiste = "UPDATE skladiste SET kolicina = kolicina - " + textBoxKolicina.Text + " WHERE artikal_id = " + textBoxID.Text + "";
                string kveriKorpa = "";

                if (ProvjeriArtikal() == 0)
                {
                    kveriKorpa = "INSERT INTO stavka_narudzbenice (narudzbenica_id, artikal_id, kolicina) VALUES (" + zadnjiID + ", " + textBoxID.Text + ", " + textBoxKolicina.Text + ")";
                }
                else
                {
                    kveriKorpa = "UPDATE stavka_narudzbenice SET kolicina = kolicina + " + textBoxKolicina.Text + " WHERE artikal_id = " + textBoxID.Text + "";
                }

                MySqlConnection konekcija = new MySqlConnection(kon);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(kveriSkladiste, konekcija);
                MySqlCommand cmdDva = new MySqlCommand(kveriKorpa, konekcija);

                cmd.ExecuteNonQuery();
                cmdDva.ExecuteNonQuery();

                konekcija.Close();
            }
        }

        private void KorpaObrisi()
        {
            if (provjerKolicinuKorpa() < Convert.ToInt32(textBoxKolicina.Text)) {
                MessageBox.Show("Broj artikala koji želite obrisati je veća nego količina koja se nalazi u korpi.");
            }
           
            else
            {
                string kveriSkladiste = "UPDATE skladiste SET kolicina = kolicina + " + textBoxKolicina.Text + " WHERE artikal_id = " + textBoxID.Text + "";
                string kveriKorpa = "UPDATE stavka_narudzbenice SET kolicina = kolicina - " + textBoxKolicina.Text + " WHERE artikal_id = " + textBoxID.Text + "";

                MySqlConnection konekcija = new MySqlConnection(kon);
                konekcija.Open();

                MySqlCommand cmd = new MySqlCommand(kveriSkladiste, konekcija);
                MySqlCommand cmdDva = new MySqlCommand(kveriKorpa, konekcija);

                cmd.ExecuteNonQuery();
                cmdDva.ExecuteNonQuery();

                konekcija.Close();
            }
        }

        private int ProvjeriArtikal()
        {
            int rez = 0;
            try
            {
                string kvera = "SELECT artikal_id FROM stavka_narudzbenice WHERE narudzbenica_id = " + zadnjiID + " AND artikal_id = "+ textBoxID.Text +"";
                MySqlConnection konekcija = new MySqlConnection(kon);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kvera, konekcija);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows) rez = Convert.ToInt32(reader[0]);
                
                konekcija.Close();
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (rez == 0) return 0;
            else return rez;
        }

        private int NepostojeciArtikalID() 
        {
            try
                {
                    string kvera = "SELECT artikal_id FROM artikal WHERE artikal_id = artikal_id ORDER BY artikal_id Desc ";
                    MySqlConnection konekcija = new MySqlConnection(kon);

                    konekcija.Open();
                    MySqlCommand cmd = new MySqlCommand(kvera, konekcija);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Read();
                    maxArtikalID = Convert.ToInt32(reader[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            return maxArtikalID;
        }

        private int provjeriKolicinuSkladiste() { 
            try
                {
                    string kvera = "SELECT kolicina FROM skladiste WHERE kolicina = kolicina";
                    MySqlConnection konekcija = new MySqlConnection(kon);

                    konekcija.Open();
                    MySqlCommand cmd = new MySqlCommand(kvera, konekcija);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Read();
                    kolicinaSkladiste = Convert.ToInt32(reader[0]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            return kolicinaSkladiste;
        }

        private int provjerKolicinuKorpa() {
            try
            {
                string kvera = "SELECT kolicina FROM stavka_narudzbenice WHERE narudzbenica_id = "+ zadnjiID +" AND artikal_id = "+ textBoxID.Text +"";
                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kvera, konekcija);

                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                kolicinaKorpa = Convert.ToInt32(reader[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return kolicinaKorpa;   
        }
        

        private void Form5_Load(object sender, EventArgs e)
        {
            PregledArtikala();
        }

        private void buttonDodaj_Click(object sender, EventArgs e)
        {
            KorpaDodaj();
            PrikazKorpe();
            PregledArtikala();   
        }

        private void buttonObrisi_Click(object sender, EventArgs e)
        {
            KorpaObrisi();
            PrikazKorpe();
            PregledArtikala();
        }

        private void prikazIBrisanjeNarudžbiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Na odabranom ste prozoru.");
        }

        private void prikazNarudžbiIStavkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 fr6 = new Form6();
            this.Hide();
            fr6.Show();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonKreiraj_Click(object sender, EventArgs e)
        {
            KreirajNarudzbu();
            PrikazKorpe();
        }


        
    }
}
