using APILivraria.Models;

namespace APILivraria.Repositories.Contracts
{
    public interface ILivroDigitalRepository
    {
        void Cadastrar(LivroDigital livrodigital);
        void Atualizar(LivroDigital livrodigital);
    }
}

