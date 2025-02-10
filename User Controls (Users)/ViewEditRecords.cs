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

namespace Engitask.User_Controls
{
    public partial class ViewEditRecords : UserControl
    {
        public ViewEditRecords() // Constructor de ViewEditRecords
        {
            InitializeComponent();
            // Actualiza las sumas
            UpdateAllSums();
            this.Load += new EventHandler(ViewEditRecords_Load); // Cambiar a ViewEditRecords_Load
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
                                reader["Lunes"].ToString(),
                                reader["Martes"].ToString(),
                                reader["Miercoles"].ToString(),
                                reader["Jueves"].ToString(),
                                reader["Viernes"].ToString(),
                                reader["Sabado"].ToString(),
                                reader["Domingo"].ToString(),
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


        private void ViewEditRecords_Load(object sender, EventArgs e) // Método corregido para el evento Load
        {
            LoadWeeks();
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
              
            }
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            // Crear la conexión a la base de datos
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

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
                                cmdActualizar.Parameters.AddWithValue("@Lunes", row.Cells[2].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Martes", row.Cells[3].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Miercoles", row.Cells[4].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Jueves", row.Cells[5].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Viernes", row.Cells[6].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Sabado", row.Cells[7].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Domingo", row.Cells[8].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@TotalHoras", row.Cells[9].Value?.ToString() ?? (object)DBNull.Value);
                                cmdActualizar.Parameters.AddWithValue("@Comentarios", row.Cells[10].Value?.ToString() ?? (object)DBNull.Value);

                                // Ejecutar el UPDATE
                                int filasAfectadas = cmdActualizar.ExecuteNonQuery();
                                if (filasAfectadas > 0)
                                {
                                    MessageBox.Show($"Registro actualizado para el proyecto {numeroProyecto}.");
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


        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            // Crear la conexión a la base de datos
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

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
                        // Obtener el valor de la columna 0 (Número de Proyecto) de la fila actual
                        string numeroProyecto = row.Cells[0].Value == null ? string.Empty : row.Cells[0].Value.ToString();
                        string semana = guna2ComboBox1.Text;

                        if (!string.IsNullOrEmpty(numeroProyecto) && !string.IsNullOrEmpty(semana))
                        {
                            // Verificar si el registro ya existe en la tabla Planeador
                            string queryVerificarRegistro = "SELECT COUNT(*) FROM [ENGITASK].[dbo].[Planeador] WHERE [No#Proyecto] = @NumeroProyecto AND [Semana] = @Semana";

                            using (SqlCommand cmdVerificar = new SqlCommand(queryVerificarRegistro, con))
                            {
                                cmdVerificar.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                                cmdVerificar.Parameters.AddWithValue("@Semana", semana);

                                int count = (int)cmdVerificar.ExecuteScalar(); // Verificar si ya existe el registro

                                if (count > 0)
                                {
                                    // Si el registro ya existe, hacer un UPDATE
                                    string queryUpdate = @"UPDATE [ENGITASK].[dbo].[Planeador]
                                     SET [Ingeniero] = @Ingeniero,
                                         [Nombre del Proyecto] = @NombreProyecto,
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
                                         [Saved as] = 'Submitted',
                                         [Fecha] = GETDATE() 
                                     WHERE [No#Proyecto] = @NumeroProyecto AND [Semana] = @Semana";


                                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con))
                                    {
                                        // Agregar parámetros para el UPDATE
                                        cmdUpdate.Parameters.AddWithValue("@Ingeniero",
                                            string.IsNullOrWhiteSpace(guna2TextBox2.Text) ? (object)DBNull.Value : guna2TextBox2.Text);

                                        cmdUpdate.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);

                                        cmdUpdate.Parameters.AddWithValue("@NombreProyecto",
                                            row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ? (object)DBNull.Value : row.Cells[1].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Puesto",
                                            string.IsNullOrWhiteSpace(guna2TextBox3.Text) ? (object)DBNull.Value : guna2TextBox3.Text);

                                        cmdUpdate.Parameters.AddWithValue("@Semana", semana);

                                        cmdUpdate.Parameters.AddWithValue("@Lunes",
                                            row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ? (object)DBNull.Value : row.Cells[2].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Martes",
                                            row.Cells[3].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()) ? (object)DBNull.Value : row.Cells[3].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Miercoles",
                                            row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()) ? (object)DBNull.Value : row.Cells[4].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Jueves",
                                            row.Cells[5].Value == null || string.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()) ? (object)DBNull.Value : row.Cells[5].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Viernes",
                                            row.Cells[6].Value == null || string.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()) ? (object)DBNull.Value : row.Cells[6].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Sabado",
                                            row.Cells[7].Value == null || string.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()) ? (object)DBNull.Value : row.Cells[7].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Domingo",
                                            row.Cells[8].Value == null || string.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()) ? (object)DBNull.Value : row.Cells[8].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@TotalHoras",
                                            row.Cells[9].Value == null || string.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()) ? (object)DBNull.Value : row.Cells[9].Value.ToString());

                                        cmdUpdate.Parameters.AddWithValue("@Comentarios",
                                            row.Cells[10].Value == null || string.IsNullOrWhiteSpace(row.Cells[10].Value.ToString()) ? (object)DBNull.Value : row.Cells[10].Value.ToString());

                                        // Ejecutar el UPDATE
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                }
                                else
                                {
                                    // Si no existe el registro, hacer un INSERT
                                    string queryInsertar = @"INSERT INTO [ENGITASK].[dbo].[Planeador]
                                      ([Ingeniero], [No#Proyecto], [Nombre del Proyecto], [Puesto], [Semana], [Lunes], 
                                      [Martes], [Miercoles], [Jueves], [Viernes], [Sabado], [Domingo], [Total de Horas], 
                                      [Comentarios], [Saved as], [Fecha])
                                      VALUES
                                      (@Ingeniero, @NoProyecto, @NombreProyecto, @Puesto, @Semana, @Lunes, @Martes, @Miercoles,
                                      @Jueves, @Viernes, @Sabado, @Domingo, @TotalHoras, @Comentarios, 'Submitted', [Fecha] = GETDATE)";

                                    using (SqlCommand cmdInsertar = new SqlCommand(queryInsertar, con))
                                    {
                                        // Agregar los parámetros con valores de las celdas o TextBox
                                        cmdInsertar.Parameters.AddWithValue("@Ingeniero",
                                            string.IsNullOrWhiteSpace(guna2TextBox2.Text) ? (object)DBNull.Value : guna2TextBox2.Text);

                                        cmdInsertar.Parameters.AddWithValue("@NoProyecto", numeroProyecto);

                                        cmdInsertar.Parameters.AddWithValue("@NombreProyecto",
                                            row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ? (object)DBNull.Value : row.Cells[1].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Puesto",
                                            string.IsNullOrWhiteSpace(guna2TextBox3.Text) ? (object)DBNull.Value : guna2TextBox3.Text);

                                        cmdInsertar.Parameters.AddWithValue("@Semana", semana);

                                        cmdInsertar.Parameters.AddWithValue("@Lunes",
                                            row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ? (object)DBNull.Value : row.Cells[2].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Martes",
                                            row.Cells[3].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()) ? (object)DBNull.Value : row.Cells[3].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Miercoles",
                                            row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()) ? (object)DBNull.Value : row.Cells[4].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Jueves",
                                            row.Cells[5].Value == null || string.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()) ? (object)DBNull.Value : row.Cells[5].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Viernes",
                                            row.Cells[6].Value == null || string.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()) ? (object)DBNull.Value : row.Cells[6].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Sabado",
                                            row.Cells[7].Value == null || string.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()) ? (object)DBNull.Value : row.Cells[7].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Domingo",
                                            row.Cells[8].Value == null || string.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()) ? (object)DBNull.Value : row.Cells[8].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@TotalHoras",
                                            row.Cells[9].Value == null || string.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()) ? (object)DBNull.Value : row.Cells[9].Value.ToString());

                                        cmdInsertar.Parameters.AddWithValue("@Comentarios",
                                            row.Cells[10].Value == null || string.IsNullOrWhiteSpace(row.Cells[10].Value.ToString()) ? (object)DBNull.Value : row.Cells[10].Value.ToString());

                                        // Ejecutar el INSERT
                                        cmdInsertar.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Datos actualizados con éxito.");
                guna2DataGridView1.Rows.Clear();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox5.Clear();
                textBox6.Clear();
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

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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
    }
}