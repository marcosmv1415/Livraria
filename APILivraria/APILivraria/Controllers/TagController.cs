using APILivraria.Models;
using APILivraria.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagRepository _tagRepository;

    public TagController(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    // GET: api/Tag
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
    {
        try
        {
            var tags = await _tagRepository.ObterTodasTags();

            if (tags == null || !tags.Any())
            {
                return NotFound(new { Message = "Nenhuma tag encontrada." });
            }

            return Ok(tags);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }
    // GET: api/Tag/{codigo}
    [HttpGet("{codigo}")]
    public async Task<ActionResult<Tag>> GetTagPorId(int codigo)
    {
        try
        {
            var tag = await _tagRepository.ObterTagPorId(codigo);

            if (tag == null)
            {
                return NotFound(new { Message = "Tag não encontrada." });
            }

            return Ok(tag);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }
    // POST: api/Tag
    [HttpPost]
    public async Task<ActionResult> CadastrarTag([FromBody] Tag tag)
    {
        try
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.Descricao))
            {
                return BadRequest(new { Message = "Dados inválidos para a tag." });
            }

            await _tagRepository.Cadastrar(tag);

            return CreatedAtAction(nameof(GetTags), new { codigo = tag.Codigo }, tag);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }
    // PUT: api/Tag/{codigo}
    [HttpPut("{codigo}")]
    public async Task<ActionResult> AtualizarTag(int codigo, [FromBody] Tag tag)
    {
        try
        {
            if (tag == null || codigo != tag.Codigo || string.IsNullOrWhiteSpace(tag.Descricao))
            {
                return BadRequest(new { Message = "Dados inválidos para a tag." });
            }

            var tagExistente = await _tagRepository.ObterTodasTags(); 
            if (tagExistente == null || !tagExistente.Any(t => t.Codigo == codigo))
            {
                return NotFound(new { Message = "Tag não encontrada." });
            }

            await _tagRepository.Atualizar(tag);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }
    // DELETE: api/Tag/{codigo}
    [HttpDelete("{codigo}")]
    public async Task<ActionResult> DeleteTag(int codigo)
    {
        try
        {
            var tagExistente = await _tagRepository.ObterTagPorId(codigo);
            if (tagExistente == null)
            {
                return NotFound(new { Message = "Tag não encontrada." });
            }
            await _tagRepository.Deletar(codigo);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Error = ex.Message });
        }
    }

}
