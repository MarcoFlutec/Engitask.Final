using Engitask.DLC_Remodel;
using Engitask.User_Controls;
using Engitask.User_Controls__Users_;
using Guna.UI2.WinForms;
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
    public partial class Form6__User_Menu_ : Form
    {
        public Form6__User_Menu_()
        {
            InitializeComponent();


        }

        private void Form6__User_Menu__Load(object sender, EventArgs e)
        {
            // Crear una instancia del UserControl que quieres mostrar
            Welcome Bienvenidx = new Welcome();

            // Mostrar el UserControl dentro del panel
            ShowUserControl(Bienvenidx);
        }

        private void ShowUserControl(UserControl control)
        {
            // Limpiar el panel antes de mostrar el nuevo UserControl
            panel2.Controls.Clear();  // Asegúrate de que el nombre es el correcto

            // Ajustar el control al tamaño del panel
            control.Dock = DockStyle.Fill;

            // Agregar el UserControl al panel
            panel2.Controls.Add(control);  // Agrega el UserControl al panel
        }

        // Cuando se hace clic en un botón
        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            // Verificar si ya se ha mostrado este control previamente
            if (!(panel2.Controls.Count > 0 && panel2.Controls[0] is NewRegister))
            {
                // Crear una instancia del UserControl que quieres mostrar
                NewRegister NewRegister = new NewRegister();

                // Mostrar el UserControl dentro del panel
                ShowUserControl(NewRegister);
            }
        }

        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {

            // Verificar si ya se ha mostrado este control previamente
            if (!(panel2.Controls.Count > 0 && panel2.Controls[0] is ViewEditRecords))
            {
                // Crear una instancia del UserControl que quieres mostrar
                ViewEditRecords VER = new ViewEditRecords();

                // Mostrar el UserControl dentro del panel
                ShowUserControl(VER);
            }
        }

        private void moveImageBox(object sender)
        {
            Guna2Button b = (Guna2Button)sender;


        }

        private void guna2GradientButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            // Usuario autenticado correctamente
            Login form1 = new Login();
            form1.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void guna2GradientButton2_Click(object sender, TimeTracker e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            // Verificar si ya se ha mostrado este control previamente
            if (!(panel2.Controls.Count > 0 && panel2.Controls[0] is ViewEditRecords))
            {
                // Crear una instancia del UserControl que quieres mostrar
                TimeTracker TT = new TimeTracker();

                // Mostrar el UserControl dentro del panel
                ShowUserControl(TT);
            }
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            // Usuario autenticado correctamente
            Form1 f11 = new Form1();
            f11.Show();
            this.Hide();  // Ocultar el Form actual
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://flutecfyt.sharepoint.com/:b:/s/EngiTask/EfiJBpDlqNhKr71t5jeTUYcBWN1OE3idy5_tDVbWHxvsPQ?e=UtK4Es",
                    UseShellExecute = true // Abre la URL en el navegador predeterminado.
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el enlace: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}