using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using crud.Config;

namespace crud.models
{
    internal class usuarioModel
    {
        private string str_nome_tabela;

        public int int_id { get; set; }
        public string str_nome { get; set; }
        public string str_email { get; set; }
        public string str_hash { get; set; }
        public bool bol_email_status { get; set; }
        public int int_codigo_email { get; set; }
        public DateTime dat_cadastro { get; set; }

        public usuarioModel()
        {
            str_nome_tabela = "usuarios";
        }


        public string execCriarUsuario(string str_nome, string str_email, string str_hash)
        {

            var config = new BDConfig();
            var conexao = new BDConexao(config);

            Random rnd = new Random();
            int_codigo_email = rnd.Next(1111, 9999);

            // Definindo a query
            string query = "INSERT INTO " + str_nome_tabela + " (str_nome, str_email, str_hash, bol_email_status, int_codigo_email, dat_cadastro) " +
                           "VALUES (@str_nome, @str_email, @str_hash, 0, @int_codigo_email, GETDATE())";

            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@str_nome", str_nome),
                new SqlParameter("@str_email", str_email),
                new SqlParameter("@str_hash", str_hash),
                new SqlParameter("@int_codigo_email", int_codigo_email)
                };

            conexao.execNonQuery(query, parametros);

            return conexao.ToString();

        }

        public List<Dictionary<string, object>> execRetornarUsuario(Dictionary<string, object[]> parametros)
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


        public int execAtualizarUsuario(Dictionary<string, object[]> parametros, Dictionary<string, object> novosValores)
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
