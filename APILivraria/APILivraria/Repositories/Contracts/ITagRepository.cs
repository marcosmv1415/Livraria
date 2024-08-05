using APILivraria.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APILivraria.Repositories.Contracts
{
    public interface ITagRepository
    {
        Task Cadastrar(Tag tag);
        Task Atualizar(Tag tag);
        Task<IEnumerable<Tag>> ObterTodasTags();
        Task Deletar(int codigo);
        Task<Tag> ObterTagPorId(int codigo);
    }
}
