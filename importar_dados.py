import pandas as pd
import pyodbc

# 1. Leitura do CSV bruto do IBGE (pulando os cabeçalhos descritivos e pegando a linha 4 como header)
print("Lendo o arquivo CSV...")
df_raw = pd.read_csv("producao.csv", sep=";", header=4)

# 2. Limpeza e transformação (Removendo colunas de totais irrelevantes e convertendo para formato longo)
# As colunas de frutas começam a partir da terceira coluna (índice 2)
frutas_colunas = df_raw.columns[2:]
municipio_col = df_raw.columns[0]

dados_para_inserir = []

for index, row in df_raw.iterrows():
  cidade = str(row[municipio_col]).strip()
  if not cidade or cidade == "nan":
    continue

  for fruta in frutas_colunas:
    valor_str = str(row[fruta]).strip()

    # Ignora valores nulos, hífens ou dados não informados ('..')
    if valor_str in ["-", "..", "nan", ""]:
      continue

    try:
      # Limpa formatação numérica e converte para decimal
      quantidade = float(valor_str.replace(",", "."))
      if quantidade > 0:
        dados_para_inserir.append((cidade, fruta.replace("*", ""), quantidade))
    except ValueError:
      continue

print(f"Total de registros processados para inserção: {len(dados_para_inserir)}")

# 3. Conexão com o SQL Server Management Studio (SSMS)
print("Conectando ao banco de dados SQL Server...")
conn_str = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=localhost\\SQLEXPRESS;"
    "DATABASE=FrutosDeGoiasDb;"
    "Trusted_Connection=yes;"
)

conn = pyodbc.connect(conn_str)
cursor = conn.cursor()

# Limpa dados antigos para evitar duplicação em execuções repetidas
cursor.execute("DELETE FROM Producoes")
conn.commit()

# 4. Inserção em lote no Banco de Dados
print("Inserindo dados na tabela dbo.Producoes...")
query = (
    "INSERT INTO Producoes (Cidade, Fruta, QuantidadeToneladas) VALUES (?, ?, ?)"
)
cursor.executemany(query, dados_para_inserir)
conn.commit()

cursor.close()
conn.close()
print("Importação concluída com sucesso!")