using APILivraria.Data;
using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APILivraria.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly IConfiguration _conf;
        private readonly BancoContext _banco;

        public LivroRepository(BancoContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public async Task Atualizar(Livro livro)
        {
            var livroExistente = await _banco.Livro.FindAsync(livro.Codigo);

            if (livroExistente != null)
            {
                livroExistente.Titulo = livro.Titulo;
                livroExistente.Autor = livro.Autor;
                livroExistente.Lancamento = livro.Lancamento;
                livroExistente.TagID = livro.TagID;

                _banco.Livro.Update(livroExistente);
                await _banco.SaveChangesAsync();
            }
        }

        public void Cadastrar(Livro livro)
        {
            _banco.Livro.Add(livro);
            _banco.SaveChanges();
        }
        public  List<Livro> ObterTodosLivros(int? ano = null, int? mes = null)
        {

            var livrosFiltrados = _banco.Livro
                .Include(l => l.Tag) 
                .Include(l => l.LivroDigital) 
                .Include(l => l.LivroImpresso) 
                .AsQueryable();

            if (ano.HasValue)
            {
                livrosFiltrados = livrosFiltrados.Where(l => l.Lancamento.Year == ano.Value);
            }

            if (mes.HasValue)
            {
                livrosFiltrados = livrosFiltrados.Where(l => l.Lancamento.Month == mes.Value);
            }

            return livrosFiltrados.ToList();
        }
        public async Task<Livro> ObterLivroPorId(int codigo)
        {
            Livro livro = null;

            using (var command = _banco.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "sp_ObterLivroPorCodigo";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var codigoParam = command.CreateParameter();
                codigoParam.ParameterName = "@Codigo";
                codigoParam.Value = codigo;
                command.Parameters.Add(codigoParam);

                await _banco.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        livro = new Livro
                        {
                            Codigo = reader.GetInt32(reader.GetOrdinal("Codigo")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Autor = reader.GetString(reader.GetOrdinal("Autor")),
                            Lancamento = reader.GetDateTime(reader.GetOrdinal("Lancamento")),
                            TagID = reader.IsDBNull(reader.GetOrdinal("TagID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TagID")),
                            Tag = reader.IsDBNull(reader.GetOrdinal("TagCodigo")) ? null : new Tag
                            {
                                Codigo = reader.GetInt32(reader.GetOrdinal("TagCodigo")),
                                Descricao = reader.GetString(reader.GetOrdinal("TagDescricao"))
                            },
                            LivroDigital = reader.IsDBNull(reader.GetOrdinal("LivroDigitalCodigo")) ? null : new LivroDigital
                            {
                                Codigo = reader.GetInt32(reader.GetOrdinal("LivroDigitalCodigo")),
                                Formato = reader.GetString(reader.GetOrdinal("LivroDigitalFormato"))
                            },
                            LivroImpresso = reader.IsDBNull(reader.GetOrdinal("LivroImpressoCodigo")) ? null : new LivroImpresso
                            {
                                Codigo = reader.GetInt32(reader.GetOrdinal("LivroImpressoCodigo")),
                                Peso = reader.GetDecimal(reader.GetOrdinal("LivroImpressoPeso")),
                                TipoEncadernacaoID = reader.GetInt32(reader.GetOrdinal("LivroImpressoTipoEncadernacaoID"))
                            }
                        };
                    }
                }

                await _banco.Database.CloseConnectionAsync();
            }

            return livro;
        }
        public async Task Deletar(int codigo)
        {
            var livro = await _banco.Livro
                .Include(l => l.LivroDigital)  
                .Include(l => l.LivroImpresso)  
                .FirstOrDefaultAsync(l => l.Codigo == codigo);

            if (livro != null)
            {
                if (livro.LivroDigital != null)
                {
                    _banco.LivroDigital.Remove(livro.LivroDigital);
                }

                if (livro.LivroImpresso != null)
                {
                    _banco.LivroImpresso.Remove(livro.LivroImpresso);
                }
                _banco.Livro.Remove(livro);
                await _banco.SaveChangesAsync();
            }
        }

    }
}
