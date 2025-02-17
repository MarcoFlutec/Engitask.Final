using Engitask.DataLayer;
using Engitask.DataLayer.Repositories;
using Engitask.Entities;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Engitask.User_Controls
{
    public partial class NewRegister : UserControl
    {
        #region "Private Methods"      
        private List<Proyecto> _ProyectosActivos = new();
        private ProjectRepositories _projRepo = new();
        #endregion

        #region "Constructor"

        public NewRegister()
        {
            CargarProyectosActivos();
            ConfigurarGuna2DataGridView();
            InitializeComponent();
            try
            {
                var noSemana = _projRepo.GetNumeroSemana();
                guna2TextBox1.Text = !String.IsNullOrEmpty(noSemana) ? noSemana : "Fecha no encontrada";
                guna2TextBox2.Text = Session.User.NombreUsuario;
                guna2TextBox3.Text = Session.User.Puesto;

            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void NewRegister_Load(object sender, EventArgs e)
        {
            CargarProyectosActivos();
        }
        #endregion

        #region "DataGrid events"

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

            //Get Projects
            FillCombobBox();

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

                    var ProjectInfo = _ProyectosActivos.FirstOrDefault(x => x.NoProyecto == numeroProyecto);

                    if (ProjectInfo == null)
                    {
                        MessageBox.Show($"Proyecto inexistente o no registrado: {numeroProyecto}");
                        return;
                    }

                    if (ProjectInfo.Status != "Activo")
                    {
                        MessageBox.Show($"El proyecto {numeroProyecto} está inactivo o sin estatus y por tanto no puede ser agregado.");
                        guna2DataGridView2.Rows[e.RowIndex].Cells[0].Value = null;
                        return;
                    }

                    guna2DataGridView2.Rows[e.RowIndex].Cells[1].Value = ProjectInfo.Nombre;

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

        private void guna2DataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Verifica si la variable 'proyectosInfo' está inicializada
            if (_ProyectosActivos == null)
            {
                MessageBox.Show("No se han cargado los proyectos activos. Por favor, inténtelo de nuevo.");
                return;
            }

            int colIndex = guna2DataGridView2.CurrentCell.ColumnIndex;

            // Si la celda es de la columna 0 (Número de Proyecto) y es un ComboBox
            if (colIndex == 0 && e.Control is System.Windows.Forms.ComboBox comboBox)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown; // Permitir escritura en el ComboBox

                // Desactivar autocompletado
                comboBox.AutoCompleteMode = AutoCompleteMode.None;
                comboBox.AutoCompleteSource = AutoCompleteSource.None;

                // Llena el ComboBox con los números de proyecto activos
                comboBox.Items.Clear();
                FillCombobBox();

                // Conectar el evento TextChanged para filtrar dinámicamente
                comboBox.TextChanged -= ComboBox_TextChanged;
                comboBox.TextChanged += ComboBox_TextChanged;
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

        private void guna2DataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context == DataGridViewDataErrorContexts.Formatting ||
                e.Context == DataGridViewDataErrorContexts.Display ||
                e.Context == DataGridViewDataErrorContexts.Parsing)
            {
                e.ThrowException = false; // Evita que la excepción se lance
            }
        }
        #endregion

        #region "Event Forms Controls"

        //TODO: Move SQL Connections
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


        // Evento para filtrar los elementos del ComboBox mientras se escribe
        // The filter is done on the List Variable _ProyectosActivos instead of the Combobox
        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ComboBox comboBox)
            {
                string filter = comboBox.Text.ToLower(); // Texto ingresado en el ComboBox

                var filteredItems = _ProyectosActivos.FindAll(x => x.NoProyecto.ToString().StartsWith(filter));

                // Si no hay coincidencias, simplemente no hacer nada
                if (filteredItems.Count == 0)
                {
                    return; // Ignorar sin errores ni mensajes
                }

                // Actualizar los ítems del ComboBox sin autocompletar el texto
                comboBox.Items.Clear();
                comboBox.Items.AddRange(filteredItems.ToArray());

                // Reabrir el desplegable para mostrar los resultados filtrados
                comboBox.DroppedDown = true;

                // Dejar el texto tal como está, sin modificarlo automáticamente
                comboBox.SelectionStart = comboBox.Text.Length;
                comboBox.SelectionLength = 0;
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
        #endregion

        #region "Common Methods"

        
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

        //Load Active Projects from Repository
        private void CargarProyectosActivos()
        {
            _ProyectosActivos = _projRepo.GetActivesProjects();
        }


        //Fill Combobox with the first 20 Projects in order to avoid overloading
        private void FillCombobBox()
        {
            CargarProyectosActivos();
            var first20Projects = _ProyectosActivos.Skip(20).Select(x => x.NoProyecto);

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)row.Cells["NoDeProyecto"];

                // Llena el ComboBox solo si está vacío
                if (comboBoxCell.Items.Count == 0)
                {
                    comboBoxCell.Items.AddRange(first20Projects);
                }
            }
        }
        #endregion

    }
}

