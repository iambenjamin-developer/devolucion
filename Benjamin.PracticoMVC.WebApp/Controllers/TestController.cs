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

        public ActionResult TablaUsuarios(Models.Test.TestModel modelParametro)
        {
            var model = new Models.Test.TestModel();

            string msj = string.Empty;
            if (modelParametro.Mensaje == null)
            {
                msj = "modelo.Mensaje nulo";
            }
            else {
                msj = "modelo parametro con datos";
                model.Mensaje = modelParametro.Mensaje;
            }


            AccesoDatos.Test metodos = new AccesoDatos.Test();

            List<Entidades.Join_UsuariosRoles> listaUyR = metodos.ListarUsuariosRoles();

            

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

            // ViewBag.MensajeDeAlerta = "para barrio alertifty!";
            model.Mensaje = "usuario agregado con exito!!!!!";
           // ViewData["Message"] = "Success";
            return RedirectToAction("TablaUsuarios", "Test", model);
            //string mensaje = "agerrgador";
            ////boostrap alert
            //TablaUsuarios(mensaje);

          //  return View("TablaUsuarios");
        }

        [HttpGet]
        public ActionResult EditarUsuario(int idUsuario)
        {

            var model = new Models.Test.TestModel();
         
            AccesoDatos.Usuarios servicioUsuario = new AccesoDatos.Usuarios();

            var userSeleccionado = servicioUsuario.Detalle(idUsuario);

            model.ObjetoUsuarioCliente = userSeleccionado;

            AccesoDatos.Test metodos = new AccesoDatos.Test();

            IList<Entidades.Roles> roles = metodos.ListaDeRoles();

            SelectList listaRoles = new SelectList(roles, "Id", "Descripcion", userSeleccionado.ID_ROL);


            model.listadoRoles = listaRoles;

            return View(model);


            //IList<Sexo> sexos = ServicioValorDominio.BuscarTodosSexos();
            //SelectList listaSexos = new SelectList(sexos, "IdValorDominio", "Descripcion", persona.Sexo.IdValorDominio);


        }

        [HttpPost]
        public ActionResult EditarUsuario(Models.Test.TestModel model)
        {
            Entidades.Join_UsuariosClientes obj = model.ObjetoUsuarioCliente;

            obj.ID_ROL = model.idRolSeleccionado;

            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

            int filasAfectadas = metodos.Editar(obj);

            model.Mensaje = "usuario editado con exito!!!!!";

            return RedirectToAction("TablaUsuarios", "Test", model);
        }


        public ActionResult test()
        {
            return View();
        }

    }
}