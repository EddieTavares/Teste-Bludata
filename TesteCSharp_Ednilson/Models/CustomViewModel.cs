using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using TesteCSharp_Ednilson.Models.Validation;
using static TesteCSharp_Ednilson.App_Start.CustomValidators;

namespace TesteCSharp_Ednilson.Models
{
    public class FornecedorViewModel
    {
        [Key]
        [Required]
        [StringLength(18)]
        [Display(Name = "Cpf/Cnpj")]
        public string Cpf_Cnpj { get; set; }

        [Required]
        [StringLength(18)]
        public string Empresa_Cnpj { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Tipo de pessoa")]
        // [TipoPessoa(ErrorMessage = "Tipo de pessoa inválido")]
        public string Tipo_Pessoa { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [StringLength(14)]
        [Display(Name = "Rg")]
        [RequiredIf("Tipo_Pessoa", "F", ErrorMessage = "Rg não preenchido.")]
        public string Rg { get; set; }

        [Display(Name = "Data Cadastro")]
        [DataType(DataType.DateTime), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Nascimento")]
        [DataType(DataType.DateTime), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Empresa")]
        public SelectList ListEmpresas { get; set; }

        [Display(Name = "Empresa")]
        public string NomeFantasia { get; set; }

        [Required]
        public List<string> Telefones { get; set; }
        
        
        public static explicit operator FornecedorViewModel(Fornecedor fornecedor)
        {

            return new FornecedorViewModel() {
                Cpf_Cnpj = fornecedor.Cpf_Cnpj,
                DataCadastro = fornecedor.DataCadastro,
                DataNascimento = fornecedor.DataNascimento,
                Empresa_Cnpj = fornecedor.Empresa_Cnpj,
                Nome = fornecedor.Nome,
                Rg = fornecedor.Rg,
                Tipo_Pessoa = fornecedor.Tipo_Pessoa
            };

            throw new NotImplementedException();
        }

        // public virtual ICollection<Telefone> Telefone { get; set; }
    }

    public class EmpresaViewModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(18)]
        [Display(Name = "Cnpj")]
        [Cnpj(ErrorMessage = "CNPJ inválido")]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Razão social")]
        public string NomeFantasia { get; set; }
    
        [StringLength(2)]
        [Display(Name = "UF")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Selecione um Estado.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UF { get; set; }

        public SelectList Estados { get; set; }


        public static explicit operator EmpresaViewModel(Empresa empresa)
        {
            return new EmpresaViewModel()
            {
                Cnpj = empresa.Cnpj,
                NomeFantasia = empresa.NomeFantasia,
                UF = empresa.UF
            };

            throw new NotImplementedException();
        }
    }

    public class EmpresaIndexViewModel
    {
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string FiltroNome { get; set; }

        [StringLength(18)]
        [Display(Name = "Cpf/Cnpj")]
        public string FiltroCpfCnpj { get; set; }


        [Display(Name = "Data inicial")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FiltroDataInicial { get; set; }

        [Display(Name = "Data final")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FiltroDataFinal { get; set; }

        public List<Fornecedor> Fornecedores { get; set; }
    }
}