using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult List(Models.Test.TestModel modelParametro)
        {

            var model = new Models.Test.TestModel();

            if (modelParametro.Mensaje != null)
            {
                model.Mensaje = modelParametro.Mensaje;
            }

            AccesoDatos.Test metodos = new AccesoDatos.Test();

            List<Entidades.Join_UsuariosRoles> listaUyR = metodos.ListarUsuariosRoles();



            model.ListaDeUsuariosyRoles = listaUyR;

            return View(model);


        }
    }
}