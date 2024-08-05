using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TesteFront.Models
{
    public class LivroImpresso
    {
        [Key, ForeignKey("Livro")] 
        public int Codigo { get; set; }
        public decimal? Peso { get; set; }

        [ForeignKey("TipoEncadernacao")]
        public int? TipoEncadernacaoID { get; set; }

        [JsonIgnore] 
        public TipoEncadernacao? TipoEncadernacao { get; set; }

        public Livro? Livro { get; set; }
    }
}
