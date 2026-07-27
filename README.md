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

---

## ⚙️ Como Executar o Projeto Localmente

### Pré-requisitos
Certifique-se de ter instalado em sua máquina:
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Python 3.x](https://www.python.org/) com as bibliotecas `pandas`, `pyodbc` e `openpyxl`
* Microsoft SQL Server (ou SQL Server Express)

### Passo 1: Clonar o Repositório
```bash
git clone [https://github.com/SEU-USUARIO/FrutosDeGoias.Api.git](https://github.com/SEU-USUARIO/FrutosDeGoias.Api.git)
cd FrutosDeGoias.Api
```

### Passo 2: Configurar o Banco de Dados e Rodar o ETL
1. Certifique-se de que o arquivo `producao.csv` está na raiz do projeto.
2. Crie um banco de dados no seu SQL Server chamado `FrutosDeGoiasDb`.
3. Instale as dependências do Python e execute o script de importação para popular o banco relacional:
```bash
pip install pandas pyodbc openpyxl
python importar_dados.py
```

### Passo 3: Configurar a String de Conexão
No arquivo `appsettings.json` da API, garanta que a string de conexão aponte corretamente para o seu SQL Server local:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FrutosDeGoiasDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### Passo 4: Iniciar a API
Execute o comando abaixo na pasta raiz do projeto .NET para compilar e iniciar o servidor web:
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