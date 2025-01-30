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
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.ApplicationServices;


namespace Engitask
{
    public class conexion
    {
        private SqlConnection con;

        public SqlConnection Connection { get; internal set; }

        public conexion()
        {
            try
            {

                con = new SqlConnection("Data Source=192.168.30.184;Initial Catalog=Engitask;User ID=Admin;Password=Root123;TrustServerCertificate=True");

                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public SqlConnection GetConnection()
        {
            return con;
        }

        public void CloseConnection()
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        internal void open()
        {
            throw new NotImplementedException();
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }
    }
}

