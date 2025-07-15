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

namespace Engitask.Recover
{
    public partial class UltimateRec : UserControl
    {
        private List<string> allProjectNumbers; // Para almacenar todos los números de proyecto
        private System.Windows.Forms.Timer searchTimer;
        public UltimateRec()
        {
            InitializeComponent();
            allProjectNumbers = new List<string>();
            searchTimer = new System.Windows.Forms.Timer { Interval = 300 }; // 300ms de retraso
            searchTimer.Tick += SearchTimer_Tick;
            // Actualiza las sumas de combox
            UpdateAllSums();
            this.Load += new EventHandler(UltimateRec_Load); // Cambiar a ViewEditRecords_Load
            // Crear la conexión
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

            try
            {
                // Usar el correo del usuario almacenado en la clase Session
                string correoUsuario = Session.CorreoUsuario;

                // Hacer la consulta SQL usando ese correo
                // Se usa la variable correoUsuario para buscar en la columna [Correo]
                string queryUsuario = "SELECT [User Name] FROM [ENGITASK].[dbo].[Usuarios] WHERE [Correo] = @correo"; // Aquí

                using (SqlCommand cmdUsuario = new SqlCommand(queryUsuario, con))
                {
                    // Agregar el parámetro con el valor de correoUsuario
                    cmdUsuario.Parameters.AddWithValue("@correo", correoUsuario); // Aquí es donde se asigna

                    // Ejecutar la consulta y obtener el resultado
                    object resultUsuario = cmdUsuario.ExecuteScalar();

                    if (resultUsuario != null)
                    {
                        // Si se encontró el resultado, mostrar el USERNAMETEXT

                        guna2TextBox2.Text = resultUsuario.ToString();
                    }
                    else
                    {
                        // Si no se encontró ningún resultado
                        MessageBox.Show("Usuario no encontrado");
                    }
                }

                // Hacer la consulta SQL usando ese correo
                // Se usa la variable correoUsuario para buscar en la columna [Correo]
                string queryJob = "SELECT [Puesto] FROM [ENGITASK].[dbo].[Usuarios] WHERE [Correo] = @correo"; // Aquí

                using (SqlCommand cmdUsuario = new SqlCommand(queryJob, con))
                {
                    // Agregar el parámetro con el valor de correoUsuario
                    cmdUsuario.Parameters.AddWithValue("@correo", correoUsuario); // Aquí es donde se asigna

                    // Ejecutar la consulta y obtener el resultado
                    object resultUsuario = cmdUsuario.ExecuteScalar();

                    if (resultUsuario != null)
                    {
                        // Si se encontró el resultado, mostrar el USERNAMETEXT

                        guna2TextBox3.Text = resultUsuario.ToString();
                    }
                    else
                    {
                        // Si no se encontró ningún resultado
                        MessageBox.Show("Usuario no encontrado");
                    }
                }


            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión
                cnn.CloseConnection();
            }
        }

