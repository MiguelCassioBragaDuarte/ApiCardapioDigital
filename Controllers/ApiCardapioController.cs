using ApiCardapioDigital.DTOs;
using ApiCardapioDigital.Models;
using ApiCardapioDigital.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCardapioDigital.Controllers
{
    /// <summary>
    /// API de Controle de Cardápio de Restaurante.
    /// </summary>
    /// <remarks>
    /// Esta controller gerencia o CRUD dos itens do cardápio.
    /// 
    /// **Padrão de Erro Crítico (500):**
    /// 
    ///     {
    ///         "erro": "ERRO_INTERNO",
    ///         "mensagem": "Erro ao processar requisição.",
    ///         "detalhe": "Mensagem técnica da exceção"
    ///     }
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ApiCardapioController : ControllerBase
    {
        private readonly ItemService _service;

        public ApiCardapioController(ItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os itens do cardápio.
        /// </summary>
        /// <remarks>
        /// **Tipo de Envio:** GET (sem corpo).  
        /// **Retorno:** Lista de itens cadastrados.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var itens = await _service.Listar();
                var resultado = itens.Select(a => new ItemReadDTO
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Descricao = a.Descricao,
                    Preco = a.Preco,
                    Categoria = a.Categoria,
                    Disponivel = a.Disponivel
                });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "ERRO_INTERNO", mensagem = "Erro ao listar itens.", detalhe = ex.Message });
            }
        }

        /// <summary>
        /// Busca um item específico pelo ID.
        /// </summary>
        /// <param name="id">Identificador único do item.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _service.ObterPorId(id);
                if (item == null)
                    return NotFound(new { erro = "NAO_ENCONTRADO", mensagem = $"ID {id} não existe." });

                var dto = new ItemReadDTO
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Descricao = item.Descricao,
                    Preco = item.Preco,
                    Categoria = item.Categoria,
                    Disponivel = item.Disponivel
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "ERRO_INTERNO", mensagem = "Erro ao buscar item.", detalhe = ex.Message });
            }
        }

        /// <summary>
        /// Cadastra um novo item no cardápio.
        /// </summary>
        /// <remarks>
        /// **Tipo de Envio:** POST (Body JSON).
        /// 
        /// **Atenção:** O campo 'Id' NÃO deve ser enviado, pois é gerado automaticamente pelo banco de dados (SQLite).
        /// 
        /// **Exemplo de Envio:**
        /// 
        ///     {
        ///        "nome": "Nhoque ao Sugo",
        ///        "descricao": "Massa artesanal de batata com molho de tomate italiano.",
        ///        "preco": 42.00,
        ///        "categoria": "Massas",
        ///        "disponivel": true
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ItemCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var item = new Item
                {
                    Nome = dto.Nome,
                    Descricao = dto.Descricao,
                    Preco = dto.Preco,
                    Categoria = dto.Categoria,
                    Disponivel = dto.Disponivel
                };

                await _service.Criar(item);
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "ERRO_INTERNO", mensagem = "Erro ao criar item.", detalhe = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza os dados de um item existente.
        /// </summary>
        /// <remarks>
        /// **Tipo de Envio:** PUT (ID na URL e no Body).
        /// 
        /// **Exemplo de Envio:**
        /// 
        ///     {
        ///        "id": 1,
        ///        "nome": "Nhoque ao Sugo (Individual)",
        ///        "descricao": "Massa artesanal de batata.",
        ///        "preco": 39.00,
        ///        "categoria": "Massas",
        ///        "disponivel": false
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] ItemUpdateDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { erro = "VALIDACAO", mensagem = "O ID da URL deve ser igual ao ID do corpo." });

                var item = await _service.ObterPorId(id);
                if (item == null)
                    return NotFound(new { erro = "NAO_ENCONTRADO", mensagem = $"Item {id} não encontrado." });

                item.Nome = dto.Nome;
                item.Descricao = dto.Descricao;
                item.Preco = dto.Preco;
                item.Categoria = dto.Categoria;
                item.Disponivel = dto.Disponivel;

                await _service.Atualizar(item);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "ERRO_INTERNO", mensagem = "Erro ao atualizar item.", detalhe = ex.Message });
            }
        }

        /// <summary>
        /// Remove um item do cardápio permanentemente por ID.
        /// </summary>
        /// <param name="id">Identificador único do item a ser excluído.</param>
        /// <remarks>
        /// **Tipo de Envio:** DELETE via URL (Path Parameter).
        /// 
        /// **Resultados Esperados:**
        /// * **204 No Content:** O item foi encontrado e excluído com sucesso.
        /// * **404 Not Found:** O ID informado não corresponde a nenhum item no banco de dados.
        /// * **500 Internal Server Error:** Falha ao acessar o banco de dados SQLite.
        /// 
        /// **Exemplo de Chamada:**
        /// 
        ///     DELETE /api/ApiCardapio/1
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Busca o item para verificar existência antes de deletar
                var item = await _service.ObterPorId(id);

                if (item == null)
                    return NotFound(new
                    {
                        erro = "NAO_ENCONTRADO",
                        mensagem = $"Não foi possível excluir: Item com ID {id} não encontrado."
                    });

                // Remove do banco de dados (SQLite)
                await _service.Deletar(id);

                // Retorna 204 (Sucesso sem corpo de resposta)
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = "ERRO_INTERNO",
                    mensagem = "Erro ao tentar deletar o item.",
                    detalhe = ex.Message
                });
            }
        }
    }
}