using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AppInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Authentication;


namespace AppInmobiliaria.Controllers
{
    [Authorize]

    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }
        public readonly RepoUsuarios repoUsuarios = new RepoUsuarios();

        // GET: Usuarios


        public ActionResult Index()
        {
            if (User.IsInRole("Administrador"))
            {
                var usuarios = repoUsuarios.ObtenerTodos();
                return View(usuarios);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            var e = repoUsuarios.ObtenerPorId(id);
            return View(e);
        }

        [AllowAnonymous]
        //[Authorize(Policy = "Administrador")]
        public ActionResult Create()

        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }


        // POST: Usuarios/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: usuario.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                usuario.Clave = hashed;
                //usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = repoUsuarios.Alta(usuario);
                if (usuario.AvatarFile != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(usuario.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                    repoUsuarios.Modificacion(usuario);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CambiarContrasenia(int idUs, String NContra, String NControl, String UCVieja, String Clave)
        {
            var vista = nameof(Edit);
            TempData["error"] = "a";

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: UCVieja,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
            UCVieja = hashed;

            if (UCVieja == Clave)
            {
                if (NContra == NControl)
                {

                    hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                               password: NContra,
                               salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                               prf: KeyDerivationPrf.HMACSHA1,
                               iterationCount: 1000,
                               numBytesRequested: 256 / 8));
                    NContra = hashed;

                    TempData["error"] = "Contraseña actualizada";
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    repoUsuarios.ActualizarContraseña(idUs, NContra);
                    Usuario u = repoUsuarios.ObtenerPorId(idUs);
                    return RedirectToAction(vista, new { Id = idUs });

                }
                else
                {
                    TempData["error"] = "Las contraseñas no coinciden";
                    ViewBag.Roles = Usuario.ObtenerRoles();

                    Usuario u = repoUsuarios.ObtenerPorId(idUs);
                    return RedirectToAction(vista, new { Id = idUs });
                }

            }
            else
            {
                TempData["error"] = "La contraseña actual no es correcta";
                ViewBag.Roles = Usuario.ObtenerRoles();
                Usuario u = repoUsuarios.ObtenerPorId(idUs);
                return RedirectToAction(vista, new { Id = idUs });
            }
        }


        // GET: Usuarios/Edit/5       
        //[Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var usuarioActual = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
            if (usuarioActual == null || usuarioActual.Id != id)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            var u = repoUsuarios.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }


        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario u)
        {
            var vista = nameof(Edit);//de que vista provengo
            try
            {
                var usuarioActual = repoUsuarios.ObtenerPorEmail(User.Identity.Name);

                if (!User.IsInRole("Administrador"))//no soy admin
                {
                    vista = nameof(Perfil);//solo puedo ver mi perfil                   
                    //u.RolId = usuarioActual.RolId;
                    if (usuarioActual.Id != id)//si no es admin, solo puede modificarse él mismo
                        return RedirectToAction(nameof(Index), "Home");
                }
                if (ModelState.IsValid)
                {
                    // TODO: Add update logic here
                    if (u.AvatarFile != null)
                    {
                        if (u.Avatar != null)
                        {
                            string avatarPath = Path.Combine(environment.WebRootPath, u.Avatar.TrimStart('/')); // construir la ruta adecuada agregando wwwRootPath y eliminando el primer slash en u.Avatar
                            System.IO.File.Delete(avatarPath);
                            using (FileStream stream = new FileStream(avatarPath, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }

                        }
                        else
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "Uploads");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            //Path.GetFileName(usuario.AvatarFile.FileName);//este nombre se puede repetir
                            string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.Avatar = Path.Combine("/Uploads", fileName);
                            // Esta operación guarda la foto en memoria en la ruta que necesitamos
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }
                        }


                    }

                    /*string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: u.Clave,
                                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                    prf: KeyDerivationPrf.HMACSHA1,
                                    iterationCount: 1000,
                                    numBytesRequested: 256 / 8));
                    u.Clave = hashed;*/
                    repoUsuarios.Modificacion(u);

                    return RedirectToAction(vista);
                }
                else
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        TempData["error"] = error.ErrorMessage;
                    }
                    return View(u);

                }
            }
            catch (Exception ex)
            {//colocar breakpoints en la siguiente línea por si algo falla
                throw;
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {

            if (User.IsInRole("Adminstrador"))
            {
                var u = repoUsuarios.ObtenerPorId(id);
                return View(u);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario us)
        {
            var usuario = repoUsuarios.ObtenerPorId(id);
            try
            {
                // TODO: Add delete logic here
                var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        //[Authorize]
        public IActionResult Avatar()
        {
            var u = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
            string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            string pathCompleto = Path.Combine(path, fileName);

            //leer el archivo
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
            //devolverlo
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //[Authorize]
        public string AvatarBase64()
        {
            var u = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
            string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            string pathCompleto = Path.Combine(path, fileName);

            //leer el archivo
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
            //devolverlo
            return Convert.ToBase64String(fileBytes);
        }

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult LoginModal()
        {
            return PartialView("_LoginModal", new LoginView());
        }

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repoUsuarios.ObtenerPorEmail(login.Usuario);
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }


}