
````markdown
# CharacterStats

CharacterStats é um projeto de estudo desenvolvido em C# e .NET 10 com temática de jogos. O projeto começou como uma modelagem simples de personagens e combate e evoluiu para uma Minimal API capaz de consultar personagens e executar ataques por meio de requisições HTTP.

O objetivo principal é praticar conceitos de orientação a objetos, encapsulamento, regras de domínio, serviços, testes unitários, Minimal APIs, serialização JSON e testes de integração com xUnit e WebApplicationFactory.

## Tecnologias utilizadas

- C#
- .NET 10
- ASP.NET Core Minimal API
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- Swagger
- LINQ
- JSON
- WebApplicationFactory

## Estrutura da solução

```text
CharacterStats
├── CharacterStats
│   ├── Models
│   │   └── Character.cs
│   └── Services
│       └── CharacterStatsService.cs
│
├── CharacterStats.Api
│   ├── Contracts
│   │   ├── AttackRequest.cs
│   │   └── AttackResponse.cs
│   └── Program.cs
│
└── CharacterStats.Tests
    ├── CharacterTests.cs
    ├── CharacterStatsServiceTests.cs
    └── CharacterStatsApiTests.cs
```

## Domínio

A entidade `Character` representa um personagem do jogo e possui as seguintes propriedades:

- `Id`
- `Name`
- `MaxHealth`
- `CurrentHealth`
- `AttackPower`
- `DefensePower`
- `IsAlive`

A entidade protege seu próprio estado e possui métodos para:

- Receber dano;
- Recuperar vida;
- Impedir valores inválidos;
- Limitar a vida mínima a zero;
- Limitar a cura à vida máxima;
- Informar se o personagem ainda está vivo.

## Regras de validação

As principais regras implementadas são:

- O ID deve ser maior que zero;
- O nome não pode ser nulo, vazio ou composto apenas por espaços;
- A vida máxima deve ser maior que zero;
- O poder de ataque deve ser maior que zero;
- A defesa pode ser zero, mas não pode ser negativa;
- O dano não pode ser negativo;
- A cura não pode ser negativa;
- A vida nunca pode ficar abaixo de zero;
- A vida nunca pode ultrapassar a vida máxima;
- Um personagem com vida igual a zero é considerado morto.

## CharacterStatsService

O `CharacterStatsService` coordena a execução de ataques entre personagens.

O método `PerformAttack`:

1. Verifica se o atacante está vivo;
2. Verifica se o defensor está vivo;
3. Calcula o dano com base no ataque e na defesa;
4. Garante que o dano mínimo seja zero;
5. Aplica o dano ao defensor;
6. Retorna o dano efetivamente causado.

A fórmula utilizada é:

```text
dano = máximo entre (AttackPower - DefensePower) e zero
```

Exemplo:

```text
AttackPower: 40
DefensePower: 15
Dano causado: 25
```

Quando o ataque é menor ou igual à defesa, o dano causado é zero.

## API

A API utiliza ASP.NET Core Minimal API e mantém os personagens em uma coleção temporária em memória.

Isso significa que:

- Os dados são reiniciados quando a aplicação é reiniciada;
- Não existe persistência em banco de dados nesta versão;
- As alterações de vida permanecem apenas durante a execução atual da API.

### Listar personagens

```http
GET /characters
```

Retorna todos os personagens registrados em memória.

### Buscar personagem por ID

```http
GET /characters/{id}
```

Exemplo:

```http
GET /characters/1
```

Quando o personagem existe, a API retorna:

```text
200 OK
```

com os dados serializados em JSON.

Quando o personagem não existe, retorna:

```text
404 Not Found
```

### Executar ataque

```http
POST /characters/{attackerId}/attack
```

Exemplo:

```http
POST /characters/2/attack
```

Corpo da requisição:

```json
{
  "defenderId": 1
}
```

A resposta de sucesso contém:

