using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TesteCSharp_Ednilson.Models;
using TesteCSharp_Ednilson.Utils;

namespace TesteCSharp_Ednilson.Controllers
{
    public class EmpresaController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Empresa
        public ActionResult Index()
        {
            var listEmpresas = db.Empresa.ToList();

            foreach (var item in listEmpresas)
                item.UF = Utils.Utils.GetEstado(item.UF);

            return View(db.Empresa.ToList());
        }

        // GET: Empresa/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empresa empresa = db.Empresa.Find(id);

            if (empresa == null)
                return HttpNotFound();

            empresa.UF = Utils.Utils.GetEstado(empresa.UF);

            return View(empresa);
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            var _empresa = new EmpresaViewModel() {
                Estados = new SelectList(Utils.Utils.DtbEstados().AsDataView(), "Uf", "Estado"),
            };

            return View("Create", _empresa);
        }

        // POST: Empresa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cnpj,NomeFantasia,UF")] EmpresaViewModel _empresa)
        {
            if (ModelState.IsValid)
            {
                var empresa = (Empresa)_empresa;

                empresa.Cnpj = Regex.Replace(empresa.Cnpj, "[^0-9,]", "");

                if (db.Empresa.Any(e => e.Cnpj.Equals(empresa.Cnpj)))
                {
                    ModelState.AddModelError("Cnpj", "Cnpj já cadastrado.");

                    _empresa.Estados = new SelectList(Utils.Utils.DtbEstados().AsDataView(), "Uf", "Estado");
                    return View("Create", _empresa);
                }

                db.Empresa.Add(empresa);
                db.SaveChanges();
                return Content("Success");
            }

            _empresa.Estados = new SelectList(Utils.Utils.DtbEstados().AsDataView(), "Uf", "Estado");
            return View("Create", _empresa);
        }

        // GET: Empresa/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var empresa = (EmpresaViewModel)db.Empresa.Find(id);
            if (empresa == null)
                return HttpNotFound();

            empresa.Estados = new SelectList(Utils.Utils.DtbEstados().AsDataView(), "Uf", "Estado", empresa.UF);
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cnpj,NomeFantasia,UF")] EmpresaViewModel empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.Cnpj = Regex.Replace(empresa.Cnpj, "[^0-9,]", "");

                db.Entry((Empresa)empresa).State = EntityState.Modified;
                db.SaveChanges();
                return Content("Success");
            }

            empresa.Estados = new SelectList(Utils.Utils.DtbEstados().AsDataView(), "Uf", "Estado", empresa.UF);
            return View(empresa);
        }

        // GET: Empresa/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
                return HttpNotFound();

            empresa.UF = Utils.Utils.GetEstado(empresa.UF);

            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Cnpj)
        {
            try
            {
                Cnpj = Regex.Replace(Cnpj, "[^0-9,]", "");

                Empresa empresa = db.Empresa.Find(Cnpj);
                db.Empresa.Remove(empresa);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var innerException = e.InnerException.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 547))
                    ModelState.AddModelError("", "A empresa possui fornecedores cadastrados, exclua-os para excluir a empresa.");
                else
                    ModelState.AddModelError("", e.Message);

                Empresa empresa = db.Empresa.Find(Cnpj);
                return View("Delete", empresa);
            }
            return Content("Success");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
