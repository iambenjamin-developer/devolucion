using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Benjamin.PracticoMVC.WebApp.Controllers
{
    public class PruebaController : Controller
    {

        [HttpGet]
        public ActionResult ObtenerPersonas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerPersonas(Entidades.Usuarios obj)
        {

            ViewBag.mensaje = "exitoso";


            return View(obj);
        }



        [HttpPost]
        public ActionResult ObtenerNumero(int edad)
        {


            return View();        
        }


        // GET: Prueba
        public ActionResult Index()
        {
            return View();
        }

        // GET: Prueba/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Prueba/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prueba/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Prueba/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Prueba/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Prueba/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Prueba/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
