using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrutosDeGoias.Api.Data;

namespace FrutosDeGoias.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProducoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducoes()
        {
            // PASSO 1: Agrupamento massivo executado estritamente no motor SQL
            var agregacaoBanco = await _context.Producoes
                .GroupBy(p => new { p.Cidade, p.Fruta })
                .Select(g => new {
                    CidadeRaw = g.Key.Cidade,
                    Fruta = g.Key.Fruta,
                    SomaTotal = g.Sum(x => x.QuantidadeToneladas)
                })
                .ToListAsync(); 

            if (!agregacaoBanco.Any())
            {
                return NotFound("Nenhum dado de produção encontrado no banco de dados.");
            }

            // PASSO 2: Formatação de dicionário executada na memória (apenas sobre os dados já reduzidos)
            var resultadoAgrupado = agregacaoBanco
                .GroupBy(a => a.CidadeRaw.Replace(" (GO)", "").Trim())
                .Select(g => new {
                    Cidade = g.Key,
                    TotalGeral = g.Sum(x => x.SomaTotal),
                    Frutas = g.ToDictionary(x => x.Fruta, x => x.SomaTotal)
                })
                .ToList();

            return Ok(resultadoAgrupado);
        }
    }
}