using System;
using System.Collections.Generic;
using System.Linq;
using crud.controllers;

namespace crud
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //usuarioController novoUsuario = new usuarioController();
            //novoUsuario.criarUsuario("Robert", "robertcostafernandespinto33@hotmail.com", "123456Ro!");

            //Dictionary<string, object> str_parametros = new Dictionary<string, object>
            //    {
            //        { "str_email", new object[] { "=", "robertcostafernandespinto33@hotmail.com" } }
            //    };

            //List<Dictionary<string, object>>  resultados = novoUsuario.retornarUsuarios(str_parametros);

            //novoUsuario.enviarEmailConfirmacaoCadastro(resultados[0]["str_email"].ToString(), resultados[0]["str_nome"].ToString(), Convert.ToDateTime(resultados[0]["dat_cadastro"]));

            //Imprime diretamente os valores
            //Console.WriteLine(novoUsuario);

            //usuarioController usuario = new usuarioController();
            //usuario.validarCredenciais("Vasco", "Vasco223");
            //Console.WriteLine(usuario);

            //usuarioController usuario = new usuarioController();
            //Dictionary<string, object> novosValores = new Dictionary<string, object>
            //{
            //    {"bol_email_status", "1"}
            //};

            //usuario.atualizarUsuario(7, novosValores);

            //usuarioController usuario = new usuarioController();
            //usuario.validarEmail("robertcostafernandespinto33@hotmail.com", 2997);

            //meiosPagamentoController meioPagamento = new meiosPagamentoController();
            //meioPagamento.criarMeioPagamento("Cartão de Crédito");
            //meioPagamento.criarMeioPagamento("Débito");
            //meioPagamento.criarMeioPagamento("Pix");
            //meioPagamento.criarMeioPagamento("Boleto");

            //Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            //{
            //    {"1", new object[] {"=", "1"} }
            //};

            //Console.WriteLine(meioPagamento.retornarMeiosPagamento(parametros));

            //mesAnoReferenciaController anoMes = new mesAnoReferenciaController();
            //anoMes.criarMesAnoReferencia(3, 2025);

            //receitaController receita = new receitaController();

            //DateTime dataPrevista = new DateTime(2025, 3, 5);
            //DateTime dataRecebimento = new DateTime(2025, 3, 7);
            //receita.criarReceita("Salário", 1000, dataPrevista, dataRecebimento, 7, 1);

            //sheetsController sheets = new sheetsController();

            //sheets.gerarExcel();

            //Dictionary<string, object> str_parametros = new Dictionary<string, object>
            //    {
            //        { "str_email", new object[] { "=", "robertcostafernandespinto33@hotmail.com" } }
            //    };

            Dictionary<string, object[]> str_parametros = new Dictionary<string, object[]> {
                { "int_id_usuario_fk", new object[] { "=", "7" } }, 
                {"int_id_mes_ano_referencia_fk", new object[] { "=", "2"} }
            };

            despesaController despesaController = new despesaController();
            List<Dictionary<string, object>> despesas = despesaController.retornarDespesa(str_parametros);

            sheetsController sheets = new sheetsController();

            sheets.gerarPlanilhaMensal(despesas);

            // Retorna os registros encontrados (Como dicionário)
            //foreach (var despesa in despesas)
            //{

            //    Console.WriteLine(despesa["str_nome"]);

            //    Console.WriteLine(); 
            //}


            Console.ReadLine();
        }
    }
}
