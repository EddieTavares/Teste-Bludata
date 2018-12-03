using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesteCSharp_Ednilson.Models;

namespace TesteCSharp_Ednilson.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            try
            {
                ViewBag.EmpresaCnpj = db.Empresa.FirstOrDefault().Cnpj;
            }
            catch (Exception)
            {
                ViewBag.EmpresaCnpj = string.Empty;
            }
            return View();
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