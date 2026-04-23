using ApiCardapioDigital.Models;

namespace ApiCardapioDigital.Repositories.Interfaces
{
    public interface IItemRepository
    {
        // Retorna todos os itens cadastrados no banco de dados
        Task<List<Item>> GetAll();

        // Retorna um item específico pelo ID
        Task<Item> GetById(int id);

        // Adiciona um novo item no banco de dados
        Task Add(Item item);

        // Atualiza os dados de um item existente
        Task Update(Item item);

        // Remove um item do banco de dados pelo ID
        Task Delete(int id);
    }
}
