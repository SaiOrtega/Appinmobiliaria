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
    public class InmueblesController : Controller
    {
        public readonly RepoInmuebles repo = new RepoInmuebles();

        // GET: Inmuebles
        public ActionResult Index()
        {
            var inmueble = repo.ObtenerTodos();
            RepoTipos repoTipo = new RepoTipos();
            RepoUsos repoUso = new RepoUsos();
            ViewBag.tipo = repoTipo.ObtenerTodos();
            ViewBag.uso = repoUso.ObtenerTodos();
            ViewBag.inmuebles = inmueble;
            return View(inmueble);
        }
        public ActionResult IndexProp()
        {
            var inmueble = repo.ObtenerTodosDisponibles();
            RepoTipos repoTipo = new RepoTipos();
            RepoUsos repoUso = new RepoUsos();
            ViewBag.tipo = repoTipo.ObtenerTodos();
            ViewBag.uso = repoUso.ObtenerTodos();
            ViewBag.inmuebles = inmueble;
            return View(inmueble);
        }


        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repo.ObtenerUno(id);
            var propietario = repo.ObtenerUno(id);
            RepoPropietarios repoProp = new RepoPropietarios();
            RepoUsos repoUso = new RepoUsos();
            RepoTipos repoTipo = new RepoTipos();

            ViewBag.propietario = repoProp.ObtenerTodos();
            ViewBag.uso = repoUso.ObtenerTodos();
            ViewBag.tipo = repoTipo.ObtenerTodos();

            return View(inmueble);
        }



        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            RepoPropietarios repoProp = new RepoPropietarios();
            RepoUsos repoUso = new RepoUsos();
            RepoTipos repoTipo = new RepoTipos();

            ViewBag.propietario = repoProp.ObtenerTodos();
            ViewBag.uso = repoUso.ObtenerTodos();
            ViewBag.tipo = repoTipo.ObtenerTodos();
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

                RepoUsos repoUso = new RepoUsos();
                ViewBag.uso = repoUso.ObtenerTodos();

                RepoTipos repoTipo = new RepoTipos();
                ViewBag.tipo = repoTipo.ObtenerTodos();
                return View();
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            var res = repo.ObtenerUno(id);
            RepoPropietarios repoProp = new RepoPropietarios();
            RepoUsos repoUso = new RepoUsos();
            RepoTipos repoTipo = new RepoTipos();

            ViewBag.propietario = repoProp.ObtenerTodos();
            ViewBag.uso = repoUso.ObtenerTodos();
            ViewBag.tipo = repoTipo.ObtenerTodos();

            return View(res);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                repo.Actualizar(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //vuelve al index, los datos hay que vovle a cagarlos?
                return View();
            }
        }
        // GET: Inmuebles/Delete/5
        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var res = repo.ObtenerUno(id);
                RepoPropietarios repoProp = new RepoPropietarios();
                ViewBag.propietario = repoProp.ObtenerTodos();
                RepoUsos repoUso = new RepoUsos();
                ViewBag.uso = repoUso.ObtenerTodos();
                RepoTipos repoTipo = new RepoTipos();
                ViewBag.tipo = repoTipo.ObtenerTodos();
                return View(res);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
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

        [HttpGet]
        public ActionResult Listado(int id)
        {
            var inmuebles = repo.ObtenerTodosInmueblesPropietario(id);
            return View(inmuebles);
        }


    }
}