using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TesteCSharp_Ednilson.Models;

namespace TesteCSharp_Ednilson.Controllers
{
    public class FornecedorController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Fornecedor
        public ActionResult Index()
        {
            try
            {
                ViewBag.EmpresaCnpj = db.Empresa.FirstOrDefault().Cnpj;
            }
            catch (Exception)
            {
                return Json(new { Rc = 9, Message = "Erro" });
            }
            return View();
        }

        // GET: Fornecedor
        public ActionResult GetPartialIndex(
            string EmpresaCnpj,
            string Nome,
            string CpfCnpj,
            string DataInicial,
            string DataFinal)
        {
            var fornecedor = db.Fornecedor.Include(f => f.Empresa).Where(x => x.Empresa_Cnpj == EmpresaCnpj);
            
            if (!string.IsNullOrWhiteSpace(Nome))
                fornecedor = fornecedor.Where(x => x.Nome.Contains(Nome));

            if (!string.IsNullOrWhiteSpace(CpfCnpj))
                fornecedor = fornecedor.Where(x => x.Cpf_Cnpj.Contains(CpfCnpj));

            var _DataInicial = new DateTime();

            if (DataInicial != string.Empty)
            {
                _DataInicial = Convert.ToDateTime(DataInicial);
                fornecedor = fornecedor.Where(x => x.DataCadastro >= _DataInicial);
            }

            var _DataFinal = new DateTime();

            if (DataFinal != string.Empty)
            {
                _DataFinal = Convert.ToDateTime(DataFinal).AddDays(1).AddMinutes(-1);
                fornecedor = fornecedor.Where(x => x.DataCadastro <= _DataFinal);
            }
            var _fornecedor = fornecedor.ToList();

            foreach (var item in _fornecedor)
            {
                if (item.Tipo_Pessoa == "F")
                    item.Cpf_Cnpj = Convert.ToUInt64(item.Cpf_Cnpj).ToString(@"000\.000\.000\-00");
                else
                    item.Cpf_Cnpj = Convert.ToUInt64(item.Cpf_Cnpj).ToString(@"00\.000\.000\/0000\-00");
            }

            return View("PartialIndex", _fornecedor);
        }

