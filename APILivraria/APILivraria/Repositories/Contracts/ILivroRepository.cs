using APILivraria.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APILivraria.Repositories.Contracts
{
    public interface ILivroRepository
    {
        void Cadastrar(Livro livro);
        List<Livro> ObterTodosLivros(int? ano = null, int? mes = null);
        Task<Livro> ObterLivroPorId(int codigo);
        Task Atualizar(Livro livro);
        Task Deletar(int codigo);
    }
}
