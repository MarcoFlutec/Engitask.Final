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
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Engitask.DLC_Remodel
{
    public partial class PR : UserControl
    {
        private List<string> allProjectNumbers; // Para almacenar todos los números de proyecto
        private System.Windows.Forms.Timer searchTimer;

        public PR()
        {
            InitializeComponent();
            allProjectNumbers = new List<string>();
            searchTimer = new System.Windows.Forms.Timer { Interval = 300 }; // 300ms de retraso
            searchTimer.Tick += SearchTimer_Tick;

            // Crear la conexión
            conexion cnn = new conexion();
            // Obtener la conexión desde la clase 'conexion'
            SqlConnection con = cnn.GetConnection();

            try
            {
                // Obtener el número de semana actual
                string fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
                string querySemana = "SELECT NumeroSemana FROM Calendar WHERE FechaNumerica = @fechaActual";

                using (SqlCommand cmdSemana = new SqlCommand(querySemana, con))
                {
                    // Agregar el parámetro para evitar SQL Injection
                    cmdSemana.Parameters.AddWithValue("@fechaActual", fechaActual);

                    // Ejecutar la consulta y obtener el número de semana
                    object resultSemana = cmdSemana.ExecuteScalar();

                    if (resultSemana != null)
                    {
                        // Si el resultado es válido, mostrar el número de semana en el TextBox
                        guna2TextBox4.Text = resultSemana.ToString();

                    }
                    else
                    {
                        // Si no se encuentra la fecha, mostrar un mensaje
                        guna2TextBox4.Text = "Fecha no encontrada";
                    }
                }

                // Obtener el periodo de la semana (fecha de inicio y fin) con el número de semana
                string queryPeriodoSemana = "SELECT MIN(FechaNumerica) AS FechaInicio, MAX(FechaNumerica) AS FechaFin " +
                                             "FROM Calendar WHERE NumeroSemana = @NumeroSemana AND YEAR(FechaNumerica) = @Anio";

                using (SqlCommand cmdPeriodoSemana = new SqlCommand(queryPeriodoSemana, con))
                {
                    // Agregar los parámetros para evitar SQL Injection
                    cmdPeriodoSemana.Parameters.AddWithValue("@NumeroSemana", guna2TextBox4.Text);
                    cmdPeriodoSemana.Parameters.AddWithValue("@Anio", DateTime.Now.Year); // Año actual para filtrar el periodo de este año

                    // Ejecutar la consulta y obtener las fechas de inicio y fin de la semana
                    using (SqlDataReader reader = cmdPeriodoSemana.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Verificar si las fechas no son nulas
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                // Obtener las fechas como string
                                string fechaInicioString = reader.GetString(0);
                                string fechaFinString = reader.GetString(1);

                                DateTime fechaInicio;
                                DateTime fechaFin;

                                // Intentamos convertir las cadenas a DateTime
                                if (DateTime.TryParse(fechaInicioString, out fechaInicio) && DateTime.TryParse(fechaFinString, out fechaFin))
                                {
                                    // Mostrar el rango de fechas en el TextBox3
                                    textBox9.Text = $"{fechaInicio.ToString("dd/MM/yyyy")} - {fechaFin.ToString("dd/MM/yyyy")}";
                                }
                                else
                                {
                                    // Si la conversión falla, mostrar un mensaje
                                    textBox9.Text = $"Error de conversión de fecha. Inicio: {fechaInicioString}, Fin: {fechaFinString}";
                                }
                            }
                            else
                            {
                                // Si alguna de las fechas es nula, mostrar un mensaje
                                textBox9.Text = "Periodo no encontrado (fechas nulas)";
                            }
                        }
                        else
                        {
                            // Si no se encuentra el periodo de la semana, mostrar un mensaje
                            textBox9.Text = "Periodo no encontrado (sin datos)";
                        }
                    }
                }

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


        private async void PR_Load(object sender, EventArgs e)
        {
            ConfigureTextBox();
            await LoadActiveProjectsAsync(); // Cargar proyectos al iniciar
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            guna2TextBox1.TextChanged += new EventHandler(guna2TextBox1_TextChanged); //
            guna2DataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(guna2DataGridView1_EditingControlShowing);
        }

        private void guna2DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.ColumnIndex >= 2 && guna2DataGridView1.CurrentCell.ColumnIndex <= 8) // Columnas 2 a 8
            {
                if (e.Control is System.Windows.Forms.TextBox textBox)
                {
                    // Remover manejadores previos para evitar duplicados
                    textBox.KeyPress -= new KeyPressEventHandler(DataGridViewTextBox_KeyPress);
                    textBox.KeyPress += new KeyPressEventHandler(DataGridViewTextBox_KeyPress);
                }
            }
        }
        private void DataGridViewTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos (0-9) y teclas de control (como Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancelar la tecla si no es un número ni control
            }
        }

        private void ConfigureTextBox()
        {
            try
            {
                guna2TextBox1.Text = "Buscar número de proyecto...";
                guna2TextBox1.ForeColor = Color.Gray;

                guna2TextBox1.Enter += (s, e) =>
                {
                    if (guna2TextBox1.Text == "Buscar número de proyecto...")
                    {
                        guna2TextBox1.Text = "";
                        guna2TextBox1.ForeColor = Color.Black;
                    }
                };

                guna2TextBox1.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
                    {
                        guna2TextBox1.Text = "Buscar número de proyecto...";
                        guna2TextBox1.ForeColor = Color.Gray;
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar TextBox: " + ex.Message);
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
                    .Take(10) // Limitar a 10 resultados
                    .ToArray();
                listBox1.Items.AddRange(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar proyectos: " + ex.Message);
            }
        }



        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedProjectNumber = listBox1.SelectedItem.ToString();
                string projectName = await GetProjectNameAsync(selectedProjectNumber); // Obtener el nombre del proyecto
                string ingeniero = guna2TextBox2.Text.Trim();  // Obtener el nombre del ingeniero
                string semana = guna2TextBox4.Text.Trim();  // Obtener el número de la semana

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

        // Método para consultar la base de datos y verificar si el proyecto ya está registrado
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

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos (0-9) y teclas de control (como Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancelar la tecla si no es un número ni control
            }
        }

        private void guna2DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si la celda editada está en las columnas 2 a 8 (Lunes a Domingo)
            if (e.ColumnIndex >= 2 && e.ColumnIndex <= 8)
            {
                decimal suma = 0;

                for (int i = 2; i <= 8; i++)
                {
                    var valorCelda = guna2DataGridView1.Rows[e.RowIndex].Cells[i].Value;

                    if (valorCelda != null && decimal.TryParse(valorCelda.ToString(), out decimal valor))
                    {
                        suma += valor;
                    }
                }

                guna2DataGridView1.Rows[e.RowIndex].Cells[9].Value = suma;
            }

        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cellValue = row.Cells[0].Value;
                    string numeroProyecto = cellValue?.ToString().Trim() ?? string.Empty;

                    if (string.IsNullOrEmpty(numeroProyecto))
                    {
                        MessageBox.Show("No dejar No de proyecto vacío");
                        break;
                    }
                }
            }

            // Crear la conexión a la base de datos
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();
            if (con.State != ConnectionState.Open)
                con.Open();


            bool seMostroMensajeDuplicado = false; // Variable para rastrear si se detectó un duplicado

            try
            {
                // Obtener valores de Ingeniero y Semana
                string ingeniero = guna2TextBox2.Text;
                string semana = guna2TextBox4.Text;

                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Verificar que no sea una fila vacía
                    {
                        string numeroProyecto = row.Cells[0].Value == null ? string.Empty : row.Cells[0].Value.ToString();
                        DateTime fecha = DateTime.Now; // O toma la fecha correspondiente si está en otra columna

                        if (!string.IsNullOrEmpty(numeroProyecto))
                        {
                            // Verificar si ya existe un registro con el mismo Ingeniero, Semana, No#Proyecto y Fecha
                            string queryVerificarExistente = @"SELECT COUNT(*) FROM [ENGITASK].[dbo].[Planeador] 
                                            WHERE [No#Proyecto] = @NoProyecto 
                                            AND [Ingeniero] = @Ingeniero 
                                            AND [Semana] = @Semana
                                            AND CONVERT(DATE, [Fecha]) = @Fecha";

                            using (SqlCommand cmdVerificarExistente = new SqlCommand(queryVerificarExistente, con))
                            {
                                cmdVerificarExistente.Parameters.AddWithValue("@NoProyecto", numeroProyecto);
                                cmdVerificarExistente.Parameters.AddWithValue("@Ingeniero", ingeniero);
                                cmdVerificarExistente.Parameters.AddWithValue("@Semana", semana);
                                cmdVerificarExistente.Parameters.AddWithValue("@Fecha", fecha.Date);

                                int count = (int)cmdVerificarExistente.ExecuteScalar();

                                if (count > 0)
                                {
                                    // Si ya existe un registro, mostrar mensaje y saltar a la siguiente fila
                                    MessageBox.Show($"Ya existe un registro para el proyecto {numeroProyecto}, Ingeniero {ingeniero}, Semana {semana} y Fecha {fecha.ToShortDateString()}. No se puede duplicar.");
                                    MessageBox.Show("Por favor cambia el numero de proyecto o borralo para poder continuar.");
                                    seMostroMensajeDuplicado = true; // Se detectó un duplicado
                                    break;
                                }
                                else
                                {
                                    // Verificar si el proyecto existe en la tabla Proyectos
                                    string queryVerificarProyecto = "SELECT COUNT(*) FROM [ENGITASK].[dbo].[Proyectos] WHERE [Numero de Proyecto] = @NumeroProyecto";

                                    using (SqlCommand cmdVerificarProyecto = new SqlCommand(queryVerificarProyecto, con))
                                    {
                                        cmdVerificarProyecto.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                                        int proyectoExiste = (int)cmdVerificarProyecto.ExecuteScalar();

                                        if (proyectoExiste > 0)
                                        {
                                            // Realizar el insert en la tabla Planeador
                                            string queryInsertar = @"INSERT INTO [ENGITASK].[dbo].[Planeador]
                         ([Ingeniero], [No#Proyecto], [Nombre del Proyecto], [Puesto], [Semana], [Lunes], 
                         [Martes], [Miercoles], [Jueves], [Viernes], [Sabado], [Domingo], [Total de Horas], 
                         [Comentarios], [Saved as], [Fecha])
                         VALUES
                         (@Ingeniero, @NoProyecto, @NombreProyecto, @Puesto, @Semana, @Lunes, @Martes, @Miercoles,
                         @Jueves, @Viernes, @Sabado, @Domingo, @TotalHoras, @Comentarios, @SavedAs, @Fecha)";

                                            using (SqlCommand cmdInsertar = new SqlCommand(queryInsertar, con))
                                            {
                                                // Agregar los parámetros con valores de las celdas o TextBox
                                                cmdInsertar.Parameters.AddWithValue("@Ingeniero", ingeniero);
                                                cmdInsertar.Parameters.AddWithValue("@NoProyecto", numeroProyecto);
                                                cmdInsertar.Parameters.AddWithValue("@NombreProyecto",
                                                    row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ? (object)DBNull.Value : row.Cells[1].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Puesto",
                                                    string.IsNullOrWhiteSpace(guna2TextBox3.Text) ? (object)DBNull.Value : guna2TextBox3.Text);
                                                cmdInsertar.Parameters.AddWithValue("@Semana", semana);
                                                cmdInsertar.Parameters.AddWithValue("@Lunes",
                                                    row.Cells[6].Value == null || string.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()) ? (object)DBNull.Value : row.Cells[6].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Martes",
                                                    row.Cells[7].Value == null || string.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()) ? (object)DBNull.Value : row.Cells[7].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Miercoles",
                                                    row.Cells[8].Value == null || string.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()) ? (object)DBNull.Value : row.Cells[8].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Jueves",
                                                    row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ? (object)DBNull.Value : row.Cells[2].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Viernes",
                                                    row.Cells[3].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()) ? (object)DBNull.Value : row.Cells[3].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Sabado",
                                                    row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()) ? (object)DBNull.Value : row.Cells[4].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Domingo",
                                                    row.Cells[5].Value == null || string.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()) ? (object)DBNull.Value : row.Cells[5].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@TotalHoras",
                                                    row.Cells[9].Value == null || string.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()) ? (object)DBNull.Value : row.Cells[9].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@Comentarios",
                                                    row.Cells[10].Value == null || string.IsNullOrWhiteSpace(row.Cells[10].Value.ToString()) ? (object)DBNull.Value : row.Cells[10].Value.ToString());
                                                cmdInsertar.Parameters.AddWithValue("@SavedAs", "Draft");
                                                cmdInsertar.Parameters.AddWithValue("@Fecha", fecha);

                                                // Ejecutar el insert
                                                cmdInsertar.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            // Si el proyecto no existe, mostrar mensaje de error
                                            MessageBox.Show($"Proyecto inexistente o no registrado: {numeroProyecto}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Si no hubo mensajes de duplicado, limpiar los controles
                if (!seMostroMensajeDuplicado)
                {
                    guna2DataGridView1.Rows.Clear(); // Limpiar el DataGridView
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                }

                MessageBox.Show("Proceso completado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión
                cnn.CloseConnection();
            }
        }
    }
}



     
