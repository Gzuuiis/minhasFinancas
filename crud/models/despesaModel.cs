using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using crud.Config;

namespace crud.models
{
    internal class despesaModel
    {
        private string str_nome_tabela;
        
        public int int_id { get; set; }
        public string str_nome { get; set; }
        public string str_tipo_despesa { get; set; }
        public int int_qtd_parcela { get; set; }
        public float flt_valor_previsto { get; set; }
        public float flt_valor_gasto { get; set; }
        public DateTime dat_data_vencimento { get; set; }
        public DateTime dat_data_pagamento { get; set; }
        public int int_usuario_id_fk { get; set; }
        public int int_meios_pagamento_id_fk { get; set; }
        public int int_mes_ano_referencia_id_fk { get; set; }

        public despesaModel()
        {
            str_nome_tabela = "despesas";
        }


        public string execCriarDespesa(string str_nome, string str_tipo_despesa, int int_qtd_parcela, float flt_valor_previsto, float flt_valor_gasto, DateTime dat_data_vencimento, DateTime dat_data_pagamento, int int_usuario_id_fk, int int_mes_ano_referencia_id_fk)
        {

            var config = new BDConfig();
            var conexao = new BDConexao(config);

            // Definindo a query
            string query = "INSERT INTO " + str_nome_tabela + " (str_nome, str_tipo_despesa, int_qtd_parcela, flt_valor_previsto, flt_valor_gasto, dat_data_vencimento, dat_data_pagamento, int_usuario_id_fk, int_mes_ano_referencia_id_fk)" +
                           "VALUES (@str_nome, @str_tipo_despesa, @flt_valor_previsto, @int_qtd_parcela, @flt_valor_gasto, @dat_data_vencimento, @dat_data_pagamento, @int_usuario_id_fk, @int_mes_ano_referencia_id_fk)";

            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@str_nome", str_nome),
                new SqlParameter("@str_tipo_despesa", str_tipo_despesa),
                new SqlParameter("@int_qtd_parcela", int_qtd_parcela),
                new SqlParameter("@flt_valor_previsto", flt_valor_previsto),
                new SqlParameter("@flt_valor_gasto", flt_valor_gasto),
                new SqlParameter("@dat_data_vencimento", dat_data_vencimento),
                new SqlParameter("@dat_data_pagamento", dat_data_pagamento),
                new SqlParameter("@int_usuario_id_fk", int_usuario_id_fk),
                new SqlParameter("@int_mes_ano_referencia_id_fk", int_mes_ano_referencia_id_fk)
                };

            conexao.execNonQuery(query, parametros);

            return conexao.ToString();

        }

        public List<Dictionary<string, object>> execRetornarDespesa(Dictionary<string, object[]> parametros)
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
            List<Dictionary<string, object>> resultado =  conexao.execQuery(query, binds);

            return resultado;
        }


        public int execAtualizarDespesa(Dictionary<string, object[]> parametros, Dictionary<string, object> novosValores)
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
