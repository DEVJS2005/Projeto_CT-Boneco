using Projeto_CT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_CT.Controllers
{
    public class LoginController : Controller
    {
        BDEntities bd = new BDEntities();
        // GET: telaLogin
        public ActionResult telaLogin()
        {
            return View();
        }
        public ActionResult login(string usuarioL, string senhaL)
        {
            usuario user = bd.usuario.ToList().Find(x => x.nome.Equals(usuarioL));
            if (user == null)
            {
                return View(ViewBag.text = "Usuário inexistente");
            }
            if (usuarioL == user.nome && senhaL == user.senha)
            {
                Session["UserAuth"] = user;
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult logout()
        {
            Session["UserAuth"] = null;
            return RedirectToAction("telaLogin", "Login");
        }
    }
}