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
    public partial class ProjectSearch : UserControl
    {
        public ProjectSearch()
        {
            InitializeComponent();
            LlenarDataGridView();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LlenarDataGridView()
        {
            // Crear una instancia de la clase conexion
            conexion con = new conexion();

            // Crear una nueva instancia de DataTable
            DataTable dtProyectos = new DataTable();

            using (SqlConnection connection = con.GetConnection())
            {
                string query = @"SELECT [Numero de Proyecto], [Nombre], [Estatus], [empresa] 
                         FROM [ENGITASK].[dbo].[Proyectos]";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Limpia las filas del DataGridView antes de llenarlo
                guna2DataGridView111.Rows.Clear();

                while (reader.Read())
                {
                    int rowIndex = guna2DataGridView111.Rows.Add();

                    // Llena las columnas con los datos correspondientes
                    guna2DataGridView111.Rows[rowIndex].Cells["NoProyecto"].Value = reader["Numero de Proyecto"].ToString();
                    guna2DataGridView111.Rows[rowIndex].Cells["NombreDelProyecto"].Value = reader["Nombre"].ToString();

                    // Para la columna de ComboBox (EstatusActual)
                    DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)guna2DataGridView111.Rows[rowIndex].Cells["EstatusActual"];
                    if (comboBoxCell.Items.Contains(reader["Estatus"].ToString()))
                    {
                        comboBoxCell.Value = reader["Estatus"].ToString();
                    }
                    else
                    {
                        comboBoxCell.Items.Add(reader["Estatus"].ToString());
                        comboBoxCell.Value = reader["Estatus"].ToString();
                    }

                    // Agrega opciones "Activo" e "Inactivo"
                    if (comboBoxCell.Items.Count <= 1)
                    {
                        comboBoxCell.Items.Add("Activo");
                        comboBoxCell.Items.Add("Inactivo");
                    }

                    // Para la columna de ComboBox (Empresa)
                    DataGridViewComboBoxCell comboBoxCell2 = (DataGridViewComboBoxCell)guna2DataGridView111.Rows[rowIndex].Cells["Empresa"];
                    if (comboBoxCell2.Items.Contains(reader["empresa"].ToString()))
                    {
                        comboBoxCell2.Value = reader["empresa"].ToString();
                    }
                    else
                    {
                        comboBoxCell2.Items.Add(reader["empresa"].ToString());
                        comboBoxCell2.Value = reader["empresa"].ToString();
                    }

                    // Agrega opciones
                    if (comboBoxCell2.Items.Count <= 1)
                    {
                        comboBoxCell2.Items.Add("Fluidos");
                        comboBoxCell2.Items.Add("Flutec LP");
                        comboBoxCell2.Items.Add("IDJ");
                        comboBoxCell2.Items.Add("Flutec Services");
                        comboBoxCell2.Items.Add("AMS");
                        comboBoxCell2.Items.Add("Superficies");
                        comboBoxCell2.Items.Add("Espiromex");
                    }
                }

                reader.Close();
                connection.Close();
            }
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

                // Recorre las filas del DataGridView
                foreach (DataGridViewRow row in guna2DataGridView111.Rows)
                {
                    if (row.IsNewRow) continue; // Ignora la fila nueva (vacía)

                    // Obtener los valores de las celdas
                    string numeroProyecto = row.Cells["NoProyecto"].Value.ToString();
                    string nombreProyecto = row.Cells["NombreDelProyecto"].Value.ToString();

                    // Obtener el valor seleccionado del ComboBox
                    var estatusCell = row.Cells["EstatusActual"] as DataGridViewComboBoxCell;
                    string estatus = estatusCell?.Value?.ToString();

                    // Obtener el valor seleccionado del ComboBox para la empresa
                    var empresacell = row.Cells["Empresa"] as DataGridViewComboBoxCell;
                    string empresa = empresacell?.Value?.ToString(); // Corregido aquí

                    // Verificar si se ha seleccionado un valor en el ComboBox
                    if (string.IsNullOrEmpty(estatus) || string.IsNullOrEmpty(empresa))
                    {
                        continue; // Salta esta fila si no hay un valor de estatus o empresa
                    }

                    // Comando SQL para hacer el UPDATE
                    string query = @"UPDATE [ENGITASK].[dbo].[Proyectos]
                             SET [Nombre] = @Nombre, 
                                 [Estatus] = @Estatus
                             WHERE [Numero de Proyecto] = @NumeroProyecto AND [empresa] = @Empresa";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar los parámetros a la consulta
                        command.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                        command.Parameters.AddWithValue("@Nombre", nombreProyecto);
                        command.Parameters.AddWithValue("@Estatus", estatus);
                        command.Parameters.AddWithValue("@Empresa", empresa);

                        // Ejecutar el comando
                        command.ExecuteNonQuery();
                    }
                }

                // La conexión se cerrará automáticamente al salir del bloque using
            }

            // Mensaje de confirmación
            MessageBox.Show("Datos actualizados correctamente en la base de datos.");
        }

   

        private void FiltrarDataGridView()
        {
            // Obtén el texto ingresado en el TextBox
            string filtro = guna2TextBox1.Text.ToLower(); // Asegúrate de usar la comparación en minúsculas

            // Recorre las filas del DataGridView
            foreach (DataGridViewRow row in guna2DataGridView111.Rows)
            {
                if (row.IsNewRow) continue; // Ignora la fila nueva (vacía)

                // Obtén los valores de las columnas que deseas filtrar
                string numeroProyecto = row.Cells["NoProyecto"].Value.ToString().ToLower();
                string nombreProyecto = row.Cells["NombreDelProyecto"].Value.ToString().ToLower();

                // Comprueba si el filtro se encuentra en el número de proyecto o en el nombre del proyecto
                bool match = numeroProyecto.Contains(filtro) || nombreProyecto.Contains(filtro);

                // Muestra o oculta la fila según el resultado del filtro
                row.Visible = match;
            }
        }

        private void guna2TextBox1_TextChanged_1(object sender, EventArgs e)
        {
            FiltrarDataGridView();
        }
    }
}