        private void LoadDataForWeek(string selectedWeek)
        {
            // Crear la conexión
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

            try
            {
                // Obtener el ingeniero del TextBox
                string ingeniero = string.IsNullOrWhiteSpace(guna2TextBox2.Text) ? string.Empty : guna2TextBox2.Text;

                // Obtener el año actual
                int currentYear = DateTime.Now.Year;

                // Consulta SQL actualizada para incluir el filtro por año en base a la columna [Fecha]
                string query = @"
    SELECT [No#Proyecto], [Nombre del Proyecto], [Lunes], [Martes], [Miercoles], 
           [Jueves], [Viernes], [Sabado], [Domingo], [Total de Horas], [Comentarios]
    FROM [ENGITASK].[dbo].[Planeador]
    WHERE [Semana] = @selectedWeek
    AND YEAR([Fecha]) = @currentYear
    AND [Ingeniero] = @ingeniero 
    AND [Saved as] = 'Draft'";  // Filtrar solo por registros que estén como "Draft"

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Agregar los parámetros
                    cmd.Parameters.AddWithValue("@selectedWeek", guna2ComboBox1.Text); // Semana seleccionada
                    cmd.Parameters.AddWithValue("@currentYear", currentYear);   // Año actual
                    cmd.Parameters.AddWithValue("@ingeniero", ingeniero);       // Ingeniero

                    // Ejecutar la consulta y llenar el DataGridView
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Limpiar el DataGridView antes de llenarlo
                        guna2DataGridView1.Rows.Clear();

                        // Leer los datos
                        while (reader.Read())
                        {
                            guna2DataGridView1.Rows.Add(
                                reader["No#Proyecto"].ToString(),
                                reader["Nombre del Proyecto"].ToString(),
                                reader["Jueves"].ToString(),
                                reader["Viernes"].ToString(),
                                reader["Sabado"].ToString(),
                                reader["Domingo"].ToString(),
                                reader["Lunes"].ToString(),
                                reader["Martes"].ToString(),
                                reader["Miercoles"].ToString(),
                                reader["Total de Horas"].ToString(),
                                reader["Comentarios"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los registros: " + ex.Message);
            }
            finally
            {
                cnn.CloseConnection();
            }
        }

        private void LoadWeeks()
        {
            // Crear la conexión
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

            try
            {
                // Consulta SQL para obtener los números de semana de forma distinta y ordenados
                string query = "SELECT DISTINCT [NumeroSemana] FROM [ENGITASK].[dbo].[Calendar]";
                MessageBox.Show("Conexión establecida, ejecutando consulta..."); // Para verificar la conexión

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Ejecutar la consulta
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Crear una lista para almacenar los números de semana
                        List<int> semanas = new List<int>();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Agregar los números de semana a la lista como enteros
                                if (int.TryParse(reader["NumeroSemana"].ToString(), out int numeroSemana))
                                {
                                    semanas.Add(numeroSemana);
                                }
                            }

                            // Ordenar la lista de semanas en orden descendente
                            semanas.Sort((a, b) => a.CompareTo(b)); // Orden descendente

                            // Limpiar el ComboBox antes de llenarlo
                            guna2ComboBox1.Items.Clear();

                            // Agregar las semanas ordenadas al ComboBox
                            foreach (var semana in semanas)
                            {
                                guna2ComboBox1.Items.Add(semana.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron números de semana.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las semanas: " + ex.Message);
            }
            finally
            {
                cnn.CloseConnection();
            }
        }

        private async void UltimateRec_Load(object sender, EventArgs e)
        {
            await LoadActiveProjectsAsync(); // Cargar proyectos al iniciar
            LoadWeeks();
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener la semana seleccionada
            if (guna2ComboBox1.SelectedItem != null)
            {
                string semanaSeleccionada = guna2ComboBox1.SelectedItem.ToString();

                // Llamar al método para cargar los datos de la semana seleccionada
                LoadDataForWeek(semanaSeleccionada);  // Corregido: pasar semanaSeleccionada

                // Actualizar las sumas para las columnas
                UpdateAllSums();

                // Aquí puedes realizar cualquier acción basada en la semana seleccionada
                ObtenerPeriodoSemana();
            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {

            // Crear la conexión a la base de datos
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();
            if (con.State != ConnectionState.Open)
                con.Open();


            try
            {
                // Obtener las semanas válidas (actual y anterior) de la tabla Calendar
                string querySemanas = @"
            SELECT DISTINCT [NumeroSemana]
            FROM [ENGITASK].[dbo].[Calendar]
            WHERE DATEDIFF(WEEK, [FechaNumerica], GETDATE()) IN (0, 1)";

                List<string> semanasValidas = new List<string>();

                using (SqlCommand cmdSemanas = new SqlCommand(querySemanas, con))
                {
                    using (SqlDataReader reader = cmdSemanas.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semanasValidas.Add(reader["NumeroSemana"].ToString());
                        }
                    }
                }

                // Verificar si se encontraron semanas válidas
                if (semanasValidas.Count == 0)
                {
                    MessageBox.Show("No se encontró la semana actual ni la anterior en el calendario.");
                    return;
                }

                // Obtener el ingeniero y la semana seleccionados
                string ingeniero = guna2TextBox2.Text;
                string semanaSeleccionada = guna2ComboBox1.Text;

                // Verificar que la semana seleccionada no sea la antepasada o más antigua
                if (!semanasValidas.Contains(semanaSeleccionada))
                {
                    MessageBox.Show("No se puede utilizar esta semana. Seleccione la semana actual o la anterior.");
                    return; // No realizar ninguna acción si la semana es inválida
                }

                // Iterar sobre cada fila del DataGridView
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Verificar que no sea una fila vacía
                    {
                        // Obtener el número de proyecto de la columna 0
                        string numeroProyecto = row.Cells[0].Value?.ToString() ?? string.Empty;

                        // Verificar que la semana sea válida
                        if (!string.IsNullOrEmpty(numeroProyecto) && !string.IsNullOrEmpty(ingeniero) &&
                            semanasValidas.Contains(semanaSeleccionada))
                        {
                            // Realizar la verificación y el UPDATE en la tabla Planeador
                            string queryVerificarYActualizar = @"
                        UPDATE [ENGITASK].[dbo].[Planeador]
                        SET [Nombre del Proyecto] = @NombreProyecto,
                            [Puesto] = @Puesto,
                            [Lunes] = @Lunes,
                            [Martes] = @Martes,
                            [Miercoles] = @Miercoles,
                            [Jueves] = @Jueves,
                            [Viernes] = @Viernes,
                            [Sabado] = @Sabado,
                            [Domingo] = @Domingo,
                            [Total de Horas] = @TotalHoras,
                            [Comentarios] = @Comentarios,
                            [Fecha] = GETDATE()
                        WHERE [No#Proyecto] = @NumeroProyecto
                        AND [Ingeniero] = @Ingeniero
                        AND [Semana] = @Semana
                        AND [Saved as] = 'Draft'";

                            using (SqlCommand cmdActualizar = new SqlCommand(queryVerificarYActualizar, con))
                            {
                                // Asignar los parámetros
                                cmdActualizar.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                                cmdActualizar.Parameters.AddWithValue("@Ingeniero", ingeniero);
                                cmdActualizar.Parameters.AddWithValue("@Semana", semanaSeleccionada);
                                cmdActualizar.Parameters.AddWithValue("@NombreProyecto", row.Cells[1].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Puesto", guna2TextBox3.Text);
                                cmdActualizar.Parameters.AddWithValue("@Lunes", row.Cells[6].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Martes", row.Cells[7].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Miercoles", row.Cells[8].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Jueves", row.Cells[2].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Viernes", row.Cells[3].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Sabado", row.Cells[4].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Domingo", row.Cells[5].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@TotalHoras", row.Cells[9].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Comentarios", row.Cells[10].Value?.ToString() ?? (object)DBNull.Value);

                                // Ejecutar el UPDATE
                                int filasAfectadas = cmdActualizar.ExecuteNonQuery();
                                if (filasAfectadas > 0)
                                {
                                    MessageBox.Show($"Registro actualizado para el proyecto {numeroProyecto}.");
                                }
                                else
                                {
                                    // INSERT si no existe
                                    string queryInsertar = @"
                        INSERT INTO [ENGITASK].[dbo].[Planeador]
                        ([No#Proyecto], [Ingeniero], [Semana], [Nombre del Proyecto], [Puesto],
                         [Lunes], [Martes], [Miercoles], [Jueves], [Viernes], [Sabado], [Domingo],
                         [Total de Horas], [Comentarios], [Fecha], [Saved as])
                        VALUES
                        (@NumeroProyecto, @Ingeniero, @Semana, @NombreProyecto, @Puesto,
                         @Lunes, @Martes, @Miercoles, @Jueves, @Viernes, @Sabado, @Domingo,
                         @TotalHoras, @Comentarios, GETDATE(), 'Draft')";

                                    using (SqlCommand cmdInsertar = new SqlCommand(queryInsertar, con))
                                    {
                                        cmdInsertar.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                                        cmdInsertar.Parameters.AddWithValue("@Ingeniero", ingeniero);
                                        cmdInsertar.Parameters.AddWithValue("@Semana", semanaSeleccionada);
                                        cmdInsertar.Parameters.AddWithValue("@NombreProyecto", row.Cells[1].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Puesto", guna2TextBox3.Text);
                                        cmdInsertar.Parameters.AddWithValue("@Lunes", row.Cells[6].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Martes", row.Cells[7].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Miercoles", row.Cells[8].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Jueves", row.Cells[2].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Viernes", row.Cells[3].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Sabado", row.Cells[4].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Domingo", row.Cells[5].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@TotalHoras", row.Cells[9].Value?.ToString() ?? (object)DBNull.Value);
                                        cmdInsertar.Parameters.AddWithValue("@Comentarios", row.Cells[10].Value?.ToString() ?? (object)DBNull.Value);

                                        int filasInsertadas = cmdInsertar.ExecuteNonQuery();
                                        if (filasInsertadas > 0)
                                        {
                                            MessageBox.Show($"Nuevo registro insertado para el proyecto {numeroProyecto}.");
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión
                cnn.CloseConnection();
            }
        }

        private void guna2DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si la celda editada está en las columnas 2 a 8 (Lunes a Domingo)
            if (e.ColumnIndex >= 2 && e.ColumnIndex <= 8)
            {
                // Variables para almacenar la suma de las columnas
                decimal suma = 0;

                // Itera sobre las columnas 2 a 8
                for (int i = 2; i <= 8; i++)
                {
                    // Obtiene el valor de la celda actual
                    var valorCelda = guna2DataGridView1.Rows[e.RowIndex].Cells[i].Value;

                    // Verifica si la celda no es nula o vacía y si se puede convertir a decimal
                    if (valorCelda != null && decimal.TryParse(valorCelda.ToString(), out decimal valor))
                    {
                        suma += valor;
                    }
                }

                // Asigna la suma a la columna 9 (Total de Horas)
                guna2DataGridView1.Rows[e.RowIndex].Cells[9].Value = suma;
            }
        }

        private void UpdateColumnSum(string columnName, System.Windows.Forms.TextBox targetTextBox)
        {
            double total = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cellValue = row.Cells[columnName].Value;
                    if (cellValue != null && double.TryParse(cellValue.ToString(), out double value))
                    {
                        total += value;
                    }
                }
            }

            targetTextBox.Text = total.ToString("F2");
        }


        private void UpdateAllSums()
        {
            UpdateColumnSum("Lunes", textBox2);
            UpdateColumnSum("Martes", textBox1);
            UpdateColumnSum("Miercoles", textBox3);
            UpdateColumnSum("Jueves", textBox4);
            UpdateColumnSum("Viernes", textBox7);
            UpdateColumnSum("Sabado", textBox8);
            UpdateColumnSum("Domingo", textBox5);
            UpdateColumnSum("Total", textBox6);
        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2ComboBox1.SelectedItem != null) // Solo calcula si hay una semana seleccionada
            {
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name is string columnName)
                {
                    switch (columnName)
                    {
                        case "Lunes":
                            UpdateColumnSum("Lunes", textBox2);
                            break;
                        case "Martes":
                            UpdateColumnSum("Martes", textBox1);
                            break;
                        case "Miercoles":
                            UpdateColumnSum("Miercoles", textBox3);
                            break;
                        case "Jueves":
                            UpdateColumnSum("Jueves", textBox4);
                            break;
                        case "Viernes":
                            UpdateColumnSum("Viernes", textBox7);
                            break;
                        case "Sabado":
                            UpdateColumnSum("Sabado", textBox8);
                            break;
                        case "Domingo":
                            UpdateColumnSum("Domingo", textBox5);
                            break;
                        case "Total":
                            UpdateColumnSum("Total", textBox6);
                            break;
                    }
                }
            }
        }

        private void guna2DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.IsCurrentCellDirty)
            {
                guna2DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void guna2DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int colIndex = guna2DataGridView1.CurrentCell.ColumnIndex;

            // Si la celda editada está en las columnas 2 a 8, restringir entrada solo a números
            if (colIndex >= 2 && colIndex <= 8 && e.Control is System.Windows.Forms.TextBox textBox)
            {
                // Desconectar el evento anterior para evitar múltiples conexiones
                textBox.KeyPress -= TextBox_KeyPressOnlyNumbers;

                // Conectar el evento KeyPress para restringir la entrada a solo números
                textBox.KeyPress += TextBox_KeyPressOnlyNumbers;
            }

            else if (colIndex == 10 && e.Control is System.Windows.Forms.TextBox textBox10)
            {
                textBox10.KeyPress -= TextBox_KeyPressOnlyNumbers;
            }
        }

        private void TextBox_KeyPressOnlyNumbers(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos, tecla de retroceso (Backspace) y un solo punto decimal
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true; // Bloquear entrada no numérica
            }

            // Si ya hay un punto, evitar que se ingrese otro
            if (e.KeyChar == '.' && sender is System.Windows.Forms.TextBox textBox && textBox.Text.Contains("."))
            {
                e.Handled = true; // Bloquear entrada de más de un punto decimal
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0) // Verificar si hay una fila seleccionada
            {
                // Obtener el No#Proyecto de la fila seleccionada (columna 0)
                string noProyecto = guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                // Obtener la semana desde el guna2ComboBox1
                string semana = guna2ComboBox1.SelectedItem?.ToString(); // Verifica si hay un valor seleccionado

                // Obtener el ingeniero desde guna2TextBox2
                string ingeniero = guna2TextBox2.Text.Trim(); // Eliminar espacios en blanco

                if (string.IsNullOrEmpty(semana))
                {
                    MessageBox.Show("Seleccione una semana antes de eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrEmpty(ingeniero))
                {
                    MessageBox.Show("Ingrese el nombre del ingeniero antes de eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Confirmación antes de eliminar
                DialogResult result = MessageBox.Show($"¿Está seguro de eliminar el proyecto {noProyecto} en la semana {semana} para el ingeniero {ingeniero}?",
                                                      "Confirmar eliminación",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Llamar a la función para eliminar de la base de datos
                    conexion miConexion = new conexion();
                    bool eliminado = EliminarDeBaseDatos(noProyecto, semana, ingeniero, miConexion);

                    if (eliminado)
                    {
                        // Eliminar la fila del DataGridView
                        guna2DataGridView1.Rows.RemoveAt(guna2DataGridView1.SelectedRows[0].Index);
                        MessageBox.Show("Proyecto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el proyecto. Verifique los datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Cerrar la conexión
                    miConexion.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool EliminarDeBaseDatos(string noProyecto, string semana, string ingeniero, conexion miConexion)
        {
            bool eliminado = false;

            try
            {
                using (SqlCommand comando = new SqlCommand("DELETE FROM [ENGITASK].[dbo].[Planeador] WHERE [No#Proyecto] = @NoProyecto AND [Semana] = @Semana AND [Ingeniero ] = @Ingeniero", miConexion.GetConnection()))
                {
                    comando.Parameters.AddWithValue("@NoProyecto", noProyecto);
                    comando.Parameters.AddWithValue("@Semana", semana);
                    comando.Parameters.AddWithValue("@Ingeniero", ingeniero);

                    int filasAfectadas = comando.ExecuteNonQuery();
                    eliminado = filasAfectadas > 0; // Si al menos una fila fue eliminada, retorna true
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return eliminado;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void ObtenerPeriodoSemana()
        {
            if (guna2ComboBox1.SelectedItem == null)
                return;

            if (!int.TryParse(guna2ComboBox1.SelectedItem.ToString(), out int semanaSeleccionada))
            {
                textBox9.Text = "Semana inválida";
                return;
            }

            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

            try
            {
                string query = @"
            SELECT 
                MIN(CAST(FechaNumerica AS DATE)) AS FechaInicio,
                MAX(CAST(FechaNumerica AS DATE)) AS FechaFin
            FROM Calendar
            WHERE NumeroSemana = @NumeroSemana 
              AND YEAR(CAST(FechaNumerica AS DATE)) = @Anio";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NumeroSemana", semanaSeleccionada);
                    cmd.Parameters.AddWithValue("@Anio", DateTime.Now.Year);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            DateTime fechaInicio = reader.GetDateTime(0);
                            DateTime fechaFin = reader.GetDateTime(1);

                            textBox9.Text = $"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}";
                        }
                        else
                        {
                            textBox9.Text = "Periodo no encontrado";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el periodo de la semana: " + ex.Message);
            }
            finally
            {
                cnn.CloseConnection();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            string searchText = guna2TextBox1.Text.Trim();
            if (searchText != "Buscar número de proyecto..." && !string.IsNullOrEmpty(searchText))
            {
                FilterProjectNumbers(searchText);
            }
            else if (string.IsNullOrWhiteSpace(searchText))
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(allProjectNumbers.ToArray());
            }
        }

        private void FilterProjectNumbers(string searchText)
        {
            try
            {
                listBox1.Items.Clear();
                var filtered = allProjectNumbers
                    .Where(p => p.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .Distinct()
                    .Take(10) // Limitar a 10 resultados
                    .ToArray();
                listBox1.Items.AddRange(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar proyectos: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedProjectNumber = listBox1.SelectedItem.ToString();
                // Opcional: Mostrar el valor seleccionado si lo necesitas
                // MessageBox.Show($"Proyecto seleccionado: {selectedProjectNumber}");
            }
        }

        private async Task LoadActiveProjectsAsync()
        {
            conexion cnn = new conexion();
            try
            {
                listBox1.Enabled = false; // Deshabilitar mientras carga
                using (SqlConnection con = cnn.GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();
                    }

                    string query = @"SELECT DISTINCT [Numero de Proyecto]
                                   FROM [ENGITASK].[dbo].[Proyectos]
                                   WHERE [Estatus] = 'Activo'
                                   ORDER BY [Numero de Proyecto]";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            listBox1.Items.Clear();
                            allProjectNumbers.Clear();
                            while (await reader.ReadAsync())
                            {
                                string projectNumber = reader["Numero de Proyecto"].ToString();
                                allProjectNumbers.Add(projectNumber);
                                listBox1.Items.Add(projectNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando proyectos: " + ex.Message);
            }
            finally
            {
                listBox1.Enabled = true; // Rehabilitar
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedProjectNumber = listBox1.SelectedItem.ToString();
                string projectName = await GetProjectNameAsync(selectedProjectNumber); // Obtener el nombre del proyecto
                string ingeniero = guna2TextBox2.Text.Trim();  // Obtener el nombre del ingeniero
                string semana = guna2ComboBox1.SelectedItem?.ToString().Trim();// Obtener el número de la semana

                if (string.IsNullOrEmpty(ingeniero) || string.IsNullOrEmpty(semana))
                {
                    MessageBox.Show("El campo Ingeniero y Semana son obligatorios.");
                    return;
                }

                // Verificar si ya existe en el DataGridView
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (row.Cells[0].Value?.ToString() == selectedProjectNumber)  // Comparar No#Proyecto
                    {
                        MessageBox.Show("Este proyecto ya ha sido agregado.");
                        return;
                    }
                }

                // Verificar en la base de datos si ya existe
                if (await ProyectoYaRegistradoAsync(ingeniero, selectedProjectNumber, semana))
                {
                    MessageBox.Show("Este proyecto ya está registrado para este ingeniero en la semana seleccionada.");
                    return;
                }

                // Agregar la fila solo si pasa las validaciones
                if (projectName != null)
                {
                    guna2DataGridView1.Rows.Insert(0, selectedProjectNumber, projectName);
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar el nombre del proyecto.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un proyecto del ListBox primero.");
            }
        }

        private async Task<bool> ProyectoYaRegistradoAsync(string ingeniero, string noProyecto, string semana)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM [ENGITASK].[dbo].[Planeador]
        WHERE [Ingeniero ] = @Ingeniero
          AND [No#Proyecto] = @NoProyecto
          AND [Semana] = @Semana";

            conexion cnn = new conexion(); // Instancia de tu clase de conexión

            try
            {
                using (SqlConnection con = cnn.GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();
                    }

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Ingeniero", ingeniero);
                        command.Parameters.AddWithValue("@NoProyecto", noProyecto);
                        command.Parameters.AddWithValue("@Semana", semana);

                        int count = (int)await command.ExecuteScalarAsync();
                        return count > 0; // Retorna true si ya existe
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar en la base de datos: " + ex.Message);
                return false;
            }
        }

        private async Task<string> GetProjectNameAsync(string projectNumber)
        {
            conexion cnn = new conexion();
            try
            {
                using (SqlConnection con = cnn.GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        await con.OpenAsync();
                    }

                    string query = @"SELECT [Nombre]
                                   FROM [ENGITASK].[dbo].[Proyectos]
                                   WHERE [Numero de Proyecto] = @projectNumber
                                   AND [Estatus] = 'Activo'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@projectNumber", projectNumber);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return reader["Nombre"].ToString();
                            }
                            return null; // Si no se encuentra el nombre
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el nombre del proyecto: " + ex.Message);
                return null;
            }
        }

    }
}
