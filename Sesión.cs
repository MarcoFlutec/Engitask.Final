using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engitask
{
    public static class Session
    {
        public static UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public string CorreoUsuario { get; set; }
        public string RolUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Puesto { get; set; }
    }

}
