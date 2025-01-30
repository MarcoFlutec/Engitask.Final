using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Engitask.Olvide_mi_contraseña.ForgotPassword;

namespace Engitask.Olvide_mi_contraseña
{
    public partial class ResetPassword : Form
    {
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Obtener las nuevas contraseñas desde los TextBoxes
            string NewPass = guna2TextBox1.Text;
            string ConPass = guna2TextBox2.Text;

            // Verificar que las contraseñas coincidan
            if (NewPass == ConPass)
            {
                string correo = ForgotPassword.Correo.ValorCorreo;
                // Crear la conexión
                conexion cnn = new conexion();
                SqlConnection con = cnn.GetConnection();

                // Verificar si los campos de contraseña están vacíos
                if (string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(ConPass))
                {
                    MessageBox.Show("Por favor, complete ambos campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Consulta SQL para actualizar la contraseña en la base de datos
                string updateCodeQuery = "UPDATE Usuarios SET Password = @NewPass WHERE [Correo] = @correo";

                try
                {
                    using (SqlCommand updateCmd = new SqlCommand(updateCodeQuery, con))
                    {
                        // Agregar parámetros
                        updateCmd.Parameters.AddWithValue("@NewPass", NewPass);
                        updateCmd.Parameters.AddWithValue("@correo", correo);

                        // Ejecutar la actualización
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Login Log = new Login();
                        Log.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Cerrar la conexión
                    cnn.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            guna2TextBox2.UseSystemPasswordChar = !guna2TextBox2.UseSystemPasswordChar;
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            guna2TextBox1.UseSystemPasswordChar = !guna2TextBox1.UseSystemPasswordChar;
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox2.UseSystemPasswordChar = true;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox1.UseSystemPasswordChar = true;
        }
    }
}