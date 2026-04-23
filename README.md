# 🍴 API Cardápio Digital

API robusta para gerenciamento de itens de cardápio de restaurantes, desenvolvida com **ASP.NET Core** e **SQLite**. A aplicação utiliza o padrão DTO para transferência de dados e possui tratamento global de exceções.

## 🚀 Como Executar o Projeto

### 1. Pré-requisitos
* [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
* Ferramenta de banco de dados SQLite (opcional, para visualização externa)

### 2. Instalação e Execução
```
# Clone o repositório
git clone https://github.com/seu-usuario/api-cardapio-digital.git

# Entre na pasta
cd ApiCardapioDigital

# Restaure as dependências
dotnet restore

# Execute as Migrations (se houver) para criar o banco SQLite
dotnet ef database update

# Rode a aplicação
dotnet run
```

---

## 📖 Documentação dos Endpoints

A URL base para todas as chamadas é: `/api/ApiCardapio`

### 📋 Listar Itens
* **Método:** `GET`
* **Descrição:** Retorna todos os itens cadastrados formatados via `ItemReadDTO`.
* **Sucesso:** `200 OK`

### 🔍 Buscar por ID
* **Método:** `GET`
* **URL:** `/api/ApiCardapio/{id}`
* **Erros possíveis:** `404 Not Found` (ID inexistente).

### ➕ Cadastrar Item
* **Método:** `POST`
* **Corpo (JSON):**
    ```json
    {
      "nome": "Nhoque ao Sugo",
      "descricao": "Massa artesanal de batata com molho de tomate italiano.",
      "preco": 42.00,
      "categoria": "Massas",
      "disponivel": true
    }
    ```
    > **Obs:** O campo `Id` não deve ser enviado (autoincremento pelo SQLite).

### 📝 Atualizar Item
* **Método:** `PUT`
* **URL:** `/api/ApiCardapio/{id}`
* **Regra:** O `id` da URL deve ser idêntico ao `id` no corpo do JSON.

### 🗑️ Remover Item
* **Método:** `DELETE`
* **URL:** `/api/ApiCardapio/{id}`
* **Sucesso:** `204 No Content`

---

## ⚠️ Padrão de Resposta de Erro

Em caso de falhas críticas (Status 500) ou erros de validação, a API retorna um objeto padronizado para facilitar o tratamento no Front-end:

```json
{
    "erro": "ERRO_INTERNO",
    "mensagem": "Erro ao processar requisição.",
    "detalhe": "Mensagem técnica da exceção (apenas em ambiente de dev)"
}
```

### Códigos de Erro Comuns:
* `ERRO_INTERNO`: Falha na comunicação com o banco ou erro inesperado.
* `NAO_ENCONTRADO`: O recurso solicitado não existe.
* `VALIDACAO`: Erro na regra de negócio (ex: IDs divergentes no PUT).

---

## 🏗️ Arquitetura de Dados

O projeto utiliza **DTOs** para proteger a entidade principal e controlar o que é exposto/recebido:
* `ItemCreateDTO`: Dados necessários para criação.
* `ItemUpdateDTO`: Inclui o ID para validação de atualização.
* `ItemReadDTO`: Formato de saída para o cliente.
