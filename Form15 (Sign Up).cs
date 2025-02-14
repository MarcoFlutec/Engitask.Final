using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engitask.DataLayer;
using Microsoft.Data.SqlClient;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace Engitask
{
    public partial class Form15__Sign_Up_ : Form
    {
        public Form15__Sign_Up_()
        {
            InitializeComponent();
            conexion cnn = new conexion();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Crear la conexión
            conexion cnn = new conexion();

            // Obtener la conexión desde la clase 'conexion'
            SqlConnection con = cnn.GetConnection();

            // Obtener los valores de los TextBox
            string Correo = guna2TextBox1.Text;
            string Password = guna2TextBox2.Text;
            string Puesto = guna2TextBox3.Text;
            string Usuario = guna2TextBox4.Text;
            String Rol = "Usuario";

            if (Correo.Contains("@Flutec.com"))
            {

                // Validar que los campos no estén vacíos
                if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Puesto) || string.IsNullOrEmpty(Usuario))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Consulta SQL para insertar datos en la tabla Usuarios
                string query = "INSERT INTO Usuarios ([Correo], [Password], [Puesto], [User Name], [Rol]) VALUES (@Correo, @Password, @Puesto, @Usuario, @Rol)";

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Agregar parámetros para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@Correo", Correo);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@Puesto", Puesto);
                        cmd.Parameters.AddWithValue("@Usuario", Usuario);
                        cmd.Parameters.AddWithValue("@Rol", Rol);

                        // Ejecutar la consulta de inserción
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Si se insertaron datos correctamente, abrir el Form1
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Usuario registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Login form1 = new Login();
                            form1.Show();
                            this.Hide();  // Ocultar el Form actual
                        }
                        else
                        {
                            MessageBox.Show("No se pudo registrar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            else
            {
                MessageBox.Show("Por favor verifica tu correo", "Validación de Correo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();
            form1.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox2.UseSystemPasswordChar = true;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            // Alternar el valor de UseSystemPasswordChar
            guna2TextBox2.UseSystemPasswordChar = !guna2TextBox2.UseSystemPasswordChar;
        }
    }
}

