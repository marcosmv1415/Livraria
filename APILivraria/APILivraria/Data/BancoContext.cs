using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APILivraria.Models;

namespace APILivraria.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext (DbContextOptions<BancoContext> options)
            : base(options)
        {
        }

        public DbSet<Livro> Livro { get; set; }
        public DbSet<LivroDigital> LivroDigital { get; set; }
        public DbSet<LivroImpresso> LivroImpresso { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TipoEncadernacao> TipoEncadernacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.LivroDigital)
                .WithOne(ld => ld.Livro)
                .HasForeignKey<LivroDigital>(ld => ld.Codigo);

            modelBuilder.Entity<Livro>()
                .HasOne(l => l.LivroImpresso)
                .WithOne(li => li.Livro)
                .HasForeignKey<LivroImpresso>(li => li.Codigo);

            modelBuilder.Entity<Tag>()
                .HasMany(t => t.Livros)
                .WithOne(l => l.Tag)
                .HasForeignKey(l => l.TagID);

            modelBuilder.Entity<TipoEncadernacao>()
                .HasMany(te => te.LivrosImpressos)
                .WithOne(li => li.TipoEncadernacao)
                .HasForeignKey(li => li.TipoEncadernacaoID);

            base.OnModelCreating(modelBuilder);
        }


    }
}
