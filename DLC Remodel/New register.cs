using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engitask.DLC_Remodel
{
    public partial class New_Register : UserControl
    {
        public New_Register()
        {
            InitializeComponent();
        }

        private void ShowUserControl(UserControl userControl)
        {
            // Asegúrate de que el UserControl no esté ya agregado al panel antes de agregarlo
            if (!guna2CustomGradientPanel1.Controls.Contains(userControl))
            {
                userControl.Dock = DockStyle.Fill; // Ajusta el tamaño del control al panel
                guna2CustomGradientPanel1.Controls.Add(userControl);
            }
        }

        // Método para agregar el UserControl al panel
        private void AddUserControl(UserControl userControl)
        {
            // Configura el UserControl para que se ajuste al ancho del FlowLayoutPanel
            userControl.Width = flowLayoutPanel1.ClientSize.Width;

            // Agrega el UserControl al FlowLayoutPanel
            flowLayoutPanel1.Controls.Add(userControl);
        }


        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Verificar si ya se ha mostrado este control previamente
            if (!(guna2CustomGradientPanel1.Controls.Count > 0 && guna2CustomGradientPanel1.Controls[0] is NR))
            {
                // Crear una instancia del UserControl que quieres mostrar
                NR Nr = new NR();

                // Mostrar el UserControl dentro del panel
                ShowUserControl(Nr);
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            NR newUserControl = new NR();
            AddUserControl(newUserControl);
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
