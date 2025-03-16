using System;
using System.Collections.Generic;
using crud.services;

namespace crud.controllers
{
    internal class sheetsController
    {
        private sheetsService sheets;

        public sheetsController()
        {
           
        }

        public void criarPlanilha(string nomePlanilha, List<Dictionary<string, object>> despesas, List<Dictionary<string, object>> receitas)
        {
            sheetsService sheets = new sheetsService();
            sheets.gerarPlanilha(nomePlanilha, despesas, receitas);
        }

        public void gerarExcel()
        {
            Console.WriteLine("\n Iniciando Chamada de Geração do Sheets - Controller \n");
            sheetsService sheets = new sheetsService();
            sheets.gerarExcelTeste();
            Console.WriteLine("\n Terminada a Chamada de Geração do Sheets - Controller \n");
        }

        /* public void gerarPlanilhaMensal(List<Dictionary<string, object>> despesas)
        {
            sheetsService sheets = new sheetsService();
            sheets.gerarCaixaDespesas(despesas);

            Console.WriteLine("\n Planilha Gerada com Sucesso");
        }*/



    }
}
