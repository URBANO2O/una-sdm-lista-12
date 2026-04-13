namespace NikeStoreApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public int QuantidadeItens { get; set; }
    }
}