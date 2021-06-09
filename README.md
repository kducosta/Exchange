# Exchange
**API web em ASP.NET Core para conversão monetária**

O serviço prover conversão monetária de pelos menos 4 moedas (BRL, USD, EUR e JPY).

Cada transação é feita por um usuário logado e o seviço armazena um histórico das transações realizadas. Esse histório contem:
* O Id da transação;
* O Id do usuário logado;
* As moedas de origem e destino;
* Os valores de origem e destino;
* A taxa usada na conversão;
* O dia e a hora que foi feita a conversão (UTC).

A documentação da API está disponível em /swagger/index.html

# Principais tecnologias

A solução foi construida em dois projeto um "Core" responsável pela persistência das informações e a lógica de negócio e uma API Web do ASP.Net Core.

**Core**

O armazenamento das informações do serviço é gerenciado pelo Entity Framework Core e persistidas em um banco (embedded) SQLite. Entity Framework também gerencia as migrações do banco de dados.
Gerencia o cadastro de usuários e a geração de JWT para autenticação no serviço usando o Identity Framework Core.
Implementa o serviço que acessa o serviço de conversão monetária (https://exchangeratesapi.io/).

**API**

Implementa os endpoints da API Web e prover os clientes HTTP (para acessar o endpoint da Exchange Rates API).

Os controladores são implementados ASP.Net Core e gera a documentação da API da Rest usando Swashbunkle a partir da documetação do código. 

# Executando

Exchange está implentado em .Net Core (https://dotnet.microsoft.com/download), com o framework instalado, e na pasta da soluction execute:

Baixa as dependências no NuGet
```console
dotnet restore
```
Execute as migrações do banco de dados:
```console
dotnet ef database update --project ./Exchange/Exchange.csproj --startup-project ./Exchange.Api/Exchange.Api.csproj
```

Execute o projeto:

```console
dotnet ef database update --project ./Exchange.Api/Exchange.Api.csproj
```
