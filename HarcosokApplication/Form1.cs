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
        private List<Skill> lista2;
        private MySqlConnection conn;
        MySqlCommand sql;

        public Form1()
        {

            InitializeComponent();
            adatbazis();
            tablaLetrehozas();
            CBhasznalo_feltolt();
            LBOXharcosok_feltolt();
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
                            LBOXharcosok_feltolt();
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

        private void BTNkepesseg_Click(object sender, EventArgs e)
        {
            if (TBnev2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Nem megfelelő a név", "Rendszer Info");
            }
            else if (TBleiras.Text.Trim().Equals(""))
            {
                MessageBox.Show("Nem megfelelő a leírás", "Rendszer Info");
            }
            else if (CBhasznalo.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kiválasztva harcos", "Rendszer Info");
            }
            else
            {
                sql.CommandText = "INSERT INTO kepessegek (id, nev, leiras, harcos_id) VALUES (NULL, '"+ TBnev2.Text + "', '"+ TBleiras.Text + "', '"+ lista[CBhasznalo.SelectedIndex].Id + "');";
                sql.ExecuteNonQuery();
                MessageBox.Show(("Sikeresen felvettük a képességet"), "Adatbázis Info");
            }
        }

        private void LBOXharcosok_feltolt()
        {
            LBOXharcosok.Items.Clear();
            foreach (Hero item in lista)
            {
                LBOXharcosok.Items.Add(item + "\t" + item.Letrehozas.ToString());
            }
        }

        private void LBOXharcosok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LBOXharcosok.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kiválasztva harcos", "Rendszer Info");
            }
            else
            {
                LBOXkepessegek.Items.Clear();
                TBleiras2.Text = "";
                lista2 = new List<Skill>();
                try
                {
                    sql.CommandText = "SELECT id, nev, leiras FROM kepessegek WHERE harcos_id = '" + lista[LBOXharcosok.SelectedIndex].Id + "';";
                    using (MySqlDataReader dr = sql.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Skill temp = new Skill(dr.GetInt32("id"), dr.GetString("nev"), dr.GetString("leiras") );
                            lista2.Add(temp);
                            LBOXkepessegek.Items.Add(temp);
                        }
                    }
                }
                catch (MySqlException ex)
                {

                    MessageBox.Show(ex.Message, "Adatbázis Info");
                }
            }
        }

        private void LBOXkepessegek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LBOXkepessegek.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kiválasztva képesség", "Rendszer Info");
            }
            else
            {
                TBleiras2.Text = lista2[LBOXkepessegek.SelectedIndex].Leiras;
            }
        }

        private void BTNtöröl_Click(object sender, EventArgs e)
        {
            if (LBOXkepessegek.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kiválasztva képesség", "Rendszer Info");
            }
            else
            {
                try
                {
                    sql.CommandText = "DELETE FROM kepessegek WHERE id = '"+ lista2[LBOXkepessegek.SelectedIndex].Id + "';";
                    sql.ExecuteNonQuery();
                    MessageBox.Show(("Sikeresen eltávolítottuk a  képességet"), "Adatbázis Info");
                }
                catch (MySqlException ex)
                {

                    MessageBox.Show(ex.Message, "Adatbázis Info");
                }
            }
        }

        private void BTNmodositas_Click(object sender, EventArgs e)
        {
            if (LBOXkepessegek.SelectedIndex == -1)
            {
                MessageBox.Show("Nincs kiválasztva képesség", "Rendszer Info");
            }
            else if (TBleiras2.Text.Trim().Equals(""))
                {
                MessageBox.Show("Nem megfelelő a leírás", "Rendszer Info");
            }
            else
            {
                try
                {
                    sql.CommandText = "UPDATE kepessegek SET leiras = '" + TBleiras2.Text + "' WHERE kepessegek.id = '" + lista2[LBOXkepessegek.SelectedIndex].Id + "';";
                    sql.ExecuteNonQuery();
                    lista2[LBOXkepessegek.SelectedIndex].Leiras = TBleiras2.Text;
                    MessageBox.Show(("Sikeresen megvátoztatuk a  képességet"), "Adatbázis Info");
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Adatbázis Info");
                }
            }
        }
    }
}
