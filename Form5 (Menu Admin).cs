using Engitask.User_Controls;
using Engitask.User_Controls__Admins_;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engitask
{
    public partial class Form5__Menu_Admin_ : Form
    {
        public Form5__Menu_Admin_()
        {
            InitializeComponent();
        }


        private void ShowUserControl(UserControl control)
        {
            // Limpiar el panel antes de mostrar el nuevo UserControl
            guna2GradientPanel1.Controls.Clear();  // Asegúrate de que el nombre es el correcto

            // Ajustar el control al tamaño del panel
            control.Dock = DockStyle.Fill;

            // Agregar el UserControl al panel
            guna2GradientPanel1.Controls.Add(control);  // Agrega el UserControl al panel
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Verificar si ya se ha mostrado este control previamente
            if (!(guna2GradientPanel1.Controls.Count > 0 && guna2GradientPanel1.Controls[0] is ProjectRegister))
            {
                // Crear una instancia del UserControl que quieres mostrar
                ProjectRegister PReg = new ProjectRegister();

                // Mostrar el UserControl dentro del panel
                ShowUserControl(PReg);
            }
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            // Verificar si ya se ha mostrado este control previamente
            if (!(guna2GradientPanel1.Controls.Count > 0 && guna2GradientPanel1.Controls[0] is ProjectSearch))
            {
                // Crear una instancia del UserControl que quieres mostrar
                ProjectSearch Pser = new ProjectSearch();

                // Mostrar el UserControl dentro del panel
                ShowUserControl(Pser);
            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();
            form1.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "http://192.168.30.184:8501/",
                    UseShellExecute = true // Abre la URL en el navegador predeterminado.
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el enlace: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://flutecfyt.sharepoint.com/:u:/s/EngiTask/EcvIk13U8CJPq2-mTgFFH7EBb2mhuOZt_k5jaJwhQw-weA?e=y6JWeh",
                    UseShellExecute = true // Abre la URL en el navegador predeterminado.
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el enlace: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form5__Menu_Admin__Load(object sender, EventArgs e)
        {

        }
    }
}
