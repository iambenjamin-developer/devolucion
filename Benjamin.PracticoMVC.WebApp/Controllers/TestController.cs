using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class TestController : Controller
    {

        public ActionResult TablaUsuarios()
        {


            AccesoDatos.Test metodos = new AccesoDatos.Test();

            List<Entidades.Join_UsuariosRoles> listaUyR = metodos.ListarUsuariosRoles();

            var model = new Models.Test.TestModel();

            model.ListaDeUsuariosyRoles = listaUyR;

            return View(model);

        }

        [HttpGet]
        public ActionResult AgregarUsuario()
        {
            AccesoDatos.Test metodos = new AccesoDatos.Test();

            IList<Entidades.Roles> roles = metodos.ListaDeRoles();

            SelectList listaRoles = new SelectList(roles, "Id", "Descripcion");


            var model = new Models.Test.TestModel();

            model.listadoRoles = listaRoles;

            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarUsuario(Models.Test.TestModel model)
        {


            Entidades.Join_UsuariosClientes obj = model.ObjetoUsuarioCliente;

            obj.ID_ROL = model.idRolSeleccionado;

            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

            int filasAfectadas = metodos.CrearUsuarioCliente(obj);


            return RedirectToAction("TablaUsuarios", "Test");
            //string mensaje = "agerrgador";
            ////boostrap alert
            //TablaUsuarios(mensaje);
        }

        [HttpGet]
        public ActionResult EditarUsuario(int idUsuario)
        {
            string editar = "Editar Usuario Nº " + idUsuario.ToString();
            ViewBag.idUsuario = editar;


            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

            var userSeleccionado = metodos.Detalle(idUsuario);

            var model = new Models.Test.TestModel();

            model.ObjetoUsuarioCliente = userSeleccionado;


            return View(model);
        }

        [HttpPost]
        public ActionResult EditarUsuario(Models.Test.TestModel model)
        {
            return View();
        }


        public ActionResult test()
        {
            return View();
        }

    }
}