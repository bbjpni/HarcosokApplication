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
        private MySqlConnection conn;
        MySqlCommand sql;
        public Form1()
        {

            InitializeComponent();
            adatbazis();
            tablaLetrehozas();
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
                sql.CommandText = "CREATE TABLE IF NOT EXISTS cs_harcosok.harcosok ( id INT NOT NULL AUTO_INCREMENT , nev TINYTEXT NOT NULL , letrehozas DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP , PRIMARY KEY (id));";
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
    }
}
