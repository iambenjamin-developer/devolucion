using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        // pagina principal de Bienvenida
        public ActionResult Index()
        {
            TimeSpan tiempoActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            int horaActual = tiempoActual.Hours;

            string periodo = string.Empty;

            if (horaActual >= 6 && horaActual <= 12)
            {
                periodo = "Buenos días";
            }
            else if (horaActual >= 13 && horaActual <= 19)
            {
                periodo = "Buenas tardes";
            }
            else if ((horaActual >= 20 && horaActual <= 24) || (horaActual >= 0 && horaActual <= 5))
            {
                periodo = "Buenas noches";
            }

            ViewBag.periodo = periodo;


            return View();
        }

        //Vista Login
        public ActionResult Login()
        {
            return View();
        }


        //Vista Cambiar
        public ActionResult CambiarClave()
        {
            return View();
        }


        //Codigo para chequear si loguea correctamente y ademas
        //se verifica que si el usuario y la contraseña es la misma
        //se debe realizar un cambio de clave en el primer login
        public int CodigoLogin(Entidades.Login obj)
        {
            bool usuarioClaveIguales;

            if (obj.USUARIO == obj.CLAVE)
            {
                usuarioClaveIguales = true;
            }
            else
            {

                usuarioClaveIguales = false;
            }

            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();


            if (metodos.ValidarLogin(obj.USUARIO, obj.CLAVE) == true)
            {

                Entidades.Sesion objSesion = metodos.ObtenerUsuarioSesion(obj.USUARIO);

                //dejar usuario de sesion logueado
                objSesion.ONLINE = true;
                Session["ID_USUARIO"] = objSesion.ID_USUARIO;
                Session["USERNAME"] = objSesion.USERNAME;
                Session["ID_ROL"] = objSesion.ID_ROL;
                Session["ROL_DESCRIPCION"] = objSesion.ROL_DESCRIPCION;
                Session["ID_CLIENTE"] = objSesion.ID_CLIENTE;
                Session["NOMBRES"] = objSesion.NOMBRES;
                Session["APELLIDOS"] = objSesion.APELLIDOS;
                Session["ONLINE"] = objSesion.ONLINE;
                Session["ESTADO_PEDIDO"] = "SIN_PEDIDOS";

              
                //si el usuario y la clave son iguales, significa que esta blanqueada
                //por lo cual hay q redirigirlos a cambiar contraseña
                if (usuarioClaveIguales == true)
                {
                    return 2;
                }
                else
                {// de lo contrario si valida ok, pero son distintos user/pass ingresa normal al sistema
                    return 1;
                }


            }
            else
            {// si el codigo es cero es que no se pudo concretar el logueo
                return 0;
            }

        }


        //antes de cambiar la clave chequear si cuando se reingresa la clave actual
        //esta escrita correctamente
        public int ValidarClaveActual(string claveActual)
        {


            Entidades.Login obj = new Entidades.Login();
            obj.USUARIO = Session["USERNAME"].ToString();

            obj.CLAVE = claveActual;
            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();


            int codigo = metodos.ValidarClaveActual(obj.USUARIO, obj.CLAVE);
            //si el codigo da 1 es porque se valida el usuario ok
            return codigo;
        }


        //ya pasando todas las anteriores verificaciones 
        //se procede a cambiar la clave
        public int CodigoCambiarClave(Entidades.Login obj)
        {

            obj.USUARIO = Session["USERNAME"].ToString();

            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

            int codigo = metodos.ActualizarPassword(obj.USUARIO, obj.CLAVE);

            return codigo;
        }


        public int CodigoResetearClave(int idUsuario)
        {
            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

            int codigoResetClave = metodos.ResetearClave(idUsuario);


            return codigoResetClave;

        }



        public string ObtenerIdRol()
        {
            string idRol = string.Empty;
            idRol = Session["ID_ROL"].ToString();
            return idRol;
        }




        public JsonResult Listar()
        {
            AccesoDatos.Usuarios obj = new AccesoDatos.Usuarios();
            var lista = obj.Listar();


            return Json(lista, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Detalle(int id)
        {
            AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

            var userSeleccionado = metodos.Detalle(id);

            return Json(userSeleccionado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ABM()
        {

            return View();
        }


        public int Guardar3(Entidades.Usuarios obj)
        {
            int retorno = -1;



            //si el ID es cero agregar
            if (obj.Id == 0)
            {
                AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

                //metodos.Crear(obj);

                retorno = 1;
            }
            else // si el ID es distinto de cero editar
            {
                AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

                //int filasAfectadas = metodos.Editar(obj);
                //si hay una fila afectada(actualizada) retornamos 2
                //if (filasAfectadas == 1)
                //{
                //    retorno = 2;
                //}

            }

            return retorno;
        }


        public int Guardar(Entidades.Join_UsuariosClientes obj_Usuario_Cliente)
        {
            int retorno = -1;



            //si el ID_USUARIO es cero agregar
            if (obj_Usuario_Cliente.ID_USUARIO == 0)
            {
               AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

                retorno = metodos.Crear(obj_Usuario_Cliente);

            }
            else // si el ID_USUARIO es distinto de cero editar
            {
                AccesoDatos.Usuarios metodos = new AccesoDatos.Usuarios();

                int filasAfectadas = metodos.Editar(obj_Usuario_Cliente);
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