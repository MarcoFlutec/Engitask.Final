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
using Engitask.Olvide_mi_contraseña;


namespace Engitask
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            conexion cnn = new conexion();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cargar credenciales si se debe recordar
            if (Properties.Settings.Default.RememberMe)
            {
                guna2TextBox1.Text = Properties.Settings.Default.UserEmail;
                guna2TextBox2.Text = Properties.Settings.Default.UserPassword;
                checkBox1.Checked = true; // Marcar el checkbox
            }
            else
            {
                // Limpiar los campos si no se debe recordar
                guna2TextBox1.Text = string.Empty;
                guna2TextBox2.Text = string.Empty;
                checkBox1.Checked = false; // Desmarcar el checkbox
            }
            guna2TextBox2.UseSystemPasswordChar = true;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c start https://apps.powerapps.com/play/e/default-7e86495a-6d6a-442e-8026-9586e600eb44/a/924326c4-74a3-4f38-8d2e-5a767051bcc3?tenantId=7e86495a-6d6a-442e-8026-9586e600eb44&hint=5de52dd9-c4bb-486b-bd06-158bb848fbd6&sourcetime=1726068845890",
                    CreateNoWindow = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el enlace: {ex.Message}");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form15__Sign_Up_ form15 = new Form15__Sign_Up_();
            form15.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            IniciarSesion();
        }


        private void IniciarSesion()
        {
            // Crear la conexión
            conexion cnn = new conexion();

            // Obtener la conexión desde la clase 'conexion'
            SqlConnection con = cnn.GetConnection();

            // Obtener los valores de los TextBox
            string correo = guna2TextBox1.Text;
            string password = guna2TextBox2.Text;

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Consulta SQL para verificar las credenciales
            string query = "SELECT Rol FROM Usuarios WHERE [Correo] = @correo AND [Password] = @password";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Agregar parámetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@password", password);

                    // Ejecutar el comando y obtener el rol del usuario
                    object result = cmd.ExecuteScalar();

                    // Si se obtuvo un resultado, el usuario existe y se autenticó correctamente
                    if (result != null)
                    {
                        string rol = result.ToString();

                        // Guardar los datos del usuario en la clase Session
                        Session.CorreoUsuario = correo;
                        Session.RolUsuario = rol;

                        // Guardar credenciales si el checkbox está marcado
                        if (checkBox1.Checked)
                        {
                            Properties.Settings.Default.UserEmail = correo;
                            Properties.Settings.Default.UserPassword = password;
                            Properties.Settings.Default.RememberMe = true;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            // Limpiar las configuraciones si no se quiere recordar
                            Properties.Settings.Default.UserEmail = string.Empty;
                            Properties.Settings.Default.UserPassword = string.Empty;
                            Properties.Settings.Default.RememberMe = false;
                            Properties.Settings.Default.Save();
                        }


                        // Verificar el rol y abrir el formulario correspondiente
                        if (rol == "Usuario")
                        {
                            // Usuario autenticado correctamente
                            Form6__User_Menu_ form6 = new Form6__User_Menu_();
                            form6.Show();
                            this.Hide();  // Ocultar el Form actual

                        }
                        else if (rol == "Admin")
                        {
                            // Administrador autenticado correctamente
                            Form5__Menu_Admin_ form5 = new Form5__Menu_Admin_();
                            form5.Show();
                            this.Hide();  // Ocultar el Form actual

                        }
                        else
                        {
                            // Rol no reconocido
                            MessageBox.Show("Rol no reconocido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Mostrar mensaje de error si las credenciales no son válidas
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Cerrar la conexión
                cnn.CloseConnection();
            }
        }


        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            // Usuario autenticado correctamente
            Form15__Sign_Up_ form15 = new Form15__Sign_Up_();
            form15.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            // Usuario autenticado correctamente
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            // Establecer la propiedad para ocultar el texto como contraseña
            guna2TextBox2.UseSystemPasswordChar = true;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            // Alternar el valor de UseSystemPasswordChar
            guna2TextBox2.UseSystemPasswordChar = !guna2TextBox2.UseSystemPasswordChar;
        }

        private void guna2TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el sonido de "ding"
                IniciarSesion();
            }
        }
    }
}
