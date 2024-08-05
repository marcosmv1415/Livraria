using APILivraria.Models;
using System.Security.Cryptography;

namespace APILivraria.Repositories.Contracts
{
    public interface ILivroImpressoRepository
    {
        void Cadastrar(LivroImpresso livroimpresso);
        void Atualizar(LivroImpresso livroimpresso);
    }
}
