using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILivraria.Models
{
    public class Livro
    {
        [Key]
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime Lancamento { get; set; }

        [ForeignKey("Tag")]
        public int? TagID { get; set; }
        public Tag Tag { get; set; }

        public LivroDigital LivroDigital { get; set; }
        public LivroImpresso LivroImpresso { get; set; }
    } 
}

