using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TesteCSharp_Ednilson.Models.Validation;
using static TesteCSharp_Ednilson.App_Start.CustomValidators;

namespace TesteCSharp_Ednilson.Models
{
    [Table("Fornecedor")]
    public partial class Fornecedor
    {
        public Fornecedor() => this.Telefone = new HashSet<Telefone>();

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [StringLength(18)]
        [Display(Name = "Cpf/Cnpj")]
        public string Cpf_Cnpj { get; set; }

        [ForeignKey("Empresa"), Column(Order = 1)]
        [StringLength(18)]
        public string Empresa_Cnpj { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Tipo de pessoa")]
        [TipoPessoa(ErrorMessage = "Tipo de pessoa inválido")]
        public string Tipo_Pessoa { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [StringLength(14)]
        [Display(Name = "Rg")]
        [RequiredIf("Tipo_Pessoa", "F", ErrorMessage = "Rg inválido.")]
        public string Rg { get; set; }

        [Display(Name = "Data Cadastro")]
        [DataType(DataType.DateTime), Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Nascimento")]
        [DataType(DataType.DateTime), Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<Telefone> Telefone { get; set; }

        public static explicit operator Fornecedor(FornecedorViewModel fornecedor)
        {
            return new Fornecedor()
            {
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
    }
}