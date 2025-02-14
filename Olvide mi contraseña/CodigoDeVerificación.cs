using Engitask.DataLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace Engitask.Olvide_mi_contraseña
{
    public partial class CodigoDeVerificación : Form
    {
        public CodigoDeVerificación()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            int codigo = int.Parse(guna2TextBox1.Text);
            // Crear la conexión
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();
            //Verificar si esta lleno el textbox
            if (string.IsNullOrEmpty(guna2TextBox1.Text))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT Correo FROM Usuarios WHERE [Codigo] = @codigo";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        ResetPassword Rp = new ResetPassword();
                        Rp.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error en conexión por favor intenta de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.CloseConnection();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();
            form1.Show();
            this.Hide();  // Ocultar el Form actual
        }
    }
}  
