using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppInmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace AppInmobiliaria.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        public readonly RepoContratos repo = new RepoContratos();

        // GET: Contratos

        public ActionResult Index()
        {
            List<Contrato> contrato = null;

            try
            {
                // contratos = repo.ObtenerTodos();
                contrato = repo.ObtenerTodos();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrio un error al obtener los datos";
            }

            return View(contrato);
        }


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


        //ver porque no trae el ID
        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            Contrato contrato = null;
            try
            {
                contrato = repo.ObtenerUno(id);
                return View(contrato);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrio un error al obtener los datos";
            }

            return View(contrato);
        }



        // GET: Contratos/Create
        public ActionResult Create()
        {
            try
            {

                RepoInmuebles repoInmuebles = new RepoInmuebles();
                RepoInquilinos repoInquilinos = new RepoInquilinos();

                ViewBag.inmuebles = repoInmuebles.ObtenerTodos();
                ViewBag.inquilinos = repoInquilinos.ObtenerTodos();
                return View();

            }
            catch (Exception ex)
            {
                throw;
            }

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

                int cont = repo.TraerContratoCrValido(contrato.FechaInicio, contrato.FechaFinal, contrato.InmuebleId);
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
            try
            {
                var contrato = repo.ObtenerUno(id);
                RepoInmuebles repoInmuebles = new RepoInmuebles();
                RepoInquilinos repoInquilinos = new RepoInquilinos();
                ViewBag.inmuebles = repoInmuebles.ObtenerTodos();
                ViewBag.inquilinos = repoInquilinos.ObtenerTodos();

                return View(contrato);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            var contexVal = new ValidationContext(contrato, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(contrato, contexVal, null, true);
            if (isValid)
            {
                int cont = repo.GetContratoEValidador(contrato.FechaInicio, contrato.FechaFinal, contrato.InmuebleId, contrato.Id);
                if (cont == 0)
                {
                    ModelState.AddModelError("FechaFinal", "El inmueble no está disponible en ese periodo");

                    RepoInmuebles reInmu = new RepoInmuebles();
                    RepoInquilinos reInqui = new RepoInquilinos();

                    ViewBag.inmuebles = reInmu.ObtenerTodos();
                    ViewBag.inquilinos = reInqui.ObtenerTodos();

                    return View(contrato);
                }
                else
                {

                    try
                    {
                        // TODO: Add update logic here
                        int res = repo.Actualizar(contrato);
                        return RedirectToAction(nameof(Index));
                    }
                    catch
                    {
                        return View();
                    }

                }
            }
            else
            {
                RepoInmuebles reInmu = new RepoInmuebles();
                RepoInquilinos reInqui = new RepoInquilinos();

                ViewBag.inmuebles = reInmu.ObtenerTodos();
                ViewBag.inquilinos = reInqui.ObtenerTodos();

                return View(contrato);
            }

        }


        // GET: Contratos/Delete/5

        public ActionResult Delete(int id)
        {

            try
            {

                var res = repo.ObtenerUno(id);

                return View();

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                // TODO: Add delete logic here

                repo.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //GET
        public ActionResult Renovar(int id)
        {

            RepoInmuebles repoInmu = new RepoInmuebles();
            RepoInquilinos repoInq = new RepoInquilinos();
            Contrato contrato = repo.ObtenerUno(id);
            decimal deuda = repo.SumaPagos(id);
            contrato.FechaInicio = contrato.FechaFinal.HasValue ? contrato.FechaFinal.Value.AddDays(1) :
            DateTime.Now;
            contrato.FechaFinal = null;
            ViewBag.inmuebles = repoInmu.ObtenerUno(contrato.InmuebleId.Value);
            ViewBag.inquilinos = repoInq.ObtenerUno(contrato.InquilinoId.Value);
            ViewBag.deuda = deuda - contrato.MontoMensual;
            return View(contrato);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST:
        public ActionResult Renovar(Contrato contrato)
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
                    ModelState.AddModelError("FechaFinal", "El inmueble no está disponible en ese periodo");

                    RepoInmuebles repoInmu = new RepoInmuebles();
                    RepoInquilinos repoInq = new RepoInquilinos();
                    ViewBag.inmuebles = repoInmu.ObtenerTodos();
                    ViewBag.inquilinos = repoInq.ObtenerTodos();

                    return View();

                }

            }
            else
            {
                RepoInmuebles repoInmu = new RepoInmuebles();
                RepoInquilinos repoInq = new RepoInquilinos();
                ViewBag.inmuebles = repoInmu.ObtenerTodos();
                ViewBag.inquilinos = repoInq.ObtenerTodos();

                return View();

            }

        }

        public ActionResult Vencidos()
        {
            var contratos = repo.ObtenerTodosVencidos();
            return View(contratos);

        }

        public ActionResult Listado(int id)
        {
            var contratos = repo.ObtenerTodosContratos(id);
            return View(contratos);
        }

    }

}