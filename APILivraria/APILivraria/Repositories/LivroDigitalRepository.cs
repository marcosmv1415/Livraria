using APILivraria.Data;
using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.Extensions.Configuration;

namespace APILivraria.Repositories
{
    public class LivroDigitalRepository : ILivroDigitalRepository
    {
        IConfiguration _conf;
        BancoContext _banco;
        public LivroDigitalRepository(BancoContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Atualizar(LivroDigital livrodigital)
        {
            _banco.SaveChanges();
        }
        public void Cadastrar(LivroDigital livrodigital)
        {
            _banco.Add(livrodigital);
            _banco.SaveChanges();

        }
    }
}
