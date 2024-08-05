using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteFront.Models
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

        [JsonIgnore] 
        public Tag? Tag { get; set; }

        public LivroDigital? LivroDigital { get; set; }
        public LivroImpresso? LivroImpresso { get; set; }
    } 
}