        // GET: Fornecedor/Create
        public ActionResult GetPartialCreate()
        {
            ViewBag.Empresa_Cnpj = new SelectList(db.Empresa, "Cnpj", "NomeFantasia");
            return View("PartialCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostConfirmaCreate(
            string CpfCnpj, 
            string EmpresaCnpj, 
            string TipoPessoa, 
            string Nome, 
            string Rg, 
            string DataNascimento, 
            List<string> Fones)
        {
            if (TipoPessoa != "F" && TipoPessoa != "J")
                return Json(new { Rc = 9, Message = "Tipo de pessoa inválido" });

            if (TipoPessoa == "F")
            {
                if (!Utils.IsCpf(CpfCnpj))
                    return Json(new { Rc = 9, Message = "Cpf inválido" });
                if (Rg == string.Empty)
                    return Json(new { Rc = 9, Message = "Rg inválido" });
            }

            if (TipoPessoa == "J" && !Utils.IsCnpj(CpfCnpj))
                return Json(new { Rc = 9, Message = "Cnpj inválido" });

            try
            {
                var fornecedor = new Fornecedor();
                
                fornecedor.Cpf_Cnpj = CpfCnpj;
                fornecedor.Empresa_Cnpj = EmpresaCnpj;
                fornecedor.Tipo_Pessoa = TipoPessoa;
                fornecedor.Nome = Nome;
                fornecedor.DataCadastro = DateTime.Now;

                if (TipoPessoa == "F")
                {
                    fornecedor.Rg = Rg;
                    fornecedor.DataNascimento = Convert.ToDateTime(DataNascimento);

                    var empresa = db.Empresa.Where(x => x.Cnpj == EmpresaCnpj).FirstOrDefault();

                    if (empresa != null && empresa.UF == "PR")
                    {
                        TimeSpan date = DateTime.Now - Convert.ToDateTime(fornecedor.DataNascimento);
                        double totalAnos = date.Days / 365;

                        if (totalAnos < 18)
                            return Json(new { Rc = 5, Message = "Não é permitido fornecedor menor de idade no estado do Paraná. " });
                    }
                }
                else
                {
                    fornecedor.Rg = string.Empty;
                    fornecedor.DataNascimento = DateTime.MaxValue;
                }

                db.Fornecedor.Add(fornecedor);
                db.SaveChanges();

                foreach (var item in Fones)
                {
                    db.Telefone.Add(
                        new Telefone(){
                            Fornecedor_Cpf_Cnpj = CpfCnpj,
                            Numero = item
                        });
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var innerException = e.InnerException.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    return Json(new { Rc = 2627, Message = "Cpf ou Cnpj já cadastrado." });
                else
                    return Json(new { Rc = 9, e.Message });
            }

            return Json(new {Rc = 0,  Message = ""});
        }

        public ActionResult GetPartialEdit(string EmpresaCnpj, string FornecedorCpfCnpj)
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

            return View("PartialEdit", fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostConfirmaEdit(
            string CpfCnpj,
            string EmpresaCnpj,
            string TipoPessoa,
            string Nome,
            string Rg,
            string DataNascimento,
            List<string> Fones)
        {
            if (TipoPessoa != "F" && TipoPessoa != "J")
                return Json(new { Rc = 9, Message = "Tipo de pessoa inválido" });

            if (TipoPessoa == "F")
            {
                if (!Utils.IsCpf(CpfCnpj))
                    return Json(new { Rc = 9, Message = "Cpf inválido" });
                if (Rg == string.Empty)
                    return Json(new { Rc = 9, Message = "Rg inválido" });
            }

            if (TipoPessoa == "J" && !Utils.IsCnpj(CpfCnpj))
                return Json(new { Rc = 9, Message = "Cnpj inválido" });

            try
            {
                var fornecedor =
                    db.Fornecedor
                    .Include(f => f.Telefone)
                    .Where(x => x.Cpf_Cnpj == CpfCnpj && x.Empresa_Cnpj == EmpresaCnpj).FirstOrDefault();

                // Addiciona ou remove telefones
                var dbFones = fornecedor.Telefone.Select(x => x.Numero).ToList();

                if (Fones == null || Fones.Count == 0)
                    db.Telefone.RemoveRange(fornecedor.Telefone.ToList());
                else
                {
                    var lsRemoveFones = dbFones.Where(x => !Fones.Contains(x)).ToList();
                    var dbRemoveFones = fornecedor.Telefone.Where(x => lsRemoveFones.Contains(x.Numero)).ToList();
                    db.Telefone.RemoveRange(dbRemoveFones);

                    var lsAddFones = Fones.Where(x => !dbFones.Contains(x)).ToList();

                    if (lsAddFones.Count > 0)
                        foreach (var item in lsAddFones)
                            db.Telefone.Add(new Telefone() { Fornecedor_Cpf_Cnpj = CpfCnpj, Numero = item });

                    db.SaveChanges();
                }


                // Demais campos fornecedor
                fornecedor.Nome = Nome;

                if (TipoPessoa == "F")
                {
                    fornecedor.Rg = Rg;
                    fornecedor.DataNascimento = Convert.ToDateTime(DataNascimento);

                    var empresa = db.Empresa.Where(x => x.Cnpj == EmpresaCnpj).FirstOrDefault();

                    if (empresa != null && empresa.UF == "PR")
                    {
                        TimeSpan date = DateTime.Now - Convert.ToDateTime(fornecedor.DataNascimento);
                        double totalAnos = date.Days / 365;

                        if (totalAnos < 18)
                            return Json(new { Rc = 5, Message = "Não é permitido fornecedor menor de idade no estado do Paraná. " });
                    }
                }
                else
                {
                    fornecedor.Rg = string.Empty;
                    fornecedor.DataNascimento = DateTime.MaxValue;
                }

                db.Entry(fornecedor).State = EntityState.Modified;

                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { Rc = 9, e.Message });
            }

            return Json(new { Rc = 0, Message = "" });
        }

        public ActionResult GetPartialDetalhes(string EmpresaCnpj, string FornecedorCpfCnpj)
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

            return View("PartialDetails", fornecedor);
        }

        // GET: Fornecedor/Delete/5
        public ActionResult GetPartialDelete(string EmpresaCnpj, string FornecedorCpfCnpj)
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

            return View("PartialDelete", fornecedor);
        }

        // POST: Fornecedor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string EmpresaCnpj, string FornecedorCpfCnpj)
        {
            FornecedorCpfCnpj = Convert.ToInt64(FornecedorCpfCnpj).ToString();

            try
            {
                var telefones = db.Telefone.Where(x => x.Fornecedor_Cpf_Cnpj == FornecedorCpfCnpj).ToList();

                db.Telefone.RemoveRange(telefones);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { Rc = 5, e.Message });
            }

            try
            {
                var fornecedor =
                    db.Fornecedor
                    .Where(x => x.Cpf_Cnpj == FornecedorCpfCnpj && x.Empresa_Cnpj == EmpresaCnpj);

                db.Fornecedor.Remove(fornecedor.FirstOrDefault());
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { Rc = 5, e.Message });
            }

            return Json(new { Rc = 0, Message = "" });
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
