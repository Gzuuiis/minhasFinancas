using System;
using System.Collections.Generic;
using crud.models;

namespace crud.controllers
{
    internal class receitaController
    {
        public int int_id { get; set; }
        public string str_nome { get; set; }
        public float flt_valor_recebido { get; set; }
        public DateTime dat_data_prevista { get; set; }
        public DateTime dat_data_recebimento { get; set; }
        public int int_id_usuario_fk { get; set; }
        public int int_id_mes_ano_referencia_fk { get; set; }

        private receitaModel receitaModel;
        public receitaController() 
        {
            receitaModel = new receitaModel();    
        }

        public List<Dictionary<string, object>> retornarReceita(Dictionary<string, object[]> str_parametos)
        {
            List<Dictionary<string, object>> resultados = receitaModel.execRetornarReceita(str_parametos);

            return resultados;
        }

        public void criarReceita(string str_nome, float flt_valor_recebido, DateTime dat_data_prevista, DateTime dat_data_recebimento, int int_id_usuario_fk, int int_id_mes_ano_referencia_fk)
        {
            receitaModel.execCriarReceita(str_nome, flt_valor_recebido, dat_data_prevista, dat_data_recebimento, int_id_usuario_fk, int_id_mes_ano_referencia_fk);
        }

        public void atualizarReceita(int int_id, Dictionary<string, object> novosValores)
        {

            Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            {
                { "int_id", new object[] { "=", int_id } }
            };

            receitaModel.execAtualizarReceita(parametros, novosValores);
        }
    }
}
