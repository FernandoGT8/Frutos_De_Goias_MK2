using FrutosDeGoias.Api.Data;
using FrutosDeGoias.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrutosDeGoias.Api.Controllers;

// Define que esta classe responde a requisições web e formata tudo em JSON
[ApiController]
[Route("api/[controller]")] // A rota será: /api/estabelecimentos
public class EstabelecimentosController : ControllerBase
{
    private readonly AppDbContext _context;

    // Injeção de Dependência: A API entrega a conexão do banco pronta para uso
    public EstabelecimentosController(AppDbContext context)
    {
        _context = context;
    }

    // Rota GET: Retorna todos os estabelecimentos do banco
    [HttpGet]
    public async Task<IActionResult> GetEstabelecimentos()
    {
        // Operação assíncrona para não travar a thread do servidor IIS/Kestrel
        var dados = await _context.Estabelecimentos.ToListAsync();
        return Ok(dados); // Retorna HTTP 200 com o JSON
    }

    // Rota POST: Insere um novo estabelecimento no banco
    [HttpPost]
    public async Task<IActionResult> PostEstabelecimento(Estabelecimento estabelecimento)
    {
        _context.Estabelecimentos.Add(estabelecimento);
        await _context.SaveChangesAsync(); // Efetiva o comando INSERT no SQL Server
        
        // Retorna HTTP 201 (Created) e o dado recém-criado com o ID gerado
        return CreatedAtAction(nameof(GetEstabelecimentos), new { id = estabelecimento.Id }, estabelecimento);
    }
}