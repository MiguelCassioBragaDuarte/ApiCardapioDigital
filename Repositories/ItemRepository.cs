using ApiCardapioDigital.Data;
using ApiCardapioDigital.Models;
using ApiCardapioDigital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCardapioDigital.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        // Construtor com injeção de dependência do DbContext
        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET ALL
        // ============================
        // Retorna todos os Item do banco
        public async Task<List<Item>> GetAll()
            => await _context.Items.ToListAsync();

        // ============================
        // GET BY ID
        // ============================
        // Busca um Item pelo ID
        public async Task<Item> GetById(int id)
            => await _context.Items.FindAsync(id);

        // ============================
        // ADD (CREATE)
        // ============================
        // Adiciona um novo Item no banco
        public async Task Add(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync(); // Salva no banco
        }

        // ============================
        // UPDATE
        // ============================
        // Atualiza um item existente
        public async Task Update(Item item)
        {
            // Busca o item existente no banco
            var existente = await _context.Items.FindAsync(item.Id);

            // Verifica se o item existe
            if (existente == null)
                throw new KeyNotFoundException($"Item com ID {item.Id} não encontrado.");

            // Atualiza os campos
            existente.Nome = item.Nome;
            existente.Descricao = item.Descricao;
            existente.Preco = item.Preco;
            existente.Categoria = item.Categoria;
            existente.Disponivel = item.Disponivel;

            try
            {
                // Salva alterações no banco
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Tratamento de erro do banco
                throw new Exception("Erro ao atualizar item no banco de dados.", ex);
            }
        }

        // ============================
        // DELETE
        // ============================
        // Remove um item do banco
        public async Task Delete(int id)
        {
            // Busca o item
            var item = await GetById(id);

            // Verifica se existe
            if (item == null)
                throw new KeyNotFoundException($"Item com ID {id} não encontrado.");

            try
            {
                // Remove o item
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao deletar item no banco de dados.", ex);
            }
        }
    }
}
