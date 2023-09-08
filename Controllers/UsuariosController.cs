using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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
            var usuarios = repoUsuarios.ObtenerTodos();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            var empleado = repoUsuarios.ObtenerUno(id);
            return View(empleado);
        }

        // GET: Usuarios/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.rol = Usuario.traerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

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
                if (usuario.Avatar != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(usuario.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.FormFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.FormFile.CopyTo(stream);
                    }
                    repoUsuarios.Actualizar(usuario);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Roles = Usuario.traerRoles();
                return View();
            }
        }
        [Authorize]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var user = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.traerRoles();
            return View(user);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var user = repoUsuarios.ObtenerUno(id);
            ViewBag.Roles = Usuario.traerRoles();
            return View(user);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario usuario)
        {
            var view = nameof(Edit);
            try
            {
                if (!User.IsInRole("Adminmistrador"))
                {
                    view = nameof(Perfil);
                    var user = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
                    if (user.Id != id)
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }

                }
                if (usuario.FormFile != null)
                {
                    if (usuario.Avatar != null)
                    {
                        string avatarPath = Path.Combine(environment.WebRootPath, usuario.Avatar.TrimStart('/'));
                        System.IO.File.Delete(avatarPath);
                        using (FileStream stream = new FileStream(avatarPath, FileMode.Create))
                        {
                            usuario.FormFile.CopyTo(stream);
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
                        //Path.GetFileName(usuario.FormFile.FaileName);//este nombre se puede repetir
                        string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.FormFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/Uploads", fileName);
                        // Esta operación guarda la foto en memoria en la ruta que necesitamos
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.FormFile.CopyTo(stream);
                        }

                    }
                }
                repoUsuarios.Actualizar(usuario);
                return RedirectToAction(view);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{usuario.Id}");
                Path.GetExtension(usuario.Avatar);
                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Avatar()
        {
            var user = repoUsuarios.ObtenerPorEmail(User.Identity.Name);

            string fileName = "avatar_" + user.Id + Path.GetExtension(user.Avatar);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            string pathCompleto = Path.Combine(path, fileName);
            byte[] filebytes = System.IO.File.ReadAllBytes(pathCompleto);

            return File(filebytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public string AvarBase64()
        {
            var user = repoUsuarios.ObtenerPorEmail(User.Identity.Name);
            string fileName = "avatar_" + user.Id + Path.GetExtension(user.Avatar);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            string pathCompleto = Path.Combine(path, fileName);
            byte[] filebytes = System.IO.File.ReadAllBytes(pathCompleto);
            return Convert.ToBase64String(filebytes);
        }

        [AllowAnonymous]
        public ActionResult LoginEstilo()
        {
            return PartialView("_LoginEstilo", new LoginView());
        }

        [AllowAnonymous]
        public ActionResult Login(String url)
        {
            TempData["url"] = url;
            return View();
        }




        [Authorize]
        [HttpPost]
        public ActionResult cambiarPassword(int userId, String newPassword, String nControl, String oldPassword, String clave)
        {
            var view = nameof(Edit);
            TempData["error"] = "a";

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: oldPassword,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));

            oldPassword = hashed;

            if (oldPassword == clave)
            {
                if (newPassword == nControl)
                {
                    hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: newPassword,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    newPassword = hashed;

                    TempData["error"] = "Contraseña cambiada";
                    ViewBag.Roles = Usuario.traerRoles();
                    repoUsuarios.ActualizarContrasenia(userId, newPassword);
                    Usuario user = repoUsuarios.ObtenerUno(userId);
                    return RedirectToAction(view, new { id = userId });

                }
                else
                {
                    TempData["error"] = "Las contraseñas no coinciden";
                    ViewBag.Roles = Usuario.traerRoles();
                    Usuario user = repoUsuarios.ObtenerUno(userId);
                    return RedirectToAction(view, new { id = userId });
                }
            }
            else
            {
                TempData["error"] = "Contraseña incorrecta";
                ViewBag.Roles = Usuario.traerRoles();
                Usuario user = repoUsuarios.ObtenerUno(userId);
                return RedirectToAction(view, new { id = userId });
            }

        }

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
                        new Claim(ClaimTypes.Role, e.rolNombre),
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

        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}