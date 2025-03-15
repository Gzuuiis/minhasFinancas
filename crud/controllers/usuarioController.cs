using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using crud.models;
using crud.services;

namespace crud.controllers
{
    internal class usuarioController
    {
        public int int_id { get; set; }
        public string str_nome { get; set; }
        public string str_email { get; set; }
        public string str_hash { get; set; }

        private usuarioModel usuarioModel;

        public usuarioController()
        {
            usuarioModel = new usuarioModel();
        }

        public List<Dictionary<string, object>> retornarUsuarios(Dictionary<string, object[]> str_parametos)
        {
            List<Dictionary<string, object>> resultados = usuarioModel.execRetornarUsuario(str_parametos);

            return resultados;
        }

        public void criarUsuario(string str_nome, string str_email, string str_hash)
        {
            hashingService hash = new hashingService();
            str_hash = hash.setHashPassword(str_hash);

           usuarioModel.execCriarUsuario(str_nome, str_email, str_hash);
        }

        public void atualizarUsuario(int int_id, Dictionary<string, object> novosValores)
        {

            Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            {
                { "int_id", new object[] { "=", int_id } }
            };

            usuarioModel.execAtualizarUsuario(parametros, novosValores);
        }

        public void enviarEmailConfirmacaoCadastro(string emailUsuario, string nomeUsuario, DateTime dataCadastro, int int_codigo_email)
        {
            emailService email = new emailService();
            email.enviarEmailConfirmacaoCadastro(emailUsuario, nomeUsuario, dataCadastro, int_codigo_email);
        }

        public void validarCredenciais(string str_email, string str_senha)
        {
            var parametros = new Dictionary<string, object[]>
            {
                { "str_email", new object[] { "=", str_email } }
            };

            var usuario = usuarioModel.execRetornarUsuario(parametros);

            // Verifica se o usuário foi encontrado
            if (usuario.Count == 0)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            var senhaHash = usuario[0]["str_hash"].ToString();

            hashingService hash = new hashingService();

            bool senhaValida = hash.verificarHashPassword(str_senha, senhaHash);

            if (senhaValida)
            {
                Console.WriteLine("Credenciais válidas.");
            }
            else
            {
                Console.WriteLine("Senha incorreta.");
            }
        }

        public int validarEmail(string str_email, int int_codigo_email)
        {

            Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            {
                {"str_email", new object[]{"=", str_email} },
                { "int_codigo_email", new object[]{"=", int_codigo_email } }
            };

            List<Dictionary<string, object>> usuario = usuarioModel.execRetornarUsuario(parametros).ToList();

            Console.WriteLine($"Linhas Encontradas {usuario.Count()} \n");

            if(usuario.Count() > 0)
            {
                Dictionary<string, object> novoValorStatusEmail = new Dictionary<string, object>
                {
                    {"bol_email_status", "1"}
                };

                int resultado = usuarioModel.execAtualizarUsuario(parametros, novoValorStatusEmail);
                Console.WriteLine($"Linhas Atualizadas {resultado} \n");

                return resultado;
            }
            else
            {
                Console.WriteLine("Email ou Código inválido!");
                return 0;
            }
            
        }

    }
}
