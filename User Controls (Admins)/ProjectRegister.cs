using Guna.UI2.WinForms;
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

namespace Engitask.User_Controls__Admins_
{
    public partial class ProjectRegister : UserControl
    {
        public ProjectRegister()
        {
            InitializeComponent();
            guna2DataGridView12.AllowUserToAddRows = false;  // Deshabilitar la adición automática de filas
            LlenarDataGridView();
        }

        private void guna2GradientButton12_Click(object sender, EventArgs e)
        {
            guna2DataGridView12.Rows.Clear(); // Elimina todas las filas del DataGridView
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la clase conexion
            conexion con = new conexion();

            using (SqlConnection connection = con.GetConnection())
            {
                // Solo abrir la conexión si está cerrada
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                foreach (DataGridViewRow row in guna2DataGridView12.Rows)
                {
                    if (row.IsNewRow) continue; // Ignorar la fila nueva (vacía)

                    // Asegúrate de que la celda no esté vacía antes de acceder a su valor
                    var numeroProyecto = row.Cells["NoProyecto"].Value?.ToString() ?? string.Empty;
                    var nombreDelProyecto = row.Cells["NombreDelProyecto"].Value?.ToString() ?? string.Empty;
                    var estatusActual = row.Cells["EstatusActual"].Value?.ToString() ?? string.Empty;
                    var empresa = row.Cells["Empresa"].Value?.ToString() ?? string.Empty;

                    if (string.IsNullOrEmpty(numeroProyecto) || string.IsNullOrEmpty(nombreDelProyecto))
                    {
                        MessageBox.Show("Faltan datos en algunos campos obligatorios.");
                        continue; // Salta a la siguiente fila si hay datos vacíos
                    }

                    // Comprobar si ya existe el registro
                    using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM [Proyectos] WHERE [Numero de Proyecto] = @NumeroProyecto AND [Empresa] = @Empresa", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                        checkCommand.Parameters.AddWithValue("@Empresa", empresa);

                        int exists = (int)checkCommand.ExecuteScalar();

                        if (exists > 0)
                        {
                            // Registro ya existe, manejarlo (puedes mostrar un mensaje, etc.)
                            MessageBox.Show($"El proyecto {numeroProyecto} ya existe para la empresa {empresa}.");
                            continue; // Salta a la siguiente fila
                        }
                    }

                    // Si no existe, insertar el nuevo registro
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO [Proyectos] ([Numero de Proyecto], [Nombre], [Estatus], [Empresa]) VALUES (@NumeroProyecto, @Nombre, @Estatus, @Empresa)", connection))
                    {
                        // Asegúrate de que los nombres de los parámetros coincidan con los de la consulta SQL
                        insertCommand.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                        insertCommand.Parameters.AddWithValue("@Nombre", nombreDelProyecto);
                        insertCommand.Parameters.AddWithValue("@Estatus", estatusActual);
                        insertCommand.Parameters.AddWithValue("@Empresa", empresa);

                        insertCommand.ExecuteNonQuery(); // Ejecutar la inserción
                    }
                }
            }

            MessageBox.Show("Datos insertados correctamente."); // Mensaje de éxito
        }

        private void guna2DataGridView12_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LlenarDataGridView()
        {
            // Asegúrate de que los ComboBox solo se llenen una vez, no cada vez que se recorra el DataGridView
            foreach (DataGridViewRow row in guna2DataGridView12.Rows)
            {
                // Verificar si la celda de "EstatusActual" es un ComboBox
                DataGridViewComboBoxCell comboBoxCell = row.Cells["EstatusActual"] as DataGridViewComboBoxCell;

                if (comboBoxCell != null && comboBoxCell.Items.Count == 0)
                {
                    // Agregar opciones al ComboBox si no las tiene
                    comboBoxCell.Items.Add("Activo");
                    comboBoxCell.Items.Add("Inactivo");

                    // Establecer valor predeterminado si es una nueva fila
                    if (row.IsNewRow)
                    {
                        comboBoxCell.Value = "Activo"; // Valor por defecto
                    }
                }

                // Verificar si la celda de "Empresa" es un ComboBox
                DataGridViewComboBoxCell comboBoxCell2 = row.Cells["Empresa"] as DataGridViewComboBoxCell;

                if (comboBoxCell2 != null && comboBoxCell2.Items.Count == 0)
                {
                    // Agregar opciones de empresa si no las tiene
                    comboBoxCell2.Items.Add("Fluidos");
                    comboBoxCell2.Items.Add("Flutec LP");
                    comboBoxCell2.Items.Add("IDJ");
                    comboBoxCell2.Items.Add("Flutec Services");
                    comboBoxCell2.Items.Add("AMS");
                    comboBoxCell2.Items.Add("Superficies");
                    comboBoxCell2.Items.Add("Espiromex");

                    // Establecer valor predeterminado si es una nueva fila
                    if (row.IsNewRow)
                    {
                        comboBoxCell2.Value = "Fluidos"; // Valor por defecto
                    }

                }
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Agregar una nueva fila al DataGridView
            int newRowIndex = guna2DataGridView12.Rows.Add();

            // Obtener la nueva fila
            DataGridViewRow newRow = guna2DataGridView12.Rows[newRowIndex];

            // Obtener la celda de ComboBox "EstatusActual"
            DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)newRow.Cells["EstatusActual"];

            // Agregar las opciones al ComboBox si no se han agregado previamente
            if (comboBoxCell.Items.Count == 0)
            {
                comboBoxCell.Items.Add("Activo");
                comboBoxCell.Items.Add("Inactivo");
            }

            // Establecer el valor predeterminado, solo si es un valor válido
            if (comboBoxCell.Items.Contains("Activo"))
            {
                comboBoxCell.Value = "Activo"; // Valor por defecto
            }

            // Asignar un valor predeterminado para la columna "Empresa"
            DataGridViewComboBoxCell comboBoxCell2 = (DataGridViewComboBoxCell)newRow.Cells["Empresa"];
            if (comboBoxCell2.Items.Count == 0)
            {
                comboBoxCell2.Items.Add("Fluidos");
                comboBoxCell2.Items.Add("Flutec LP");
                comboBoxCell2.Items.Add("IDJ");
                comboBoxCell2.Items.Add("Flutec Services");
                comboBoxCell2.Items.Add("AMS");
                comboBoxCell2.Items.Add("Superficies");
                comboBoxCell2.Items.Add("Espiromex");
            }
            comboBoxCell2.Value = "Fluidos";  // Valor por defecto para la columna "Empresa"
        }

        private void guna2DataGridView12_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Captura y maneja el error, por ejemplo mostrando un mensaje
            MessageBox.Show("Error en el valor de la celda: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
    
