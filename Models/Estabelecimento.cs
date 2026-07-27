namespace FrutosDeGoias.Api.Models;

public class Estabelecimento
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Cidade { get; set; } = string.Empty;
}