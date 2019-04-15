using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TesteCSharp_Ednilson.Models;
using TesteCSharp_Ednilson.Utils;

namespace TesteCSharp_Ednilson.Controllers
{
    public class FornecedorController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Fornecedor
        public ActionResult Index()
        {
            return View();
        }

        // GET: Fornecedor/GetLista
        public ActionResult GetLista(
            string FiltroNome,
            string FiltroCpfCnpj,
            string FiltroDataInicial,
            string FiltroDataFinal)
        {
            var fornecedor = db.Fornecedor.Include(f => f.Empresa);

            if (!string.IsNullOrWhiteSpace(FiltroNome))
                fornecedor = fornecedor.Where(x => x.Nome.Contains(FiltroNome));

            FiltroCpfCnpj = Regex.Replace(FiltroCpfCnpj, "[^0-9,]", "");

            if (!string.IsNullOrWhiteSpace(FiltroCpfCnpj))
                fornecedor = fornecedor.Where(x => x.Cpf_Cnpj.Contains(FiltroCpfCnpj));

            if (!string.IsNullOrWhiteSpace(FiltroDataInicial)) {
                var _FiltroDataInicial = Convert.ToDateTime(FiltroDataInicial).AddSeconds(1);
                fornecedor = fornecedor.Where(x => x.DataCadastro >= _FiltroDataInicial);
            }

            if (!string.IsNullOrWhiteSpace(FiltroDataFinal)) {
                var _FiltroDataFinal = Convert.ToDateTime(FiltroDataFinal).AddDays(1).AddSeconds(-1);
                fornecedor = fornecedor.Where(x => x.DataCadastro <= _FiltroDataFinal);
            }

            var _fornecedores = fornecedor.ToList();
            
            return View("Lista", _fornecedores);
        }

