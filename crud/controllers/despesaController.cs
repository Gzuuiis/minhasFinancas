using System;
using System.Collections.Generic;
using crud.models;

namespace crud.controllers
{
    internal class despesaController
    {
        public int int_id { get; set; }
        public string str_nome { get; set; }
        public string str_tipo_despesa { get; set; }
        public int int_qtd_parcela { get; set; }
        public float flt_valor_previsto { get; set; }
        public float flt_valor_gasto { get; set; }
        public DateTime dat_data_vencimento { get; set; }
        public DateTime dat_data_pagamento { get; set; }
        public int int_id_usuario_fk { get; set; }
        public int int_id_meios_pagamento_fk { get; set; }
        public int int_id_mes_ano_referencia_fk { get; set; }


        private despesaModel despesaModel;
        public despesaController() 
        {
            despesaModel = new despesaModel();    
        }

        public List<Dictionary<string, object>> retornarDespesa(Dictionary<string, object[]> str_parametos)
        {
            List<Dictionary<string, object>> resultados = despesaModel.execRetornarDespesa(str_parametos);

            return resultados;
        }

        public void criarDespesa(string str_nome, string str_tipo_despesa, int int_qtd_parcela, float flt_valor_previsto, float flt_valor_gasto, DateTime dat_data_vencimento, DateTime dat_data_pagamento, int int_id_usuario_fk, int int_id_mes_ano_referencia_fk)
        {
            despesaModel.execCriarDespesa(str_nome, str_tipo_despesa, int_qtd_parcela, flt_valor_previsto, flt_valor_gasto, dat_data_vencimento, dat_data_pagamento, int_id_usuario_fk, int_id_mes_ano_referencia_fk);
        }

        public void atualizarDespesa(int int_id, Dictionary<string, object> novosValores)
        {

            Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            {
                { "int_id", new object[] { "=", int_id } }
            };

            despesaModel.execAtualizarDespesa(parametros, novosValores);
        }
    }
}
