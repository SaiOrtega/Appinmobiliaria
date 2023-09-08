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
    public class PagosController : Controller
    {
        public readonly RepoPagos repo = new RepoPagos();
        // GET: Pagos
        public ActionResult Index(int? id)
        {


            if (id != null)
            {
                var pagos = repo.ObtenerTodos(id);
                ViewBag.id = id;
                return View(pagos);
            }
            else
            {
                var pagos = repo.ObtenerTodos(id);
                return View(pagos);
            }
        }

        // GET: Pagos/Details/5


        // GET: Pagos/Create
        public ActionResult Create()
        {
            RepoContratos repoContratos = new RepoContratos();
            ViewBag.contratos = repoContratos.ObtenerTodos();

            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            int? idContrato = pago.contratoId;
            try
            {
                int res = repo.Alta(pago);

                return RedirectToAction("Index", new { id = idContrato });
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Edit/5
        public ActionResult Edit(int id)
        {
            RepoContratos repoContratos = new RepoContratos();

            ViewBag.contratos = repoContratos.ObtenerTodos();
            var contrato = repo.ObtenerUno(id);

            return View(contrato);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pago pago)
        {
            try
            {
                repo.Actualizar(pago);

                return RedirectToAction("Index", new { id = pago.contratoId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Pago pago = repo.ObtenerUno(id);
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                repo.Eliminar(id);

                return RedirectToAction("Index", new { id = pago.contratoId });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult efectuarPago(int id)
        {

            RepoContratos repoContratos = new RepoContratos();

            Contrato contrato = repoContratos.ObtenerUno(id);
            ViewBag.montoM = contrato.MontoMensual;
            ViewBag.contratoId = id;
            return View();
        }
    }
}