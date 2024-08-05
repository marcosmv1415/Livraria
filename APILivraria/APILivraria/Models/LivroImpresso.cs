using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APILivraria.Models
{
    public class LivroImpresso
    {
        [Key, ForeignKey("Livro")] 
        public int Codigo { get; set; }
        public decimal Peso { get; set; }

        [ForeignKey("TipoEncadernacao")]
        public int TipoEncadernacaoID { get; set; }
        public TipoEncadernacao TipoEncadernacao { get; set; }

        public Livro Livro { get; set; }
    }
}