        // GET: Fornecedor/Create
        public ActionResult Create()
        {
            var fornecedor = new FornecedorViewModel() {
                ListEmpresas = new SelectList(db.Empresa, "Cnpj", "NomeFantasia"),
                DataNascimento = DateTime.Now.AddYears(-18)
            };
            
            return View("Create", fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FornecedorViewModel fornecedor)
        {
            if (ModelState.IsValid)
            {
                // Validacoes
                if (fornecedor.Tipo_Pessoa == "F" && !Utils.Utils.IsCpf(fornecedor.Cpf_Cnpj))
                    ModelState.AddModelError("Cpf_Cnpj", "Cpf inválido.");

                if (fornecedor.Tipo_Pessoa == "J" && !Utils.Utils.IsCnpj(fornecedor.Cpf_Cnpj))
                    ModelState.AddModelError("Cpf_Cnpj", "Cnpj inválido.");

                if (fornecedor.Tipo_Pessoa == "F" && fornecedor.Rg == string.Empty)
                    ModelState.AddModelError("Rg", "Rg inválido.");

                if (fornecedor.Tipo_Pessoa == "F")
                {
                    var empresa = db.Empresa.Where(x => x.Cnpj == fornecedor.Empresa_Cnpj).FirstOrDefault();

                    // validacao de idade para empresas do Parana
                    if (empresa != null && empresa.UF == "PR")
                    {
                        var hoje = DateTime.Now;

                        var idade = hoje.Year - fornecedor.DataNascimento.Year;
                        if (fornecedor.DataNascimento > hoje.AddYears(-idade))
                            idade--;

                        if (idade < 18)
                            ModelState.AddModelError("DataNascimento", "Não é permitido fornecedor menor de idade no estado do Paraná.");
                    }
                }
                else
                {
                    fornecedor.Rg = string.Empty;
                    fornecedor.DataNascimento = DateTime.MaxValue;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    fornecedor.Cpf_Cnpj = Regex.Replace(fornecedor.Cpf_Cnpj, "[^0-9,]", "");
                    fornecedor.Rg = Regex.Replace(fornecedor.Cpf_Cnpj, "[^0-9,]", "");
                    fornecedor.DataCadastro = DateTime.Now;

                    var _fornecedor = (Fornecedor)fornecedor;

                    db.Fornecedor.Add(_fornecedor);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                        ModelState.AddModelError("Cpf_Cnpj", "Cpf ou Cnpj já cadastrado.");
                    else
                        ModelState.AddModelError("", e.Message);

                    fornecedor.ListEmpresas = new SelectList(db.Empresa, "Cnpj", "NomeFantasia");
                    return View("Create", fornecedor);
                }

                try
                {
                    foreach (var item in fornecedor.Telefones)
                        db.Telefone.Add(
                            new Telefone()
                            {
                                Fornecedor_Cpf_Cnpj = fornecedor.Cpf_Cnpj,
                                Numero = item
                            });

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var innerException = e.InnerException.InnerException as SqlException;
                    ModelState.AddModelError("", e.Message);

                    fornecedor.ListEmpresas = new SelectList(db.Empresa, "Cnpj", "NomeFantasia");
                    return View("Create", fornecedor);
                }
                // return Content("Success");
            }

            fornecedor.ListEmpresas = new SelectList(db.Empresa, "Cnpj", "NomeFantasia");
            return View("Create", fornecedor);
        }

        public ActionResult Edit(string EmpresaCnpj, string FornecedorCpfCnpj)
        {
            var fornecedor =
                db.Fornecedor
                .Include(f => f.Telefone)
                .Where(x => x.Cpf_Cnpj.Contains(FornecedorCpfCnpj) && x.Empresa_Cnpj == EmpresaCnpj)                    
                .FirstOrDefault();

            if (fornecedor == null)
                return HttpNotFound();
            
            var _fornecedor = (FornecedorViewModel)fornecedor;

            _fornecedor.Telefones = fornecedor.Telefone.Select(x => x.Numero).ToList();
            _fornecedor.NomeFantasia = db.Empresa.Where(x => x.Cnpj == _fornecedor.Empresa_Cnpj).Select(x => x.NomeFantasia).FirstOrDefault();

            return View("Edit", _fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirmed(FornecedorViewModel fornecedorViewModel)
        {
            var _fornecedor = new Fornecedor();

            if (ModelState.IsValid)
            {
                if (fornecedorViewModel.Tipo_Pessoa == "F" && !Utils.Utils.IsCpf(fornecedorViewModel.Cpf_Cnpj))
                    ModelState.AddModelError("Cpf_Cnpj", "Cpf inválido.");

                if (fornecedorViewModel.Tipo_Pessoa == "J" && !Utils.Utils.IsCnpj(fornecedorViewModel.Cpf_Cnpj))
                    ModelState.AddModelError("Cpf_Cnpj", "Cnpj inválido.");

                if (fornecedorViewModel.Tipo_Pessoa == "F" && fornecedorViewModel.Rg == string.Empty)
                    ModelState.AddModelError("Rg", "Rg inválido.");

                if (fornecedorViewModel.Tipo_Pessoa == "F")
                {
                    var empresa = db.Empresa.Where(x => x.Cnpj == fornecedorViewModel.Empresa_Cnpj).FirstOrDefault();

                    if (empresa != null && empresa.UF == "PR")
                    {

                        var hoje = DateTime.Now;

                        var idade = hoje.Year - fornecedorViewModel.DataNascimento.Year;
                        if (fornecedorViewModel.DataNascimento > hoje.AddYears(-idade))
                            idade--;

                        if (idade < 18)
                            ModelState.AddModelError("DataNascimento", "Não é permitido fornecedor menor de idade" + Environment.NewLine + "no estado do Paraná.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    fornecedorViewModel.Cpf_Cnpj = Regex.Replace(fornecedorViewModel.Cpf_Cnpj, "[^0-9,]", "");
                    fornecedorViewModel.Empresa_Cnpj = Regex.Replace(fornecedorViewModel.Empresa_Cnpj, "[^0-9,]", "");
                    fornecedorViewModel.Rg = fornecedorViewModel.Rg != null ? Regex.Replace(fornecedorViewModel.Rg, "[^0-9,]", "") : string.Empty;

                    _fornecedor =
                        db.Fornecedor
                        .Include(f => f.Telefone)
                        .Where(x => x.Cpf_Cnpj == fornecedorViewModel.Cpf_Cnpj && x.Empresa_Cnpj == fornecedorViewModel.Empresa_Cnpj).FirstOrDefault();

                    // Addiciona ou remove telefones
                    var dbFones = _fornecedor.Telefone.Select(x => x.Numero).ToList();

                    if (fornecedorViewModel.Telefones == null || fornecedorViewModel.Telefones.Count == 0)
                        db.Telefone.RemoveRange(_fornecedor.Telefone.ToList());
                    else
                    {
                        var lsRemoveFones = dbFones.Where(x => !fornecedorViewModel.Telefones.Contains(x)).ToList();
                        var dbRemoveFones = _fornecedor.Telefone.Where(x => lsRemoveFones.Contains(x.Numero)).ToList();
                        db.Telefone.RemoveRange(dbRemoveFones);

                        var lsAddFones = fornecedorViewModel.Telefones.Where(x => !dbFones.Contains(x)).ToList();

                        if (lsAddFones.Count > 0)
                            foreach (var item in lsAddFones)
                                db.Telefone.Add(new Telefone() { Fornecedor_Cpf_Cnpj = fornecedorViewModel.Cpf_Cnpj, Numero = item });
                        // db.SaveChanges();
                    }

                    _fornecedor.DataNascimento = fornecedorViewModel.DataNascimento;
                    _fornecedor.Nome = fornecedorViewModel.Nome;
                    _fornecedor.Rg = fornecedorViewModel.Rg;
                    _fornecedor.Tipo_Pessoa = fornecedorViewModel.Tipo_Pessoa;

                    db.Entry(_fornecedor).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var innerException = e.InnerException.InnerException as SqlException;
                    ModelState.AddModelError("", e.Message);

                    fornecedorViewModel.ListEmpresas = new SelectList(db.Empresa, "Cnpj", "NomeFantasia");
                    return View("Edit", fornecedorViewModel);
                }
                // return Content("Success");
            }

            fornecedorViewModel.ListEmpresas = new SelectList(db.Empresa, "Cnpj", "NomeFantasia");
            return View("Edit", fornecedorViewModel);
        }

        public ActionResult Details(string EmpresaCnpj, string FornecedorCpfCnpj)
        {

            FornecedorCpfCnpj = Convert.ToInt64(FornecedorCpfCnpj).ToString();

            var fornecedor =
                db.Fornecedor
                .Include(f => f.Telefone)
                .Where(x => x.Cpf_Cnpj.Contains(FornecedorCpfCnpj) && x.Empresa_Cnpj == EmpresaCnpj).FirstOrDefault();

            if (fornecedor.Tipo_Pessoa == "F")
                fornecedor.Cpf_Cnpj = Convert.ToUInt64(fornecedor.Cpf_Cnpj).ToString(@"000\.000\.000\-00");
            else
                fornecedor.Cpf_Cnpj = Convert.ToUInt64(fornecedor.Cpf_Cnpj).ToString(@"00\.000\.000\/0000\-00");

            if (fornecedor == null)
                return HttpNotFound();

            return View("Details", fornecedor);
        }

        // GET: Fornecedor/Delete/5
        public ActionResult Delete(string EmpresaCnpj, string FornecedorCpfCnpj)
        {
            var fornecedor =
                db.Fornecedor
                .Include(f => f.Telefone)
                .Where(x => x.Cpf_Cnpj.Contains(FornecedorCpfCnpj) && x.Empresa_Cnpj == EmpresaCnpj).FirstOrDefault();

            if (fornecedor == null)
                return HttpNotFound();

            return View("Delete", fornecedor);
        }

        // POST: Fornecedor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Empresa_Cnpj, string Cpf_Cnpj)
        {
            var fornecedor =
                db.Fornecedor
                .Include(f => f.Telefone)
                .Where(x => x.Cpf_Cnpj.Contains(Cpf_Cnpj) && x.Empresa_Cnpj == Empresa_Cnpj).FirstOrDefault();

            try
            {
                var telefones = db.Telefone.Where(x => x.Fornecedor_Cpf_Cnpj == Cpf_Cnpj).ToList();
                db.Telefone.RemoveRange(telefones);
                db.Fornecedor.Remove(fornecedor);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var innerException = e.InnerException.InnerException as SqlException;
                if (innerException != null)
                    ModelState.AddModelError("", e.Message);

                return View("Delete", fornecedor);
            }

            // return Content("Success");
            return View("Delete", fornecedor);
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
