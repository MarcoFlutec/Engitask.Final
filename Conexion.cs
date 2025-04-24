using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.ApplicationServices;


namespace Engitask
{
    public class conexion
    {
        private readonly SqlConnection con;

        public conexion()
        {
            // Reemplaza los datos según tu entorno si es necesario
            con = new SqlConnection("Data Source=192.168.30.184;Initial Catalog=Engitask;User ID=Admin;Password=Root123;TrustServerCertificate=True");
        }

        public SqlConnection GetConnection()
        {
            if (con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir la conexión: " + ex.Message);
                }
            }
            return con;
        }

        public void CloseConnection()
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}

