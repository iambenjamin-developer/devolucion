using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Models.Test
{
    public class TestModel
    {
        public List<Entidades.Join_UsuariosRoles> ListaDeUsuariosyRoles { get; set; }

        
        public Entidades.Join_UsuariosClientes ObjetoUsuarioCliente { get; set; }


        public SelectList listadoRoles { get; set; }

        public string idRolSeleccionado { get; set; }


        public string Mensaje { get; set; }

    }
}