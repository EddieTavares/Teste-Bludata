using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TesteCSharp_Ednilson.Models
{
    [Table("Telefone")]
    public partial class Telefone
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(30)]
        public string Numero { get; set; }

        [Key, ForeignKey("Fornecedor"), Column(Order = 1)]
        [StringLength(18)]
        public string Fornecedor_Cpf_Cnpj { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }
    }
}