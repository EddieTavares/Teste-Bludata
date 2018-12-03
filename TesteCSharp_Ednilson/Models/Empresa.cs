using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteCSharp_Ednilson.Models
{
    [Table("Empresa")]
    public partial class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(18)]
        [Display(Name = "Cnpj")]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Razão social")]
        public string NomeFantasia { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "UF")]
        public string UF { get; set; }

        public virtual ICollection<Fornecedor> Fornecedor { get; set; }
    }
}