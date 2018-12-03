using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesteCSharp_Ednilson.Models;

namespace TesteCSharp_Ednilson.Controllers
{
    public class EmpresaController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Empresa/Edit/5
        public ActionResult Edit()
        {
            Empresa empresa = db.Empresa.FirstOrDefault();
            
            if (empresa == null)
            {
                ViewBag.create = true;
                ViewBag.UF = new SelectList(ListaEstados().AsDataView(), "Uf", "Estado");
                return View(new Empresa());
            }

            ViewBag.UF = new SelectList(ListaEstados().AsDataView(), "Uf", "Estado", empresa.UF);
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cnpj,UF,NomeFantasia")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _empresa = db.Empresa.Where(x => x.Cnpj == empresa.Cnpj).FirstOrDefault();

                    if (_empresa == null)
                    {
                        if (!Utils.IsCnpj(empresa.Cnpj))
                            return Json(new { Rc = 9, Message = "Cpf inválido" });

                        db.Empresa.Add(empresa);
                        db.SaveChanges();
                    }
                    else
                    {
                        _empresa.UF = empresa.UF;
                        _empresa.NomeFantasia = empresa.NomeFantasia;

                        db.SaveChanges();
                    }

                    ViewBag.UF = new SelectList(ListaEstados().AsDataView(), "Uf", "Estado", empresa.UF);
                    return View();
                }
                catch (Exception e)
                {
                    return View();
                }
            }

            ViewBag.UF = new SelectList(ListaEstados().AsDataView(), "Uf", "Estado");
            return View(empresa);
        }

        public DataTable ListaEstados()
        {
            var dtEstados = new DataTable();
            dtEstados.Columns.Add("Estado");
            dtEstados.Columns.Add("UF");

            dtEstados.Rows.Add("Acre", "AC");
            dtEstados.Rows.Add("Alagoas", "AL");
            dtEstados.Rows.Add("Amapá", "AP");
            dtEstados.Rows.Add("Amazonas", "AM");
            dtEstados.Rows.Add("Bahia", "BA");
            dtEstados.Rows.Add("Ceará", "CE");
            dtEstados.Rows.Add("Distrito Federal", "DF");
            dtEstados.Rows.Add("Espírito Santo", "ES");
            dtEstados.Rows.Add("Goiás", "GO");
            dtEstados.Rows.Add("Maranhão", "MA");
            dtEstados.Rows.Add("Mato Grosso", "MT");
            dtEstados.Rows.Add("Mato Grosso do Sul", "MS");
            dtEstados.Rows.Add("Minas Gerais", "MG");
            dtEstados.Rows.Add("Pará", "PA");
            dtEstados.Rows.Add("Paraíba", "PB");
            dtEstados.Rows.Add("Paraná", "PR");
            dtEstados.Rows.Add("Pernambuco", "PE");
            dtEstados.Rows.Add("Piauí", "PI");
            dtEstados.Rows.Add("Rio de Janeiro", "RJ");
            dtEstados.Rows.Add("Rio Grande do Norte", "RN");
            dtEstados.Rows.Add("Rio Grande do Sul", "RS");
            dtEstados.Rows.Add("Rondônia", "RO");
            dtEstados.Rows.Add("Roraima", "RR");
            dtEstados.Rows.Add("Santa Catarina", "SC");
            dtEstados.Rows.Add("São Paulo", "SP");
            dtEstados.Rows.Add("Sergipe", "SE");
            dtEstados.Rows.Add("Tocantins", "TO");

            return dtEstados;
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
    