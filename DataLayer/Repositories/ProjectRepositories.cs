using Engitask.Entities;
using Microsoft.Data.SqlClient;

namespace Engitask.DataLayer.Repositories
{
    public class ProjectRepositories
    {
        public List<Proyecto> GetActivesProjects()
        {
            conexion cnn = new conexion();
            using (SqlConnection con = cnn.GetConnection())
            {
                con.Open();
                // Consulta para obtener los proyectos activos
                string query = "SELECT [Numero de Proyecto], [Nombre], [Estatus] FROM [ENGITASK].[dbo].[Proyectos] WHERE [Estatus] = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, con);

                // Crear una lista para almacenar los proyectos activos y un diccionario para nombre y estatus
                List<Proyecto> proyectos = new();

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        proyectos.Add(new()
                        {
                            NoProyecto = reader["Numero de Proyecto"]?.ToString(),
                            Nombre = reader["Nombre"]?.ToString(),
                            Status = reader["Estatus"]?.ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar proyectos: " + ex.Message);
                    return null;
                }
                con.Close();
                return proyectos;

            }
        }

        public string GetNumeroSemana()
        {
            conexion cnn = new conexion();
            var noSemana = "";
            string fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
            // Obtener la conexión desde la clase 'conexion'
            using (SqlConnection con = cnn.GetConnection())
            {
                con.Open();
                // Obtener el número de semana actual
                string querySemana = "SELECT NumeroSemana FROM Calendar WHERE FechaNumerica = @fechaActual";

                using (SqlCommand cmdSemana = new SqlCommand(querySemana, con))
                {
                    cmdSemana.Parameters.AddWithValue("@fechaActual", fechaActual);
                    SqlDataReader reader = cmdSemana.ExecuteReader();
                    while (reader.Read())
                    {
                        noSemana = reader["NumeroSemana"]?.ToString();
                    }
                    reader.Close();
                   
                }
                con.Close();
                return noSemana;
            }
        }
    }
}
