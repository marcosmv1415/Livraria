using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteFront.Models
{
    public class LivroDigital
    {
        [Key, ForeignKey("Livro")] 
        public int Codigo { get; set; }
        public string? Formato { get; set; }

        public Livro? Livro { get; set; }
    }
}
