using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class MarcasController : Controller
    {
        //listar Marcas para comboBox
        public JsonResult Listar()
        {
            AccesoDatos.Marcas metodos = new AccesoDatos.Marcas();
            var lista = metodos.Listar();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }


    }
}