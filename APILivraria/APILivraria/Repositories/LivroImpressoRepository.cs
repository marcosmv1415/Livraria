using APILivraria.Data;
using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace APILivraria.Repositories
{
    public class LivroImpressoRepository : ILivroImpressoRepository
    {
        IConfiguration _conf;
        BancoContext _banco;
        public LivroImpressoRepository(BancoContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Atualizar(LivroImpresso livroimpresso)
        {
            _banco.SaveChanges();
        }
        public void Cadastrar(LivroImpresso livroimpresso)
        {
            _banco.Add(livroimpresso);
            _banco.SaveChanges();

        }
    }
}
