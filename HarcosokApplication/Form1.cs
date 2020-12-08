using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HarcosokApplication
{
    public partial class Form1 : Form
    {
        private List<Hero> lista;
        private MySqlConnection conn;
        MySqlCommand sql;
        public Form1()
        {

            InitializeComponent();
            adatbazis();
            tablaLetrehozas();
            CBhasznalo_feltolt();
        }

        private void adatbazis() {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Database = "cs_harcosok";
            sb.CharacterSet = "utf8";
            conn = new MySqlConnection(sb.ToString());
            try
            {
                conn.Open();
                sql = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
            MessageBox.Show("Kapcsolat létrejött.", "Adatbázis Info");
        }

        private void tablaLetrehozas()
        {
            try
            {
                sql.CommandText = "CREATE TABLE IF NOT EXISTS cs_harcosok.harcosok ( id INT NOT NULL AUTO_INCREMENT , nev TINYTEXT NOT NULL , letrehozas DATE NOT NULL DEFAULT CURRENT_TIMESTAMP , PRIMARY KEY (id));";
                sql.ExecuteNonQuery();
                sql.CommandText = "CREATE TABLE IF NOT EXISTS cs_harcosok.kepessegek ( id INT NOT NULL AUTO_INCREMENT , nev TINYTEXT NOT NULL , leiras TEXT NOT NULL , harcos_id INT NOT NULL , PRIMARY KEY (id));;";
                sql.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message, "Adatbázis Info");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            MessageBox.Show("Kapcsolat felbomlott.", "Adatbázis Info");
        }

        private void BTNharcosLetrehozas_Click(object sender, EventArgs e)
        {
            if (!TBharcosNev.Text.Trim().Equals(""))
            {
                try
                {
                    sql.CommandText = "SELECT COUNT(*) AS szam FROM harcosok WHERE nev LIKE '" + TBharcosNev.Text +"'; ";
                    int i = 0;
                    using (MySqlDataReader dr = sql.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            i = dr.GetInt32("szam");
                        }
                    }
                    if (i == 0)
                    {
                        try
                        {
                            sql.CommandText = "INSERT INTO harcosok (id, nev, letrehozas) VALUES (NULL, '" + TBharcosNev.Text + "', current_timestamp());";
                            sql.ExecuteNonQuery();
                            CBhasznalo_feltolt();
                            MessageBox.Show(("Sikeresen felvettük "+ TBharcosNev.Text + " nevű harcost"), "Adatbázis Info");
                        }
                        catch (MySqlException ex)
                        {

                            MessageBox.Show(ex.Message, "Adatbázis Info");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Már létezik " + TBharcosNev.Text + " nevű harcos!", "Rendszer Info");
                        return;
                    }
                }
                catch (MySqlException ex)
                {

                    MessageBox.Show(ex.Message, "Adatbázis Info");
                }
            }
            else
            {
                MessageBox.Show("A név nem megfelelő", "Rendszer Info");
            }
        }

        private void CBhasznalo_feltolt()
        {
            lista = new List<Hero>();
            CBhasznalo.Items.Clear();
            try
            {
                sql.CommandText = "SELECT nev, id, letrehozas FROM harcosok WHERE 1;";
                using (MySqlDataReader dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Hero temp = new Hero(dr.GetString("letrehozas"), dr.GetString("nev"), dr.GetInt32("id"));
                        lista.Add(temp);
                        CBhasznalo.Items.Add(temp);
                    }
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message, "Adatbázis Info");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Rendszer Info");
            }
        }
    }
}
