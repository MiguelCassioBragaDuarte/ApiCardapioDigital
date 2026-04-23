namespace ApiCardapioDigital.DTOs
{
    public class ItemCreateDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public bool Disponivel { get; set; } = true;
    }
}
