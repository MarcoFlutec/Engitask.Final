using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engitask.DataLayer.Repositories
{
    public class UserRepositories
    {
        private ApplicationDbContext _dbContext;

        public UserRepositories()
        {
            _dbContext = new();
        }
        public static string GetRol(string correo, string password)
        {
            // Crear la conexión
            conexion cnn = new conexion();
            string rol = "";
            try
            {

                // Obtener la conexión desde la clase 'conexion'
                SqlConnection con = cnn.GetConnection();
                // Consulta SQL para verificar las credenciales
                string query = "SELECT Rol FROM Usuarios WHERE [Correo] = @correo AND [Password] = @password";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Agregar parámetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@password", password);
                    //Create an entity 
                    // Ejecutar el comando y obtener el rol del usuario
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rol = reader["Rol"].ToString()!;
                        }
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.CloseConnection();
            }
            return rol;
        }

        public async Task<int> CreateUser(User user)
        {
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User?> GetUser(int id)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<List<User>> GetUsers()
        {
            var result = await _dbContext.Users.ToListAsync();

            return result;
        }

        public async Task UpdateUser(int id, User user)
        {
            var usr = await GetUser(id);
            if (usr != null)
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int id)
        {
            var usr = await GetUser(id);
            if (usr != null)
            {
                _dbContext.Users.Remove(usr);
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
