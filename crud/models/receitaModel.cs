using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using crud.Config;

namespace crud.models
{
    internal class receitaModel
    {
        private string str_nome_tabela;
        
        public int int_id { get; set; }
        public string str_nome { get; set; }
        public float flt_valor_recebido { get; set; }
        public DateTime dat_data_prevista { get; set; }
        public DateTime dat_data_recebimento { get; set; }
        public int int_id_usuario_fk { get; set; }
        public int int_id_mes_ano_referencia_fk { get; set; }

        public receitaModel()
        {
            str_nome_tabela = "receita";
        }


        public string execCriarReceita(string str_nome, float flt_valor_recebido, DateTime dat_data_prevista, DateTime dat_data_recebimento, int int_id_usuario_fk, int int_id_mes_ano_referencia_fk)
        {

            var config = new BDConfig();
            var conexao = new BDConexao(config);

            // Definindo a query
            string query = "INSERT INTO " + str_nome_tabela + " (str_nome, flt_valor_recebido, dat_data_prevista, dat_data_recebimento, int_id_usuario_fk, int_id_mes_ano_referencia_fk) " +
                           "VALUES (@str_nome, @flt_valor_recebido, @dat_data_prevista, @dat_data_recebimento, @int_id_usuario_fk, @int_id_mes_ano_referencia_fk)";

            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@str_nome", str_nome),     
                new SqlParameter("@flt_valor_recebido", flt_valor_recebido),
                new SqlParameter("@dat_data_prevista", dat_data_prevista),
                new SqlParameter("@dat_data_recebimento", dat_data_recebimento),
                new SqlParameter("@int_id_usuario_fk", int_id_usuario_fk),
                new SqlParameter("@int_id_mes_ano_referencia_fk", int_id_mes_ano_referencia_fk)
                };

            conexao.execNonQuery(query, parametros);

            return conexao.ToString();

        }

        public List<Dictionary<string, object>> execRetornarReceita(Dictionary<string, object[]> parametros)
        {
            var config = new BDConfig();
            var conexao = new BDConexao(config);

            // Verificação para garantir que parâmetros foram passados
            if (parametros.Count == 0)
                throw new ArgumentException("É necessário fornecer pelo menos um parâmetro para a consulta.");

            // Monta a query base
            string query = $"SELECT * FROM {str_nome_tabela} WHERE ";

            // Criação das condições WHERE
            List<string> whereConditions = parametros.Select(param =>
            {
                string nomeParametro = param.Key;
                var operador = param.Value[0];  // Primeiro item do array: operador (ex.: "=", ">", "<")
                return $"{nomeParametro} {operador} @{nomeParametro}";
            }).ToList();

            // Junta as condições com AND
            query += string.Join(" AND ", whereConditions);

            // Criação dos parâmetros SQL (binds)
            var bindsList = parametros.Select(param =>
            {
                string nomeParametro = param.Key;
                var valorParametro = param.Value[1];  // Segundo item do array: valor do parâmetro
                return new SqlParameter($"@{nomeParametro}", valorParametro ?? DBNull.Value);
            }).ToList();

            SqlParameter[] binds = bindsList.ToArray();

            // Executa a query e retorna os resultados
            return conexao.execQuery(query, binds);
          }


        public int execAtualizarReceita(Dictionary<string, object[]> parametros, Dictionary<string, object> novosValores)
        {
            var config = new BDConfig();
            var conexao = new BDConexao(config);


            // Monta a parte do SET
            List<string> setValues = novosValores.Select(valor => $"{valor.Key} = @{valor.Key}").ToList();

            // Monta a parte do WHERE
            List<string> whereConditions = parametros.Select(param =>
            {
                string nomeParametro = param.Key;
                var operador = param.Value[0];  // operador (ex.: "=", ">", "<")
                return $"{nomeParametro} {operador} @{nomeParametro}_cond";
            }).ToList();

            // Monta a query final
            string query = $"UPDATE {str_nome_tabela} SET {string.Join(", ", setValues)} WHERE {string.Join(" AND ", whereConditions)};";

            // Cria os parâmetros para SET
            var bindsList = novosValores.Select(valor =>
                new SqlParameter($"@{valor.Key}", valor.Value ?? DBNull.Value)
            ).ToList();

            // Cria os parâmetros para WHERE
            bindsList.AddRange(parametros.Select(param =>
                new SqlParameter($"@{param.Key}_cond", param.Value[1] ?? DBNull.Value)
            ));

            SqlParameter[] binds = bindsList.ToArray();

            // Executa a query e retorna o número de linhas afetadas
             int resultado = conexao.execNonQuery(query, binds);
            Console.WriteLine($"Resultado da Model: {resultado}");

            return resultado;
        }


    }
}
