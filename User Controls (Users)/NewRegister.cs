using Engitask;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Engitask.User_Controls
{
    public partial class NewRegister : UserControl
    {
        private ProyectoDataLoader proyectoDataLoader = new ProyectoDataLoader();

        public NewRegister()
        {
            CargarProyectosActivos();
            ConfigurarGuna2DataGridView();
            InitializeComponent();
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
                        guna2TextBox1.Text = resultSemana.ToString();

                    }
                    else
                    {
                        // Si no se encuentra la fecha, mostrar un mensaje
                        guna2TextBox1.Text = "Fecha no encontrada";
                    }
                }

                // Obtener el periodo de la semana (fecha de inicio y fin) con el número de semana
                string queryPeriodoSemana = "SELECT MIN(FechaNumerica) AS FechaInicio, MAX(FechaNumerica) AS FechaFin " +
                                             "FROM Calendar WHERE NumeroSemana = @NumeroSemana AND YEAR(FechaNumerica) = @Anio";

                using (SqlCommand cmdPeriodoSemana = new SqlCommand(queryPeriodoSemana, con))
                {
                    // Agregar los parámetros para evitar SQL Injection
                    cmdPeriodoSemana.Parameters.AddWithValue("@NumeroSemana", guna2TextBox1.Text);
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


        private void NewRegister_Load(object sender, EventArgs e)
        {
            var proyectosActivos = proyectoDataLoader.ObtenerProyectosActivos();

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)row.Cells["NoDeProyecto"];

                // Llena el ComboBox solo si está vacío
                if (comboBoxCell.Items.Count == 0)
                {
                    foreach (string proyecto in proyectosActivos.Keys)
                    {
                        comboBoxCell.Items.Add(proyecto);
                    }
                }
            }
        }


        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                if (guna2DataGridView2.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    MessageBox.Show($"Botón presionado en la fila {e.RowIndex}");
                }


                if (guna2DataGridView2.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
                {
                    var cell = guna2DataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                    if (cell != null)
                    {
                        MessageBox.Show($"Valor seleccionado: {cell.Value}");
                    }
                }
            }
        }

        private void ConfigurarGuna2DataGridView()
        {
            if (guna2DataGridView2 == null)
            {
                return;
            }

            try
            {
                guna2DataGridView2.GridColor = Color.Black;
                guna2DataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                guna2DataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                guna2DataGridView2.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                guna2DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                guna2DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                guna2DataGridView2.DefaultCellStyle.BackColor = Color.White;
                guna2DataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            }
            catch (NullReferenceException ex)
            {

            }
        }


        private void guna2DataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            conexion cnn = new conexion();
            using (SqlConnection con = cnn.GetConnection())
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Consulta para obtener los proyectos activos
                string query = "SELECT [Numero de Proyecto], [Nombre], [Estatus] FROM [ENGITASK].[dbo].[Proyectos] WHERE [Estatus] = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, con);

                // Crear una lista para almacenar los proyectos activos y un diccionario para nombre y estatus
                List<string> proyectosActivos = new List<string>();
                Dictionary<string, (string Nombre, string Estatus)> proyectosInfo = new Dictionary<string, (string, string)>();

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string numeroProyecto = reader["Numero de Proyecto"].ToString();
                        string nombre = reader["Nombre"].ToString();
                        string estatus = reader["Estatus"].ToString();

                        proyectosActivos.Add(numeroProyecto);
                        proyectosInfo[numeroProyecto] = (nombre, estatus);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar proyectos: " + ex.Message);
                    return;
                }

                // Llenar ComboBox de cada fila solo con proyectos activos
                foreach (DataGridViewRow row in guna2DataGridView2.Rows)
                {
                    DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)row.Cells["NoDeProyecto"];

                    if (comboBoxCell.Items.Count == 0)
                    {
                        foreach (string proyecto in proyectosActivos)
                        {
                            comboBoxCell.Items.Add(proyecto);
                        }
                    }
                }

                // Verifica si la celda editada es de la columna 0 (Número de Proyecto)
                if (e.ColumnIndex == 0)
                {
                    string numeroProyecto = guna2DataGridView2.Rows[e.RowIndex].Cells[0].Value?.ToString();

                    if (!string.IsNullOrEmpty(numeroProyecto))
                    {
                        // Verifica si el número de proyecto ya existe en la columna 0
                        bool existe = false;
                        for (int i = 0; i < guna2DataGridView2.Rows.Count; i++)
                        {
                            if (i != e.RowIndex && guna2DataGridView2.Rows[i].Cells[0].Value?.ToString() == numeroProyecto)
                            {
                                existe = true;
                                break;
                            }
                        }

                        if (existe)
                        {
                            MessageBox.Show($"El número de proyecto {numeroProyecto} ya está agregado en otra fila.");
                            guna2DataGridView2.Rows[e.RowIndex].Cells[0].Value = null;
                            return;
                        }

                        // Obtiene nombre y estatus del proyecto si existe
                        if (proyectosInfo.TryGetValue(numeroProyecto, out var proyectoInfo))
                        {
                            if (proyectoInfo.Estatus.Equals("Activo", StringComparison.OrdinalIgnoreCase))
                            {
                                guna2DataGridView2.Rows[e.RowIndex].Cells[1].Value = proyectoInfo.Nombre;
                            }
                            else
                            {
                                MessageBox.Show($"El proyecto {numeroProyecto} está inactivo o sin estatus y por tanto no puede ser agregado.");
                                guna2DataGridView2.Rows[e.RowIndex].Cells[0].Value = null;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Proyecto inexistente o no registrado: {numeroProyecto}");
                        }
                    }
                }

                // Verifica si la celda editada está en las columnas 2 a 8 (Lunes a Domingo)
                if (e.ColumnIndex >= 2 && e.ColumnIndex <= 8)
                {
                    decimal suma = 0;

                    for (int i = 2; i <= 8; i++)
                    {
                        var valorCelda = guna2DataGridView2.Rows[e.RowIndex].Cells[i].Value;

                        if (valorCelda != null && decimal.TryParse(valorCelda.ToString(), out decimal valor))
                        {
                            suma += valor;
                        }
                    }

                    guna2DataGridView2.Rows[e.RowIndex].Cells[9].Value = suma;
                }

            }
        }

        private void UpdateColumnSum(string columnName, System.Windows.Forms.TextBox targetTextBox)
        {
            double total = 0;

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
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

        private void guna2DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView2.Columns[e.ColumnIndex].Name is string columnName)
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

        private void guna2DataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView2.IsCurrentCellDirty)
            {
                guna2DataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton3_Click_1(object sender, EventArgs e)
        {
            guna2DataGridView2.Rows.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void guna2GradientButton4_Click_1(object sender, EventArgs e)
        {


            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
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

            bool seMostroMensajeDuplicado = false; // Variable para rastrear si se detectó un duplicado

            try
            {
                // Obtener valores de Ingeniero y Semana
                string ingeniero = guna2TextBox2.Text;
                string semana = guna2TextBox1.Text;

                foreach (DataGridViewRow row in guna2DataGridView2.Rows)
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
                    guna2DataGridView2.Rows.Clear();
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

        private void guna2GradientButton5_Click_1(object sender, EventArgs e)
        {
            // Crear la conexión a la base de datos
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();
            bool registroDuplicado = false; // Bandera para saber si hubo registros duplicados

            try
            {
                // Obtener los valores de ingeniero y semana
                string ingeniero = string.IsNullOrWhiteSpace(guna2TextBox2.Text) ? string.Empty : guna2TextBox2.Text;
                int semana = int.TryParse(guna2TextBox1.Text, out int resultadoSemana) ? resultadoSemana : 0;

                // Iterar sobre cada fila del DataGridView
                foreach (DataGridViewRow row in guna2DataGridView2.Rows)
                {
                    if (!row.IsNewRow) // Verificar que no sea una fila vacía
                    {
                        string numeroProyecto = row.Cells[0].Value == null ? string.Empty : row.Cells[0].Value.ToString();

                        if (!string.IsNullOrEmpty(numeroProyecto))
                        {
                            // Verificar si ya existe un registro con el mismo ingeniero, semana y número de proyecto
                            string queryVerificarRegistro = @"
                    SELECT COUNT(*) 
                    FROM [ENGITASK].[dbo].[Planeador] 
                    WHERE [Ingeniero] = @Ingeniero 
                    AND DATEPART(WEEK, [Fecha]) = @Semana 
                    AND [No#Proyecto] = @NoProyecto";

                            using (SqlCommand cmdVerificarRegistro = new SqlCommand(queryVerificarRegistro, con))
                            {
                                cmdVerificarRegistro.Parameters.AddWithValue("@Ingeniero", ingeniero);
                                cmdVerificarRegistro.Parameters.AddWithValue("@Semana", semana);
                                cmdVerificarRegistro.Parameters.AddWithValue("@NoProyecto", numeroProyecto);

                                int countRegistro = (int)cmdVerificarRegistro.ExecuteScalar();

                                if (countRegistro > 0)
                                {
                                    MessageBox.Show($"Ya existe un registro para el ingeniero {ingeniero}, semana {semana} y número de proyecto {numeroProyecto}.");
                                    registroDuplicado = true; // Se detectó un registro duplicado
                                    continue;
                                }
                            }

                            // Verificar si el proyecto existe en la tabla Proyectos
                            string queryVerificarProyecto = "SELECT COUNT(*) FROM [ENGITASK].[dbo].[Proyectos] WHERE [Numero de Proyecto] = @NumeroProyecto";

                            using (SqlCommand cmdVerificar = new SqlCommand(queryVerificarProyecto, con))
                            {
                                cmdVerificar.Parameters.AddWithValue("@NumeroProyecto", numeroProyecto);
                                int countProyecto = (int)cmdVerificar.ExecuteScalar();

                                if (countProyecto > 0)
                                {
                                    string queryInsertar = @"
                            INSERT INTO [ENGITASK].[dbo].[Planeador]
                            ([Ingeniero], [No#Proyecto], [Nombre del Proyecto], [Puesto], [Semana], [Lunes], 
                            [Martes], [Miercoles], [Jueves], [Viernes], [Sabado], [Domingo], [Total de Horas], 
                            [Comentarios], [Saved as], [Fecha])
                            VALUES
                            (@Ingeniero, @NoProyecto, @NombreProyecto, @Puesto, @Semana, @Lunes, @Martes, @Miercoles,
                            @Jueves, @Viernes, @Sabado, @Domingo, @TotalHoras, @Comentarios, @SavedAs, GETDATE())";

                                    using (SqlCommand cmdInsertar = new SqlCommand(queryInsertar, con))
                                    {
                                        cmdInsertar.Parameters.AddWithValue("@Ingeniero", ingeniero);
                                        cmdInsertar.Parameters.AddWithValue("@NoProyecto", numeroProyecto);
                                        cmdInsertar.Parameters.AddWithValue("@NombreProyecto", row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ? (object)DBNull.Value : row.Cells[1].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Puesto", string.IsNullOrWhiteSpace(guna2TextBox3.Text) ? (object)DBNull.Value : guna2TextBox3.Text);
                                        cmdInsertar.Parameters.AddWithValue("@Semana", semana);
                                        cmdInsertar.Parameters.AddWithValue("@Lunes", row.Cells[6].Value == null || string.IsNullOrWhiteSpace(row.Cells[6].Value.ToString()) ? (object)DBNull.Value : row.Cells[6].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Martes", row.Cells[7].Value == null || string.IsNullOrWhiteSpace(row.Cells[7].Value.ToString()) ? (object)DBNull.Value : row.Cells[7].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Miercoles", row.Cells[8].Value == null || string.IsNullOrWhiteSpace(row.Cells[8].Value.ToString()) ? (object)DBNull.Value : row.Cells[8].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Jueves", row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ? (object)DBNull.Value : row.Cells[2].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Viernes", row.Cells[3].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()) ? (object)DBNull.Value : row.Cells[3].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Sabado", row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()) ? (object)DBNull.Value : row.Cells[4].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Domingo", row.Cells[5].Value == null || string.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()) ? (object)DBNull.Value : row.Cells[5].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@TotalHoras", row.Cells[9].Value == null || string.IsNullOrWhiteSpace(row.Cells[9].Value.ToString()) ? (object)DBNull.Value : row.Cells[9].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@Comentarios", row.Cells[10].Value == null || string.IsNullOrWhiteSpace(row.Cells[10].Value.ToString()) ? (object)DBNull.Value : row.Cells[10].Value.ToString());
                                        cmdInsertar.Parameters.AddWithValue("@SavedAs", "Submitted");

                                        cmdInsertar.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Proyecto inexistente o no registrado: {numeroProyecto}");
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Proceso completado.");

                // Si no hubo registros duplicados, limpiar campos
                if (!registroDuplicado)
                {
                    guna2DataGridView2.Rows.Clear();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cnn.CloseConnection();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        // Bandera para evitar bucles infinitos
        private bool isUpdatingComboBox = false;

        private void guna2DataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (proyectosInfo == null)
            {
                MessageBox.Show("No se han cargado los proyectos activos. Por favor, inténtelo de nuevo.");
                return;
            }

            int colIndex = guna2DataGridView2.CurrentCell.ColumnIndex;

            if (colIndex == 0 && e.Control is System.Windows.Forms.ComboBox comboBox)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.AutoCompleteMode = AutoCompleteMode.None;
                comboBox.AutoCompleteSource = AutoCompleteSource.None;

                comboBox.Items.Clear();
                comboBox.Items.AddRange(proyectosInfo.Keys.ToArray());

                comboBox.TextChanged -= ComboBox_TextChanged;
                comboBox.TextChanged += ComboBox_TextChanged;

                comboBox.KeyDown -= ComboBox_KeyDown;
                comboBox.KeyDown += ComboBox_KeyDown;
            }

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

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is System.Windows.Forms.ComboBox comboBox)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    if (comboBox.SelectedItem == null && comboBox.Items.Count > 0)
                    {
                        var match = comboBox.Items.Cast<string>()
                            .FirstOrDefault(item => item.StartsWith(comboBox.Text, StringComparison.OrdinalIgnoreCase));

                        if (match != null)
                        {
                            isUpdatingComboBox = true; // Evitar bucles
                            comboBox.SelectedItem = match;
                            isUpdatingComboBox = false;
                        }
                    }

                    guna2DataGridView2.EndEdit();
                    SendKeys.Send("{TAB}");
                }
            }
        }


        // Método para restringir entrada a solo números
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

        // Variable para almacenar los proyectos activos
        private Dictionary<string, (string Nombre, string Estatus)> proyectosInfo;

        // Evento para filtrar los elementos del ComboBox mientras se escribe
        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            if (isUpdatingComboBox) return; // Evitar bucles infinitos

            if (sender is System.Windows.Forms.ComboBox comboBox)
            {
                string filter = comboBox.Text.ToLower();
                var filteredItems = proyectosInfo.Keys
                    .Where(p => p.ToLower().Contains(filter))
                    .ToArray();

                isUpdatingComboBox = true;  // Evitar que el evento se dispare nuevamente

                string currentText = comboBox.Text;

                comboBox.BeginUpdate();
                comboBox.Items.Clear();
                comboBox.Items.AddRange(filteredItems);
                comboBox.Text = currentText;
                comboBox.SelectionStart = currentText.Length;
                comboBox.SelectionLength = 0;
                comboBox.EndUpdate();

                comboBox.DroppedDown = filteredItems.Length > 0;  // Mostrar solo si hay elementos filtrados

                isUpdatingComboBox = false;  // Permitir eventos de nuevo
            }
        }

        // Método para cargar los proyectos activos al inicio
        private void CargarProyectosActivos()
        {
            ProyectoDataLoader dataLoader = new ProyectoDataLoader();
            proyectosInfo = dataLoader.ObtenerProyectosActivos();
        }

        private void guna2DataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (guna2DataGridView2.Columns[e.ColumnIndex].Index == 0)
            {
                string enteredValue = e.FormattedValue.ToString();
                if (!proyectosInfo.ContainsKey(enteredValue))
                {
                    MessageBox.Show("El número de proyecto ingresado no es válido.");
                    e.Cancel = true;
                }
            }
        }

        private void guna2DataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context == DataGridViewDataErrorContexts.Formatting ||
                e.Context == DataGridViewDataErrorContexts.Display ||
                e.Context == DataGridViewDataErrorContexts.Parsing)
            {
                e.ThrowException = false; // Evita que la excepción se lance
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }

}


    public class ProyectoDataLoader
    {
        private conexion cnn = new conexion();

        public Dictionary<string, (string Nombre, string Estatus)> ObtenerProyectosActivos()
        {
            Dictionary<string, (string Nombre, string Estatus)> proyectosInfo = new Dictionary<string, (string, string)>();

            using (SqlConnection con = cnn.GetConnection())
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = "SELECT [Numero de Proyecto], [Nombre], [Estatus] FROM [ENGITASK].[dbo].[Proyectos] WHERE [Estatus] = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string numeroProyecto = reader["Numero de Proyecto"].ToString();
                        string nombre = reader["Nombre"].ToString();
                        string estatus = reader["Estatus"].ToString();

                        proyectosInfo[numeroProyecto] = (nombre, estatus);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar proyectos: " + ex.Message);
                }
            }

            return proyectosInfo;
        }

    

}