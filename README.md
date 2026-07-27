# 🍇 Mapeamento da Fruticultura em Goiás

> **Contexto do Projeto:** Projeto acadêmico de faculdade desenvolvido para atender a uma demanda real de uma colega de trabalho, unindo engenharia de dados, uma API robusta em .NET e uma interface geográfica interativa de alta performance.

---

## 🚀 Sobre o Projeto

O **Frutos de Goiás** é uma aplicação full-stack desenvolvida para analisar, processar e visualizar espacialmente os dados de produção agrícola (fruticultura) do estado de Goiás com base nos dados oficiais do IBGE (PAM). 

O sistema substitui soluções estáticas por uma arquitetura moderna e desacoplada, composta por:
1. **Pipeline de ETL em Python**: Tratamento e carga de dados brutos tabulares para um banco de dados relacional.
2. **Back-end em .NET Web API**: Uma API limpa construída com C# e Entity Framework Core para gerenciamento e entrega de dados.
3. **Front-end Interativo**: Um mapa web baseado em Leaflet.js com renderização vetorial de polígonos municipais, escala de cores dinâmica (*choropleth*) baseada no volume de produção, filtros reativos por cultura agrícola e ranking dinâmico em tempo real.

---

## 🛠️ Tecnologias Utilizadas

* **Back-end:** C#, .NET 10.0, ASP.NET Core Web API, Entity Framework Core
* **Banco de Dados:** Microsoft SQL Server (SSMS)
* **Engenharia de Dados (ETL):** Python 3, Pandas, PyODBC
* **Front-end:** HTML5, CSS3, JavaScript Moderno (ES6+)
* **Geoprocessamento & UI:** Leaflet.js, Malha GeoJSON oficial do IBGE (Goiás - Código 52), Bootstrap

---

## 🏗️ Arquitetura do Sistema

```text
[ Arquivo CSV do IBGE ] 
       │
       ▼ (Script Python / Pandas / PyODBC)
[ Banco SQL Server (dbo.Producoes) ] 
       │
       ▼ (Entity Framework Core / LINQ)
[ ASP.NET Core Web API (/api/producoes) ] 
       │
       ▼ (Fetch assíncrono / JSON)
[ Front-end (Leaflet.js + Filtros Dinâmicos) ]
```

## Evolução Arquitetural (Versão MK2)

Esta versão (MK2) representa uma reformulação arquitetural estrutural em relação à concepção original do projeto. O objetivo da refatoração foi eliminar gargalos de escalabilidade, tratamento manual de arquivos e acoplamento de responsabilidades, elevando a aplicação a um padrão de mercado. 

As principais decisões de engenharia incluem:

* **Pipeline de Dados Automático (ETL):** Substituição da inserção manual de dados por um script Python dedicado (`pandas` e `pyodbc`). Este pipeline realiza a extração, limpeza (higienização de *encoding* e formatos) e carga (Load) direta do arquivo `.csv` para o banco de dados.
* **Desacoplamento e Tipagem Forte:** Adoção do framework .NET 10 para a construção de uma Web API Restful estrita. O uso do C# garante tipagem forte e maior previsibilidade em tempo de compilação.
* **Persistência Relacional:** Transição de dados estáticos para um banco de dados relacional estruturado (SQL Server), com mapeamento objeto-relacional gerenciado via Entity Framework Core (Code-First), garantindo integridade referencial entre as entidades de Estabelecimentos e Produções.

---

## ⚙️ Como Executar o Projeto Localmente

### Pré-requisitos
Certifique-se de ter instalado em sua máquina:
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Python 3.x](https://www.python.org/)
* Microsoft SQL Server (ou SQL Server Express)

### Passo 1: Clonar o Repositório
```bash
git clone [https://github.com/fernandogt8/FrutosDeGoias.Api.git](https://github.com/fernandogt8/FrutosDeGoias.Api.git)
cd FrutosDeGoias.Api
```

### Passo 2: Configurar a Conexão com o Banco
1. Crie um banco de dados vazio no seu SQL Server chamado `FrutosDeGoiasDb`.
2. No arquivo `appsettings.json`, garanta que a string de conexão aponte corretamente para o seu servidor local. Exemplo:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FrutosDeGoiasDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

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
Execute o comando abaixo na pasta raiz do projeto para compilar e iniciar o servidor web:
```bash
dotnet run
```

Acesse no navegador:
* **Interface do Mapa (Front-end):** `http://localhost:5098/index.html`
* **Documentação Swagger (API):** `http://localhost:5098/swagger`

---

## 📸 Funcionalidades da Interface

* **Filtros Multi-critério por Fruta:** Janela flutuante interativa com checkboxes para selecionar ou ocultar culturas específicas em tempo real.
* **Mapa Estilizado (*Choropleth*):** Os polígonos municipais de Goiás alteram automaticamente a intensidade do tom de azul com base proporcional no volume de produção agrícola.
* **Popups Informativos:** Clique sobre qualquer município para visualizar o detalhamento individual de cada fruta produzida e o somatório geral.
* **Ranking Dinâmico Top 10:** O painel lateral recalcula instantaneamente as cidades líderes de produção conforme os filtros são modificados.

---

## 👨‍💻 Autor

Desenvolvido por **Fernando Pimenta** como projeto prático universitário aplicado.
