using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benjamin.PracticoMVC.WebApp.Models.Usuarios
{
    public class UsuariosModel
    {

        public List<Entidades.Join_UsuariosRoles> ListaDeUsuarios { get; set; }

        public List<Entidades.Roles> ListaDeRoles { get; set; }

        public DateTime Hora { get; set; }


    }
}