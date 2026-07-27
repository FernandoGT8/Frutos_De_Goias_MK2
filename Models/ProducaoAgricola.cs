namespace FrutosDeGoias.Api.Models;

public class ProducaoAgricola
{
    public int Id { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public string Fruta { get; set; } = string.Empty;
    
    // Armazena a quantidade em toneladas, utilizando decimal para garantir precisão matemática
    public decimal QuantidadeToneladas { get; set; } 
}