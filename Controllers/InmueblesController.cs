using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppInmobiliaria.Models;

namespace AppInmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        RepoInmuebles repo = new RepoInmuebles();

        // GET: Inmuebles
        public ActionResult Index()
        {
            var inmueble = repo.ObtenerTodos();
            return View(inmueble);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repo.ObtenerUno(id);
            RepoPropietarios repoProp = new RepoPropietarios();
            ViewBag.propietario = repoProp.ObtenerUno(id);

            return View(inmueble);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            RepoPropietarios repoProp = new RepoPropietarios();
            ViewBag.propietario = repoProp.ObtenerTodos();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repo.Alta(inmueble);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                RepoPropietarios repoProp = new RepoPropietarios();
                ViewBag.propietario = repoProp.ObtenerTodos();
                return View();
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            var res = repo.ObtenerUno(id);
            RepoPropietarios repoProp = new RepoPropietarios();
            ViewBag.propietario = repoProp.ObtenerTodos();

            return View(res);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                var res = repo.Actualizar(inmueble);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                RepoPropietarios repoProp = new RepoPropietarios();
                ViewBag.propietario = repoProp.ObtenerTodos();
                return View();
            }
        }

        // GET: Inmuebles/Delete/5
        public ActionResult Delete(int id)
        {
            var res = repo.ObtenerUno(id);
            RepoPropietarios repoProp = new RepoPropietarios();
            ViewBag.propietario = repoProp.ObtenerTodos();
            return View(res);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                repo.Eliminar(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}