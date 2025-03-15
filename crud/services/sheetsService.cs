using System;
using System.Configuration;
using IronXL;
using crud.models;
using System.Collections.Generic;

namespace crud.services
{
    internal class sheetsService
    {

        private despesaModel despesaModel;

        public sheetsService()
        {
            despesaModel = new despesaModel();
        }

        public void gerarExcelTeste()
        {

            Console.WriteLine("\n Iniciando Chamada de Geração do Sheets - Services \n");

            WorkBook xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsWorkbook.Metadata.Author = "IronXL";
            WorkSheet xlsSheet = xlsWorkbook.CreateWorkSheet("new_sheet");
            xlsSheet["A1"].Value = "Hello World";
            xlsSheet["A2"].Style.BottomBorder.SetColor("#ff6600");
            xlsSheet["A2"].Style.BottomBorder.Type = IronXL.Styles.BorderType.Double;
            //xlsWorkbook.SaveAs("NewExcelFile.xls"); //Save the excel file
            xlsWorkbook.SaveAs(@"C:\Users\Teste\source\repos\crud\crud\NewExcelFile.xlsx");

            Console.WriteLine("\n Terminada a Chamada de Geração do Sheets - Services \n");
        }

        public void gerarPlanila()
        {

        }

        public void gerarCaixaDespesas(List<Dictionary<string, object>> despesas)
        {

            Console.WriteLine("\n Iniciando Chamada de Geração do Sheets - Services \n");

            WorkBook xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
            xlsWorkbook.Metadata.Author = "MinhasFinancas";

            WorkSheet xlsSheet = xlsWorkbook.CreateWorkSheet("planilha_mensal.xlsx");
            xlsSheet["A1"].Value = "GASTOS";
            xlsSheet["A2"].Style.BottomBorder.SetColor("#ff6600");

            xlsSheet["A2"].Value = "Descrição";
            xlsSheet["B2"].Value = "Tipo de Despesa";
            xlsSheet["C2"].Value = "Meio de Pagamento";
            xlsSheet["D2"].Value = "Forma de Pagamento";
            xlsSheet["E2"].Value = "Valor Previsto";
            xlsSheet["F2"].Value = "Valor Gasto";
            xlsSheet["G2"].Value = "Data de Vencimento";
            xlsSheet["H2"].Value = "Data do Pagamento";

            int coluna = 3;
            foreach(var despesa in despesas)
            {
                xlsSheet["A" + coluna].Value = despesa["str_nome"];
                xlsSheet["B" + coluna].Value = despesa["str_tipo_despesa"];
                xlsSheet["C" + coluna].Value = "Ainda não retornado";
                xlsSheet["D" + coluna].Value = "Ainda não retornado";
                xlsSheet["E" + coluna].Value = despesa["flt_valor_previsto"];
                xlsSheet["F" + coluna].Value = despesa["flt_valor_gasto"];
                xlsSheet["G" + coluna].Value = despesa["dat_data_vencimento"];
                xlsSheet["H" + coluna].Value = despesa["dat_data_pagamento"];

                coluna++;
            }

            xlsWorkbook.SaveAs(@"C:\Users\Teste\source\repos\crud\crud\planilha_mensal.xlsx");

            Console.WriteLine("\n Terminada a Chamada de Geração do Sheets - Services \n");
        }

    }
}