```json
{
  "attackerId": 2,
  "defenderId": 1,
  "damage": 25,
  "remainingHealth": 75,
  "defenderIsAlive": true
}
```

Quando o atacante ou defensor não existe, a API retorna:

```text
404 Not Found
```

Quando o atacante ou defensor está morto, a operação entra em conflito com o estado atual do combate e retorna:

```text
409 Conflict
```

## Contracts

Os contratos representam os formatos de comunicação da API.

`AttackRequest` representa os dados recebidos pelo endpoint:

```json
{
  "defenderId": 1
}
```

`AttackResponse` representa os dados retornados depois do ataque:

- `AttackerId`
- `DefenderId`
- `Damage`
- `RemainingHealth`
- `DefenderIsAlive`

Esses tipos são separados da entidade `Character` para evitar que o formato interno do domínio seja confundido com o contrato HTTP da aplicação.

## Testes unitários

Os testes unitários verificam o comportamento isolado da entidade e do serviço.

São cobertos cenários como:

- Criação válida de personagem;
- Validação de ID;
- Validação de nome;
- Vida inicial igual à vida máxima;
- Dano normal;
- Dano superior à vida atual;
- Dano zero;
- Dano negativo;
- Cura normal;
- Cura acima da vida máxima;
- Cura negativa;
- Estado `IsAlive`;
- Ataques com dano positivo;
- Ataques com dano zero;
- Atacante morto;
- Defensor morto.

## Testes de integração

Os testes de integração utilizam:

- xUnit;
- `WebApplicationFactory<Program>`;
- `HttpClient`;
- `HttpStatusCode`;
- `System.Net.Http.Json`.

Eles inicializam a API em memória e fazem requisições HTTP reais contra os endpoints.

Cenários cobertos:

- `GET /characters/1` retorna `200 OK`;
- `GET /characters/999` retorna `404 Not Found`;
- Ataque com dano positivo retorna `200 OK`;
- Ataque com dano zero retorna `200 OK`;
- Atacante inexistente retorna `404 Not Found`;
- Defensor inexistente retorna `404 Not Found`.

Os testes verificam tanto o status HTTP quanto os dados retornados no JSON.

## Como executar

Para executar a aplicação:

```bash
dotnet run --project CharacterStats.Api
```

Depois, acesse o Swagger pela URL exibida no terminal, normalmente semelhante a:

```text
https://localhost:7046/swagger
```

Para executar todos os testes:

```bash
dotnet test
```

Para compilar a solução:

```bash
dotnet build
```

## Aprendizados principais

Este projeto permitiu praticar:

- Modelagem de entidades;
- Encapsulamento;
- Validação de estado;
- Regras de combate;
- Separação entre entidade e serviço;
- Minimal APIs;
- Rotas HTTP;
- Parâmetros de rota;
- Corpo JSON;
- Model binding;
- Serialização e desserialização;
- Status codes;
- DTOs e Contracts;
- Testes unitários;
- Testes parametrizados;
- Testes de exceção;
- Testes de integração;
- Uso de `WebApplicationFactory`;
- Isolamento de testes;
- Organização de uma solução com múltiplos projetos.

## Próximos passos possíveis

As próximas evoluções naturais do projeto são:

- Adicionar persistência com Entity Framework Core;
- Criar um banco de dados;
- Separar a lógica de consulta em serviços;
- Criar testes de integração para mais cenários;
- Adicionar criação de personagens pela API;
- Adicionar cura via endpoint;
- Adicionar experiência e níveis;
- Implementar tratamento global de exceções;
- Adicionar validações de request;
- Documentar melhor os contratos HTTP;
- Adicionar autenticação e autorização em uma etapa futura.

## Status atual

O projeto possui um domínio funcional de personagens e combate, uma Minimal API com endpoints de consulta e ataque, contratos separados para entrada e saída e uma suíte inicial de testes unitários e de integração.

Esta versão representa o primeiro MVP de backend do projeto CharacterStats.
````