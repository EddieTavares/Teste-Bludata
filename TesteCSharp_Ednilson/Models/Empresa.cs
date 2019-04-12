using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TesteCSharp_Ednilson.Models.Validation;

namespace TesteCSharp_Ednilson.Models
{
    [Table("Empresa")]
    public partial class Empresa
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

        public virtual ICollection<Fornecedor> Fornecedor { get; set; }

        public static explicit operator Empresa(EmpresaViewModel empresa)
        {
            return new Empresa(){
                Cnpj = empresa.Cnpj,
                NomeFantasia = empresa.NomeFantasia,
                UF = empresa.UF
            };

            throw new NotImplementedException();
        }
    }
}