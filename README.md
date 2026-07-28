# 🍇 Mapeamento da Fruticultura em Goiás

> **Contexto do Projeto:** Projeto desenvolvido para atender a uma demanda real de visualização de dados, substituindo planilhas estáticas por uma arquitetura Client-Server desacoplada. O sistema une engenharia de dados, uma API robusta em .NET otimizada para alta performance e uma interface geográfica reativa.

---

## 🚀 Sobre o Projeto

O **Frutos de Goiás** é uma aplicação full-stack construída para analisar, processar e renderizar espacialmente dados da produção agrícola do estado de Goiás, baseada no Levantamento Sistemático da Produção Agrícola (PAM/IBGE). 

A arquitetura foi projetada com foco em performance e integridade, dividida em três pilares:
1. **Pipeline de ETL (Python):** Ingestão transacional e higienização de dados brutos tabulares (CSV) com inserção em lote (Bulk Insert) em banco relacional.
2. **Back-end (.NET Web API):** API RESTful construída em C# com Entity Framework Core. Otimizada para delegar agregações pesadas ao motor SQL, minimizando o uso de memória (RAM) no servidor de aplicação.
3. **Front-end (GIS interativo):** Mapa web baseado em Leaflet.js com renderização vetorial, *choropleth* (escala de cores dinâmica) e recálculo em tempo real de rankings via requisições assíncronas.

---

## 🛠️ Tecnologias Utilizadas

* **Back-end:** C#, .NET 10.0, ASP.NET Core Web API, Entity Framework Core (Fluent API)
* **Banco de Dados:** Microsoft SQL Server (Indexação B-Tree)
* **Engenharia de Dados (ETL):** Python 3, Pandas, PyODBC
* **Front-end:** HTML5, CSS3, JavaScript (ES6+), Leaflet.js, Bootstrap

---

## 🧠 Decisões de Arquitetura e Engenharia

Para garantir que a aplicação escale e responda em milissegundos, as seguintes otimizações foram implementadas:

* **Deslocamento de Carga (Application vs. Database):** Todo o agrupamento de dados (`GroupBy`) foi mapeado e traduzido via LINQ para ser executado diretamente no motor do SQL Server, evitando o tráfego de milhares de linhas ociosas pela rede e prevenindo *Memory Leaks* na aplicação.
* **Índices Compostos e Tipagem Estrita:** Uso de Fluent API no EF Core para evitar colunas `NVARCHAR(MAX)` e garantir precisão matemática com `Decimal(18,2)`. Criação de um Índice Composto (`Composite Index`) para as colunas `Cidade` e `Fruta`, reduzindo a complexidade de busca de O(N) para O(log N).
* **Transações ACID no ETL:** O script de importação em Python executa a limpeza e inserção dos dados dentro de um bloco transacional unificado. Em caso de falha de I/O ou tipagem, um *Rollback* automático é acionado, impedindo estados corrompidos no banco.
* **True Bulk Insert:** Configuração de `fast_executemany = True` no PyODBC, convertendo milhares de instruções sequenciais em uma única operação de memória, reduzindo o tempo de ingestão de forma drástica.

---

## 🏗️ Arquitetura do Sistema

[ Arquivo CSV do IBGE ] 
       │
       ▼ (Script Python / Pandas / PyODBC / ACID Transactions)
[ Banco SQL Server (dbo.Producoes) ] 
       │
       ▼ (Entity Framework Core / LINQ / B-Tree Indexing)
[ ASP.NET Core Web API (/api/producoes) ] 
       │
       ▼ (Fetch assíncrono / JSON)
[ Front-end (Leaflet.js + Filtros Dinâmicos) ]

## Evolução Arquitetural (Versão MK2)

Esta versão (MK2) representa uma reformulação arquitetural estrutural em relação à concepção original do projeto. O objetivo da refatoração foi eliminar gargalos de escalabilidade, tratamento manual de arquivos e acoplamento de responsabilidades, elevando a aplicação a um padrão de mercado. 

As principais decisões de engenharia incluem:

* **Pipeline de Dados Automático (ETL):** Substituição da inserção manual de dados por um script Python dedicado (`pandas` e `pyodbc`). Este pipeline realiza a extração, limpeza (higienização de *encoding* e formatos) e carga (Load) direta do arquivo `.csv` para o banco de dados.
* **Desacoplamento e Tipagem Forte:** Adoção do framework .NET 10 para a construção de uma Web API Restful estrita. O uso do C# garante tipagem forte e maior previsibilidade em tempo de compilação.
* **Persistência Relacional:** Transição de dados estáticos para um banco de dados relacional estruturado (SQL Server), com mapeamento objeto-relacional gerenciado via Entity Framework Core (Code-First), garantindo integridade referencial entre as entidades de Estabelecimentos e Produções.

---

## ⚙️ Como Executar o Projeto Localmente

### Pré-requisitos
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Python 3.x](https://www.python.org/) com as bibliotecas `pandas`, `pyodbc` e `openpyxl`
* Microsoft SQL Server (ou Express) rodando localmente.

### Passo 1: Clonar e Configurar
git clone https://github.com/SEU-USUARIO/FrutosDeGoias.Api.git
cd FrutosDeGoias.Api

No arquivo `appsettings.json` da API, valide a string de conexão apontando para o seu servidor local:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FrutosDeGoiasDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

### Passo 2: Inicializar o Banco de Dados (Migrations)
Na pasta raiz do projeto .NET, aplique as configurações físicas e os índices no SQL Server:
dotnet ef database update

### Passo 3: Executar o Pipeline ETL
Certifique-se de que o arquivo `producao.csv` está na raiz do projeto, instale as dependências e rode a importação:
pip install pandas pyodbc openpyxl
python importar_dados.py

### Passo 3: Criar o Schema e Popular o Banco (ETL)
1. Primeiro, aplique as migrações do Entity Framework para gerar as tabelas estruturadas via C#:
```bash
dotnet ef database update
```
2. Em seguida, instale as dependências do Python e execute o script de importação para ler o `producao.csv` e popular o banco relacional:
```bash
pip install pandas pyodbc
python importar_dados.py
```

### Passo 4: Iniciar a API
Compile e inicie o servidor web:
dotnet run

* **Interface do Mapa (Front-end):** Acesse `http://localhost:5098/index.html`
* **Documentação Swagger (API):** Acesse `http://localhost:5098/swagger`

---

## 👨‍💻 Autor

Desenvolvido por **Fernando Pimenta**, graduando em Ciência da Computação (FacUnicamps), com foco em desenvolvimento Back-end e infraestrutura de software.