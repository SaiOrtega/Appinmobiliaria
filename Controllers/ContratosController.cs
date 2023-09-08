using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppInmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace AppInmobiliaria.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        public readonly RepoContratos repo = new RepoContratos();

        // GET: Contratos
        public ActionResult Index()
        {
            List<Contrato> contratos = null;

            try
            {
                contratos = repo.ObtenerTodos();

            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "Ocurrio un error al obtener los datos";
            }
            // if (contratos != null || contratos.Count == 0)
            // {
            //     ViewBag.NoDataMessage = "No se encontraron contratos en Base";
            // }

            return View(contratos);
        }

        //deberia separar a los vigentes y los vencidos?

        public ActionResult Vigentes()
        {
            List<Contrato> contratos = null;
            try
            {
                contratos = repo.ObtenerTodosVigentes();

            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "Ocurrio un error al obtener los datos";
            }
            if (contratos != null || contratos.Count == 0)
            {
                ViewBag.NoDataMessage = "No se encontraron contratos en Base";
            }

            return View(contratos);
        }



        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            var contrato = repo.ObtenerUno(id);

            return View(contrato);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            RepoInmuebles repoInmuebles = new RepoInmuebles();
            RepoInquilinos repoInquilinos = new RepoInquilinos();

            ViewBag.inmuebles = repoInmuebles.ObtenerTodos();
            ViewBag.inquilinos = repoInquilinos.ObtenerTodos();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        // public ActionResult Create(Contrato contrato)
        // {
        //     int res = repo.Alta(contrato);
        //     return RedirectToAction(nameof(Index));
        // }

        public ActionResult Create(Contrato contrato)
        {
            var contx = new ValidationContext(contrato, serviceProvider: null, items: null);

            var isValid = Validator.TryValidateObject(contrato, contx, null, true);

            if (isValid)
            {

                int cont = repo.verificarPosibilidad(contrato.FechaInicio, contrato.FechaFinal, contrato.InmuebleId);
                if (cont == 0)
                {

                    try
                    {
                        int res = repo.Alta(contrato);
                        return RedirectToAction(nameof(Index));
                    }
                    catch
                    {
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("FechaFinal", "El inmueble no está disponilbe para esa fecha");
                    RepoInmuebles repoInmuebles = new RepoInmuebles();
                    RepoInquilinos repoInquilinos = new RepoInquilinos();
                    ViewBag.inmuebles = repoInmuebles.ObtenerTodos();
                    ViewBag.inquilinos = repoInquilinos.ObtenerTodos();
                    return View();

                }

            }
            else
            {
                RepoInmuebles repoInmuebles = new RepoInmuebles();
                RepoInquilinos repoInquilinos = new RepoInquilinos();
                ViewBag.inmuebles = repoInmuebles.ObtenerTodos();
                ViewBag.inquilinos = repoInquilinos.ObtenerTodos();
                return View();

            }


        }


        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
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