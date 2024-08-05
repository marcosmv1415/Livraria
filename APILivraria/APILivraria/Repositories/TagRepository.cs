using APILivraria.Data;
using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APILivraria.Repositories
{
    public class TagRepository : ITagRepository
    {
        IConfiguration _conf;
        BancoContext _banco;
        public TagRepository(BancoContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public async Task Atualizar(Tag tag)
        {
            var tagExistente = await _banco.Tag.FindAsync(tag.Codigo);

            if (tagExistente != null)
            {
                tagExistente.Descricao = tag.Descricao;
                _banco.Tag.Update(tagExistente);
                await _banco.SaveChangesAsync();
            }
        }
        public async Task Cadastrar(Tag tag)
        {
            _banco.Tag.Add(tag);
            await _banco.SaveChangesAsync();
        }
        public async Task<IEnumerable<Tag>> ObterTodasTags()
        {
            return await _banco.Tag.ToListAsync();
        }
        public async Task<Tag> ObterTagPorId(int codigo)
        {
            return await _banco.Tag.FirstOrDefaultAsync(t => t.Codigo == codigo);
        }
        public async Task Deletar(int codigo)
        {
            var tag = await _banco.Tag.FindAsync(codigo);
            bool isTagInUse = await _banco.Livro.AnyAsync(l => l.TagID == codigo);

            if (tag != null && !isTagInUse)
            {
                _banco.Tag.Remove(tag);
                await _banco.SaveChangesAsync();
            }
            else if (isTagInUse)
            {
                throw new InvalidOperationException("A tag está em uso e não pode ser deletada.");
            }
        }

    }
}
