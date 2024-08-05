using APILivraria.Data;
using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APILivraria.Repositories
{
    public class TipoEncadernacaoRepository : ITipoEncadernacaoRepository
    {
        IConfiguration _conf;
        BancoContext _banco;
        public TipoEncadernacaoRepository(BancoContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public async Task Atualizar(TipoEncadernacao tipoEncadernacao)
        {
            var tipoEncadernacaoExistente = await _banco.TipoEncadernacao.FindAsync(tipoEncadernacao.Codigo);

            if (tipoEncadernacaoExistente != null)
            {
                tipoEncadernacaoExistente.Nome = tipoEncadernacao.Nome;
                tipoEncadernacaoExistente.Descricao = tipoEncadernacao.Descricao;
                tipoEncadernacaoExistente.Formato = tipoEncadernacao.Formato;

                _banco.TipoEncadernacao.Update(tipoEncadernacaoExistente);
                await _banco.SaveChangesAsync();
            }
        }
        public async Task Cadastrar(TipoEncadernacao tipoEncadernacao)
        {
            _banco.TipoEncadernacao.Add(tipoEncadernacao);
            await _banco.SaveChangesAsync();
        }
        public async Task<IEnumerable<TipoEncadernacao>> ObterTodosTiposEncadernacao()
        {
            return await _banco.TipoEncadernacao.ToListAsync();
        }
        public async Task<TipoEncadernacao> ObterTipoEncadernacaoPorId(int codigo)
        {
            return await _banco.TipoEncadernacao.FirstOrDefaultAsync(t => t.Codigo == codigo);
        }

        public async Task<bool> TipoEncadernacaoEmUso(int codigo)
        {
            return await _banco.LivroImpresso.AnyAsync(l => l.TipoEncadernacaoID == codigo);
        }

        public async Task Deletar(int codigo)
        {
            var tipoEncadernacao = await _banco.TipoEncadernacao.FindAsync(codigo);
            bool isTipoEncadernacaoInUse = await TipoEncadernacaoEmUso(codigo);

            if (tipoEncadernacao != null && !isTipoEncadernacaoInUse)
            {
                _banco.TipoEncadernacao.Remove(tipoEncadernacao);
                await _banco.SaveChangesAsync();
            }
            else if (isTipoEncadernacaoInUse)
            {
                throw new InvalidOperationException("O tipo de encadernação está em uso e não pode ser deletado.");
            }
        }

    }
}
