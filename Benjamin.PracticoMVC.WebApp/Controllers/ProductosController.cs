using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class ProductosController : Controller
    {
        // ABM de productos como administrador
        public ActionResult ABM()
        {
            return View();
        }

        public ActionResult Cards()
        {
            return View();
        }

        public ActionResult Cards2()
        {
            return View();
        }






        //Json que obtiene la lista de todos los producto
        public JsonResult Listar()
        {

            AccesoDatos.Productos metodos = new AccesoDatos.Productos();

            var lista = metodos.Listar();

            return Json(lista, JsonRequestBehavior.AllowGet);

        }



        //Json que obtiene la lista de todos los producto activos
        //para mostrar a los clientes en forma de cards
        public JsonResult ListarCards()
        {

            AccesoDatos.Productos metodos = new AccesoDatos.Productos();

            var lista = metodos.ListarCards();

            return Json(lista, JsonRequestBehavior.AllowGet);

        }


        //OBTENER EL REGISTRO DEL PRODUCTO POR SU ID
        public JsonResult Detalle(int id)
        {
            AccesoDatos.Productos metodos = new AccesoDatos.Productos();

            Entidades.Productos obj = metodos.Detalle(id);

            return Json(obj, JsonRequestBehavior.AllowGet);
        }



        public int Guardar(Entidades.Productos obj)
        {
            int retorno = -1;

            //si el ID es cero agregar
            if (obj.Codigo == 0)
            {
                AccesoDatos.Productos metodos = new AccesoDatos.Productos();

                metodos.Crear(obj);

                retorno = 1;
            }
            else // si el ID es distinto de cero editar
            {
                AccesoDatos.Productos metodos = new AccesoDatos.Productos();

                int filasAfectadas = metodos.Editar(obj);
                //si hay una fila afectada(actualizada) retornamos 2
                if (filasAfectadas == 1)
                {
                    retorno = 2;
                }

            }

            return retorno;
        }


    }
}