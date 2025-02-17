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
using Engitask.DataLayer;
using Microsoft.Graph.Drives.Item.Items.Item.GetActivitiesByIntervalWithStartDateTimeWithEndDateTimeWithInterval;
using Engitask.DataLayer.Repositories;


namespace Engitask
{
    public partial class Login : Form
    {
        private UserRepositories _userRepo = new();
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

        //Change Name of Button
        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            IniciarSesion();
        }

        //All methods for Data layer move to a repository 
        private void IniciarSesion()
        {

            // Obtener los valores de los TextBox
            string correo = guna2TextBox1.Text;
            string password = guna2TextBox2.Text;

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //For Data types like Roles or Types or other kind of data, use numbers in Database

           var userInfo = _userRepo.GetUserInfo(correo,password);

            if (userInfo == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Guardar los datos del usuario en la clase Session
            Session.User = userInfo;
            
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

            switch (userInfo.RolUsuario)
            {
                case "Usuario":
                    Form6__User_Menu_ form6 = new Form6__User_Menu_();
                    form6.Show();
                    this.Hide();  // Ocultar el Form actual
                    break;
                case "Admin":
                    // Administrador autenticado correctamente
                    Form5__Menu_Admin_ form5 = new Form5__Menu_Admin_();
                    form5.Show();
                    this.Hide();  // Ocultar el Form actual
                    break;
                default:
                    MessageBox.Show("Rol no reconocido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
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
