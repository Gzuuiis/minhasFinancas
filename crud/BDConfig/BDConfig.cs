using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace crud.Config
{
    public class BDConfig
    {
        public string DBServer { get; }
        public string DBNome { get; }
        public string DBUsuario { get; }
        public string DBSenha { get; }
        public bool AutenticacaoWindows { get; }

        public BDConfig()
        {
            DBServer = ConfigurationManager.AppSettings["FD_DB_HOST"];
            DBNome = ConfigurationManager.AppSettings["FD_DB_NAME"];
            DBUsuario = ConfigurationManager.AppSettings["FD_DB_USER"];
            DBSenha = ConfigurationManager.AppSettings["FD_DB_PASS"];
            AutenticacaoWindows = bool.TryParse(ConfigurationManager.AppSettings["FD_DB_WIN_AUT"], out bool result) && result;
        }
    }

    public class BDConexao
    {

        private readonly string _connectionString;

        public BDConexao(BDConfig config)
        {
            _connectionString = config.AutenticacaoWindows
                ? $"Server={config.DBServer};Database={config.DBNome};Integrated Security=True;"
                : $"Server={config.DBServer};Database={config.DBNome};User Id={config.DBUsuario};Password={config.DBSenha};";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private SqlConnection abrirConexao()
        {
            SqlConnection conexao = new SqlConnection(_connectionString);
            conexao.Open();

            return conexao;
        }

        public void TestarConexao()
        {
            using (SqlConnection conn = abrirConexao())
            {
                try
                {
                    Console.WriteLine("Conexão bem-sucedida!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao conectar: {ex.Message}");
                }
            }
        }

        // Executa uma consulta SELECT e exibe os resultados.
         //Necessário colocar List<Dictionary> devido a necessitarmos retornar um dicionário, correspondente
         // do array associativo em PHP
        public List<Dictionary<string, object>> execQuery(string query, SqlParameter[] parametros)
        {
            var resultados = new List<Dictionary<string, object>>();  

            using (SqlConnection conn = abrirConexao())
            {
                try
                {
                    SqlCommand comando = new SqlCommand(query, conn);
          
                       // Bindando valores
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);

                    // Executa a consulta e obtém o SqlDataReader
                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        // Verifica se há registros
                        if (!leitor.HasRows)
                        {
                            Console.WriteLine("Nenhum registro encontrado.");
                            return resultados;  // Retorna a lista vazia se não houver registros
                        }

                        // Lê os registros e armazena os dados na lista
                        while (leitor.Read())
                        {
                            var registro = new Dictionary<string, object>();

                            for (int i = 0; i < leitor.FieldCount; i++)
                            {
                                // Adiciona o nome da coluna e o valor no dicionário
                                registro[leitor.GetName(i)] = leitor[i];
                            }

                            // Adiciona o registro à lista
                            resultados.Add(registro);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao executar consulta: {ex.Message}");
                }
            }

            return resultados; 
        }


        // Executa uma NonQuery e exibe a quantidade de linhas afetadas.
        // Executa uma NonQuery com parâmetros e exibe a quantidade de linhas afetadas.
        public int execNonQuery(string query, SqlParameter[] parametros)
        {
            using (SqlConnection conn = abrirConexao())
            {
                try
                {
                    SqlCommand comando = new SqlCommand(query, conn);

                    // Adiciona os parâmetros de forma segura
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);

                    int linhasAfetadas = comando.ExecuteNonQuery();
                    return linhasAfetadas;

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error ao executar non-query: {e.Message}");
                    return 0;
                }
            }
        }

    }
}
