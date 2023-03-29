using Projeto_CT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_CT.Controllers
{
    public class HomeController : Controller
    {
        public bool VerificarLogin()
        {
            if (Session["UserAuth"] != null)
            {
                return true;
            }
            return false;
        }
        BDEntities bd = new BDEntities();
        public ActionResult Index()
        {
            if(VerificarLogin()){
                return View((usuario)Session["UserAuth"]);
            }
            else
            {
                return RedirectToAction("telaLogin","Login");
            }
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}