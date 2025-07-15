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
    public partial class Recover : UserControl
    {
        public Recover()
        {
            InitializeComponent();
            // Actualiza las sumas de combox
            
            this.Load += new EventHandler(Recover_Load); // Cambiar a ViewEditRecords_Load
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



        private void Recover_Load(object sender, EventArgs e)
        {
           
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
         
        }
    }
}
