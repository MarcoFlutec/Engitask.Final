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
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.Mail;
using System.Net;
using Microsoft.Data.SqlClient;
using Guna.UI2.WinForms;
using Engitask.DataLayer;

namespace Engitask.Olvide_mi_contraseña
{
    public partial class ForgotPassword : Form
    {

        public ForgotPassword()
        {
            InitializeComponent();

        }

        public string CorreoTexto => guna2TextBox1.Text;

        public static class CodigoGenerator
        {
            private static Random random = new Random();
            public static int Codigo { get; private set; } = random.Next(100000, 1000000);

            // Método opcional para regenerar el código si se necesita uno nuevo
            public static void GenerarNuevoCodigo()
            {
                Codigo = random.Next(100000, 1000000);
            }
        }

        public static class Correo
        {
            private static string correo;

            public static string ValorCorreo
            {
                get { return correo; }
                set { correo = value; }
            }
        }


        int codigo = CodigoGenerator.Codigo;


        // Método que se ejecuta al hacer clic en el botón para enviar el código
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {

            // Asigna el correo ingresado al campo estático en la clase Correo
            Correo.ValorCorreo = CorreoTexto;

            // Crear la conexión
            conexion cnn = new conexion();
            SqlConnection con = cnn.GetConnection();

            // Obtener los valores de los TextBox
            string correo = guna2TextBox1.Text;



            if (string.IsNullOrEmpty(correo))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT [User Name] FROM Usuarios WHERE [Correo] = @correo";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Actualizar el campo Codigo del usuario en la base de datos
                        string updateCodeQuery = "UPDATE Usuarios SET Codigo = @codigo WHERE [Correo] = @correo";
                        using (SqlCommand updateCmd = new SqlCommand(updateCodeQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@codigo", codigo);
                            updateCmd.Parameters.AddWithValue("@correo", correo);
                            updateCmd.ExecuteNonQuery();
                        }

                        MailAddress from = new MailAddress("Marco.avila@flutec.com", "Marco");
                        MailAddress to = new MailAddress(correo, "Name and stuff");
                        List<MailAddress> cc = new List<MailAddress> { new MailAddress("Marco.avila@flutec.com", "Marco") };
                        SendEmail("Codigo de Verificación", from, to, cc);

                        CodigoDeVerificación Cv = new CodigoDeVerificación();
                        Cv.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error en conexión por favor intenta de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.CloseConnection();
            }
        }

        private void SendEmail(string _subject, MailAddress _from, MailAddress _to, List<MailAddress> _cc, List<MailAddress> _bcc = null)
        {
            string Text = "Buen día, tu codigo de verificación es:  " + codigo;
            SmtpClient mailClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("Marco.avila@flutec.com", "Pur39755")
            };

            using (MailMessage msgMail = new MailMessage())
            {
                msgMail.From = _from;
                msgMail.To.Add(_to);

                if (_bcc != null)
                {
                    foreach (MailAddress addr in _bcc)
                    {
                        msgMail.Bcc.Add(addr);
                    }
                }
                msgMail.Subject = _subject;
                msgMail.Body = Text;
                msgMail.IsBodyHtml = true;

                mailClient.Send(msgMail);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();
            form1.Show();
            this.Hide();  // Ocultar el Form actual
        }
    }
}

