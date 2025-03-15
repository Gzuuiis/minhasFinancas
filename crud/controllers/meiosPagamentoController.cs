using System;
using System.Collections.Generic;
using crud.models;

namespace crud.controllers
{
    internal class meiosPagamentoController
    {
        public int int_id { get; set; }
        public string str_nome { get; set; }

        private meiosPagamentoModel meiosPagamentoModel;
        public meiosPagamentoController() 
        {
            meiosPagamentoModel = new meiosPagamentoModel();    
        }

        public List<Dictionary<string, object>> retornarMeiosPagamento(Dictionary<string, object[]> str_parametos)
        {
            List<Dictionary<string, object>> resultados = meiosPagamentoModel.execRetornarMeioPagamento(str_parametos);

            return resultados;
        }

        public void criarMeioPagamento(string str_nome)
        {
            meiosPagamentoModel.execCriarMeioPagamento(str_nome);
        }

        public void atualizarMeioPagamento(int int_id, Dictionary<string, object> novosValores)
        {

            Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            {
                { "int_id", new object[] { "=", int_id } }
            };

            meiosPagamentoModel.execAtualizarMeioPagamento(parametros, novosValores);
        }
    }
}
