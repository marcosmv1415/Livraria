using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APILivraria.Models
{
    public class Tag
    {
        [Key]
        public int Codigo { get; set; }
        public string Descricao { get; set; }

        public ICollection<Livro> Livros { get; set; }
        
    }
}
