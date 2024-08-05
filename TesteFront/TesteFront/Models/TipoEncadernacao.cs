using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesteFront.Models
{
    public class TipoEncadernacao
    {
        [Key]
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Formato { get; set; }
        public ICollection<LivroImpresso> LivrosImpressos { get; set; }
    }
}
