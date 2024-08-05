using APILivraria.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APILivraria.Repositories.Contracts
{
    public interface ITipoEncadernacaoRepository
    {
        Task Cadastrar(TipoEncadernacao tipoEncadernacao);
        Task Atualizar(TipoEncadernacao tipoEncadernacao);
        Task<IEnumerable<TipoEncadernacao>> ObterTodosTiposEncadernacao();
        Task<TipoEncadernacao> ObterTipoEncadernacaoPorId(int codigo);
        Task Deletar(int codigo);
    }
}
