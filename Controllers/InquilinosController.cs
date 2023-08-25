using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppInmobiliaria.Models;

namespace AppInmobiliaria.Controllers
{
    public class InquilinosController : Controller
    {
        RepoInquilinos repo = new RepoInquilinos();

        // GET: Inquilinos
        public ActionResult Index()
        {
            var inquilino = repo.ObtenerTodos();
            return View(inquilino);
        }

        // GET: Inquilinos/Details/5
        public ActionResult Details(int id)
        {
            var inquilino = repo.ObtenerUno(id);

            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {

            try
            {
                // TODO: Add insert logic here
                int res = repo.Alta(inquilino);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {
            var res = repo.ObtenerUno(id);

            return View(res);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {

            try
            {
                var res = repo.Actualizar(inquilino);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inquilino)
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