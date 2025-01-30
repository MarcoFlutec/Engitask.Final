using Engitask.User_Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Guna.UI2.WinForms;

namespace Engitask.User_Controls__Users_
{
    public partial class TimeTracker : UserControl
    {

        // Variables globales para el cronómetro
        private System.Windows.Forms.Timer timer;
        private int currentRowIndex = -1; // Índice de la fila actual en el DataGridView
        private int elapsedTimeInSeconds;

        public TimeTracker()
        {
            InitializeComponent();

            // Inicializar el DataGridView vacío
            guna2DataGridView1.Rows.Clear(); // Limpia cualquier fila existente

            // Configuración del Timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 ms = 1 segundo
            timer.Tick += Timer_Tick;
 

        }

        private void TimeTracker_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica si se presionó la tecla "Suprimir"
            if (e.KeyCode == Keys.Delete)
            {
                // Limpiar las filas del DataGridView
                guna2DataGridView1.Rows.Clear();
            }
        }



        private void TimeTracker_Load(object sender, EventArgs e)
        {
            // Inicializar el DataGridView vacío
            guna2DataGridView1.Rows.Clear(); // Limpia cualquier fila existente

        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (currentRowIndex >= 0)
            {
                // Incrementar el tiempo transcurrido
                elapsedTimeInSeconds++;

                // Actualizar el tiempo en formato "HH:MM:SS" en la columna 5 de la fila actual
                TimeSpan time = TimeSpan.FromSeconds(elapsedTimeInSeconds);
                guna2DataGridView1.Rows[currentRowIndex].Cells[5].Value = time.ToString(@"hh\:mm\:ss");
            }

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Detener el cronómetro
            timer.Stop();
            int rowIndex = guna2DataGridView1.Rows.Add();
            guna2DataGridView1.Rows[rowIndex].Cells[0].Value = guna2TextBox2.Text;
            guna2DataGridView1.Rows[rowIndex].Cells[1].Value = guna2TextBox1.Text;
          

            // Resetear el cronómetro a ceros
            elapsedTimeInSeconds = 0;
           
        }


        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            conexion conexionBD = new conexion();
            string numeroProyecto = guna2TextBox1.Text;

            // Llama a ObtenerEstatus y ObtenerNombre
            string estatus = ObtenerEstatus(numeroProyecto);
            if (estatus == "Activo")
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells[0].Value = guna2TextBox2.Text;
                guna2DataGridView1.Rows[rowIndex].Cells[1].Value = guna2TextBox1.Text;
                guna2DataGridView1.Rows[rowIndex].Cells[2].Value = ObtenerNombre(numeroProyecto);

                // Habilitar el botón en la nueva fila
                guna2DataGridView1.Rows[rowIndex].Cells[6].Value = "PLAY"; // Establecer el estado inicial del botón a "PLAY"
            }
            else
            {
                MessageBox.Show("El proyecto no se puede agregar porque está 'Inactivo' o 'Null'.");
            }
        }

        private string ObtenerEstatus(string numeroProyecto)
        {
            string estatus = "";
            using (SqlConnection connection = new conexion().GetConnection())
            {
                string query = "SELECT [Estatus] FROM [ENGITASK].[dbo].[Proyectos] WHERE [Numero de Proyecto] = @numeroProyecto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numeroProyecto", numeroProyecto);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        estatus = reader["Estatus"].ToString();
                    }
                }
            }
            return estatus;
        }

        private string ObtenerNombre(string numeroProyecto)
        {
            string nombre = "";
            using (SqlConnection connection = new conexion().GetConnection())
            {
                string query = "SELECT [Nombre] FROM [ENGITASK].[dbo].[Proyectos] WHERE [Numero de Proyecto] = @numeroProyecto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numeroProyecto", numeroProyecto);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        nombre = reader["Nombre"].ToString();
                    }
                }
            }
            return nombre;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                DataGridViewButtonCell buttonCell = row.Cells[6] as DataGridViewButtonCell;

                // Evita la interacción si el valor es "NO DISPONIBLE"
                if (buttonCell.Value != null && buttonCell.Value.ToString() == "NO DISPONIBLE")
                {
                    return; // Ignora el clic
                }

                if (buttonCell != null)
                {
                    if (buttonCell.Value.ToString() == "PLAY")
                    {
                        buttonCell.Value = "STOP";
                        string currentTimeText = row.Cells[5].Value?.ToString() ?? "00:00:00";
                        TimeSpan currentTimeSpan = TimeSpan.Parse(currentTimeText);
                        elapsedTimeInSeconds = (int)currentTimeSpan.TotalSeconds;
                        currentRowIndex = e.RowIndex;
                        timer.Start();
                    }
                    else
                    {
                        buttonCell.Value = "PLAY";
                        timer.Stop();
                        currentRowIndex = -1;
                    }
                }
            }
        }
        
    }
           
   
}
               

        
