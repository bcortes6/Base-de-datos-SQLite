using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasedeDatosSQLite
{
    public partial class Form1 : Form
    {
        //agregamos la base de datos
        string path = "data_table.db";
        string cs = @"URI=file:" + Application.StartupPath + "\\data_table.db";
        //la base de datos se crea en la carpeta DEBUG

        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader dr;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var conn = new SQLiteConnection(cs);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            try
            {
                cmd.CommandText = "INSERT INTO text(name,id) VALUES(@name,@id)";

                string NAME = txtNombre.Text;
                string ID = txtID.Text;
                cmd.Parameters.AddWithValue("@name", NAME);
                cmd.Parameters.AddWithValue("@id", ID);

                dataGridView1.ColumnCount = 2;
                dataGridView1.Columns[0].Name = "Nombre";
                dataGridView1.Columns[1].Name = "Id";
                string[] row = new string[] { NAME, ID };
                dataGridView1.Rows.Add(row);
                cmd.ExecuteNonQuery();

            }
            catch(Exception)
            {
                Console.WriteLine("No se pudo insertar la imformación");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var conn = new SQLiteConnection(cs);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            try
            {
                cmd.CommandText = "UPDATE test set id=@Id where name = @Name";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                cmd.Parameters.AddWithValue("@ID", txtID.Text);

                cmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                data_show();

            }
            catch (Exception)
            {
                Console.WriteLine("No se pudo actualizar la información");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var conn = new SQLiteConnection(cs);
            conn.Open();
            var cmd = new SQLiteCommand(conn);
            try
            {
                cmd.CommandText = "DELATE FROM test WHERE name = @Name";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@Name", txtNombre.Text);

                cmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                data_show();

            }
            catch (Exception)
            {
                Console.WriteLine("No se pudo eliminar la información");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Create_db();
            data_show();
        }

        private void Create_db()
        {
            if(!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source="+path))
                {
                    sqlite.Open();
                    string sql = "create table test(name varchar(20), id varchar(12))";
                    SQLiteCommand command = new SQLiteCommand(sql,sqlite);
                    command.ExecuteNonQuery();
                }
                   
            }
            else
            {
                Console.WriteLine("La base de datos no se puede crear");
            }
        }
        private void data_show()
        {
            var conn = new SQLiteConnection(cs);
            conn.Open();

            string stm = "SELECT * FROM test";
            var cmd = new SQLiteCommand(stm, conn);
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                dataGridView1.Rows.Insert(0, dr.GetString(0), dr.GetString(1));
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
