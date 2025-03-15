using System;
using System.Collections.Generic;
using crud.models;

namespace crud.controllers
{
    internal class mesAnoReferenciaController
    {
        public int int_id { get; set; }
        public string int_mes { get; set; }
        public string int_ano { get; set; }

        private mesAnoReferenciaModel mesAnoReferenciaModel;
        public mesAnoReferenciaController() 
        {
            mesAnoReferenciaModel = new mesAnoReferenciaModel();    
        }

        public List<Dictionary<string, object>> retornarMesAnoReferencia(Dictionary<string, object[]> str_parametos)
        {
            List<Dictionary<string, object>> resultados = mesAnoReferenciaModel.execRetornarMesAnoReferencia(str_parametos);

            return resultados;
        }

        public void criarMesAnoReferencia(int int_mes, int int_ano)
        {
            mesAnoReferenciaModel.execCriarMesAnoReferencia(int_mes, int_ano);
        }

        public void atualizarMesAnoReferencia(int int_id, Dictionary<string, object> novosValores)
        {

            Dictionary<string, object[]> parametros = new Dictionary<string, object[]>
            {
                { "int_id", new object[] { "=", int_id } }
            };

            mesAnoReferenciaModel.execAtualizarMesAnoReferencia(parametros, novosValores);
        }
    }
}
