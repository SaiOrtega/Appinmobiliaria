using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppInmobiliaria.Controllers
{
    [Authorize]
    public class PropietariosController : Controller
    {
        RepoPropietarios repo = new RepoPropietarios();

        // GET: Propietarios
        public ActionResult Index()
        {
            var propietario = repo.ObtenerTodos();
            return View(propietario);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            var inquilino = repo.ObtenerUno(id);

            return View(inquilino);
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repo.Alta(propietario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
            var res = repo.ObtenerUno(id);

            return View(res);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                // TODO: Add update logic here
                var res = repo.Actualizar(propietario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Delete/5

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var res = repo.ObtenerUno(id);
            return View(res);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                repo.Eliminar(id);
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}



