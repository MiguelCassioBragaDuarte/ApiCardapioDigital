using ApiCardapioDigital.Models;
using ApiCardapioDigital.Repositories.Interfaces;

namespace ApiCardapioDigital.Services
{
    public class ItemService
    {
        private readonly IItemRepository _repo;

        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Retorna todos os itens do cardápio cadastrados.
        /// </summary>
        public async Task<List<Item>> Listar()
            => await _repo.GetAll();

        /// <summary>
        /// Retorna um item específico através do seu identificador único.
        /// </summary>
        public async Task<Item> ObterPorId(int id)
            => await _repo.GetById(id);

        /// <summary>
        /// Realiza a inclusão de um novo item no cardápio.
        /// </summary>
        public async Task Criar(Item item)
        {
            // Exemplo de regra de negócio: Garantir que o nome não seja nulo
            if (string.IsNullOrEmpty(item.Nome))
                throw new Exception("O nome do item é obrigatório para o cadastro.");

            await _repo.Add(item);
        }

        /// <summary>
        /// Atualiza as informações de um item existente.
        /// </summary>
        public async Task Atualizar(Item item)
            => await _repo.Update(item);

        /// <summary>
        /// Remove um item do cardápio permanentemente.
        /// </summary>
        public async Task Deletar(int id)
            => await _repo.Delete(id);
    }
}