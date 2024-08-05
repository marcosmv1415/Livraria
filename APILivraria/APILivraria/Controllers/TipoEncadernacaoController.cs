using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class TipoEncadernacaoController : ControllerBase
{
    private readonly ITipoEncadernacaoRepository _tipoEncadernacaoRepository;

    public TipoEncadernacaoController(ITipoEncadernacaoRepository tipoEncadernacaoRepository)
    {
        _tipoEncadernacaoRepository = tipoEncadernacaoRepository;
    }

    // GET: api/TipoEncadernacao
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoEncadernacao>>> GetTiposEncadernacao()
    {
        try
        {
            var tiposEncadernacao = await _tipoEncadernacaoRepository.ObterTodosTiposEncadernacao();

            if (tiposEncadernacao == null || !tiposEncadernacao.Any())
            {
                return NotFound(new { Message = "Nenhum tipo de encadernação encontrado." });
            }

            return Ok(tiposEncadernacao);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }

    // GET: api/TipoEncadernacao/{codigo}
    [HttpGet("{codigo}")]
    public async Task<ActionResult<TipoEncadernacao>> GetTipoEncadernacaoPorId(int codigo)
    {
        try
        {
            var tipoEncadernacao = await _tipoEncadernacaoRepository.ObterTipoEncadernacaoPorId(codigo);

            if (tipoEncadernacao == null)
            {
                return NotFound(new { Message = "Tipo de encadernação não encontrado." });
            }

            return Ok(tipoEncadernacao);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }

    // POST: api/TipoEncadernacao
    [HttpPost]
    public async Task<ActionResult> CadastrarTipoEncadernacao([FromBody] TipoEncadernacao tipoEncadernacao)
    {
        try
        {
            if (tipoEncadernacao == null || string.IsNullOrWhiteSpace(tipoEncadernacao.Nome))
            {
                return BadRequest(new { Message = "Dados inválidos para o tipo de encadernação." });
            }

            await _tipoEncadernacaoRepository.Cadastrar(tipoEncadernacao);

            return CreatedAtAction(nameof(GetTipoEncadernacaoPorId), new { codigo = tipoEncadernacao.Codigo }, tipoEncadernacao);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }

    // PUT: api/TipoEncadernacao/{codigo}
    [HttpPut("{codigo}")]
    public async Task<ActionResult> AtualizarTipoEncadernacao(int codigo, [FromBody] TipoEncadernacao tipoEncadernacao)
    {
        try
        {
            if (tipoEncadernacao == null || codigo != tipoEncadernacao.Codigo || string.IsNullOrWhiteSpace(tipoEncadernacao.Nome))
            {
                return BadRequest(new { Message = "Dados inválidos para o tipo de encadernação." });
            }

            var tipoExistente = await _tipoEncadernacaoRepository.ObterTipoEncadernacaoPorId(codigo);
            if (tipoExistente == null)
            {
                return NotFound(new { Message = "Tipo de encadernação não encontrado." });
            }

            await _tipoEncadernacaoRepository.Atualizar(tipoEncadernacao);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }

    // DELETE: api/TipoEncadernacao/{codigo}
    [HttpDelete("{codigo}")]
    public async Task<ActionResult> DeleteTipoEncadernacao(int codigo)
    {
        try
        {
            var tipoExistente = await _tipoEncadernacaoRepository.ObterTipoEncadernacaoPorId(codigo);
            if (tipoExistente == null)
            {
                return NotFound(new { Message = "Tipo de encadernação não encontrado." });
            }

            await _tipoEncadernacaoRepository.Deletar(codigo);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }
}
