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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public string kon = Login.konekcioniString;

        private void PrikazNarudzbi()
        {
            try
            {
                string kveri = "SELECT k.ime,k.prezime,n.narudzbenica_id,n.kupac_id,n.datum_narudzbe FROM kupac k, narudzbenica n" +
                    " WHERE k.kupac_id=n.kupac_id";

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
        private void Brisanje()
        {

            try
            {
                string kveri = "DELETE FROM  narudzbenica  " +
                " WHERE narudzbenica_id='" + textBox5.Text + "' ";



                MySqlConnection konekcija = new MySqlConnection(kon);

                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(kveri, konekcija);

                cmd.ExecuteNonQuery();
                konekcija.Close();

                MessageBox.Show("Uspješno obrisana narudžba.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void Form4_Load(object sender, EventArgs e)
        {
            PrikazNarudzbi();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Brisanje();
            PrikazNarudzbi();
            textBox5.Text = "";
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void kupciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fr2 = new Form2();
            this.Hide();
            fr2.Show();
        }

        private void artikliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 fr3 = new Form3();
            this.Hide();
            fr3.Show();
        }


    }
}
