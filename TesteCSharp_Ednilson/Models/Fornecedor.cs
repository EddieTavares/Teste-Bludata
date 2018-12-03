using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteCSharp_Ednilson.Models
{
    [Table("Fornecedor")]
    public partial class Fornecedor
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(18)]
        [Display(Name = "Cpf/Cnpj")]
        public string Cpf_Cnpj { get; set; }

        [ForeignKey("Empresa"), Column(Order = 1)]
        [StringLength(18)]
        public string Empresa_Cnpj { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Tipo de pessoa")]
        public string Tipo_Pessoa { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [StringLength(14)]
        [Display(Name = "Rg")]
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
    }
}