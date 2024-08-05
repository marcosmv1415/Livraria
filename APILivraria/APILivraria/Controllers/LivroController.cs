using Microsoft.AspNetCore.Mvc;
using APILivraria.Repositories.Contracts;
using APILivraria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APILivraria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroRepository _livroRepository;
        private readonly ILivroDigitalRepository _livroDigitalRepository;
        private readonly ILivroImpressoRepository _livroImpressoRepository;

        public LivrosController(ILivroRepository livroRepository, ILivroDigitalRepository livroDigitalRepository, ILivroImpressoRepository livroImpressoRepository)
        {
            _livroRepository = livroRepository;
            _livroDigitalRepository = livroDigitalRepository;
            _livroImpressoRepository= livroImpressoRepository;
        }

        // GET: api/Livros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivros([FromQuery] int? ano = null, [FromQuery] int? mes = null)
        {
            try
            {
                var livros = await Task.Run(() => _livroRepository.ObterTodosLivros(ano, mes));
                
                if (livros == null || !livros.Any())
                {
                    return NotFound(new { Message = "Nenhum livro encontrado." });
                }

                return Ok(livros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarLivro([FromBody] Livro livro)
        {
            try
            {
                if (livro == null || string.IsNullOrEmpty(livro.Titulo) || string.IsNullOrEmpty(livro.Autor))
                {
                    return BadRequest(new { Message = "Dados inválidos para o livro." });
                }
                _livroRepository.Cadastrar(livro);


                return CreatedAtAction(nameof(GetLivroPorId), new { codigo = livro.Codigo }, livro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
            }
        }
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Livro>> GetLivroPorId(int codigo)
        {
            try
            {
                var livro = await _livroRepository.ObterLivroPorId(codigo);

                if (livro == null)
                {
                    return NotFound(new { Message = "Livro não encontrado." });
                }

                return Ok(livro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
            }
        }
        // PUT: api/Livros/{codigo}
        [HttpPut("{codigo}")]
        public async Task<ActionResult> EditarLivro(int codigo, [FromBody] Livro livro)
        {
            try
            {
                if (livro == null || codigo != livro.Codigo)
                {
                    return BadRequest(new { Message = "Dados inválidos para o livro." });
                }

                var livroExistente = await _livroRepository.ObterLivroPorId(codigo);
                if (livroExistente == null)
                {
                    return NotFound(new { Message = "Livro não encontrado." });
                }
                await _livroRepository.Atualizar(livro);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
            }
        }
        [HttpDelete("{codigo}")]
        public async Task<ActionResult> DeleteLivro(int codigo)
        {
            try
            {

                var livroExistente = await _livroRepository.ObterLivroPorId(codigo);
                if (livroExistente == null)
                {
                    return NotFound(new { Message = "Livro não encontrado." });
                }

                await _livroRepository.Deletar(codigo);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
            }
        }

    }
}
