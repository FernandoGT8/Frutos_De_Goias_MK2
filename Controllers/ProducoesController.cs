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
            var dadosBrutos = await _context.Producoes.ToListAsync();
            
            if (!dadosBrutos.Any())
            {
                return NotFound("Nenhum dado de produção encontrado no banco de dados.");
            }

            // Agrupa os dados por cidade para o front-end consumir de forma limpa
            var resultadoAgrupado = dadosBrutos
                .GroupBy(p => p.Cidade.Replace(" (GO)", "").Trim())
                .Select(g => new {
                    Cidade = g.Key,
                    TotalGeral = g.Sum(x => x.QuantidadeToneladas),
                    Frutas = g.ToDictionary(x => x.Fruta, x => x.QuantidadeToneladas)
                })
                .ToList();

            return Ok(resultadoAgrupado);
        }
    }
}