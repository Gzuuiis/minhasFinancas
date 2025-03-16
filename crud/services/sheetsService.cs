using System;
using System.Collections.Generic;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Drawing; 
using System.IO;
using crud.models;
using BitMiracle.LibTiff.Classic;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

namespace crud.services
{
    internal class sheetsService
    {
        private despesaModel despesaModel;
        private string rootPath;

        public sheetsService()
        {
            despesaModel = new despesaModel();
            rootPath = "C:\\Users\\CRVG Robert\\source\\repos\\crud\\crud";
        }

        public void gerarExcelTeste()
        {
            Console.WriteLine("\n Iniciando Chamada de Geração do Sheets - Services \n");

            // Criação de um novo arquivo Excel
            IWorkbook xlsWorkbook = new XSSFWorkbook();
            ISheet xlsSheet = xlsWorkbook.CreateSheet("new_sheet");

            // Configuração de células
            IRow row1 = xlsSheet.CreateRow(0);  // Linha 1
            row1.CreateCell(0).SetCellValue("Hello World");

            IRow row2 = xlsSheet.CreateRow(1);  // Linha 2
            row2.CreateCell(0).SetCellValue("Hello World");

            // Estilo para a célula
            ICellStyle style = xlsWorkbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Double;
            style.BottomBorderColor = IndexedColors.Orange.Index;
            row2.Cells[0].CellStyle = style;

            // Salvando o arquivo Excel
            using (var fs = new FileStream(rootPath + "\\NewExcelFile.xlsx", FileMode.Create, FileAccess.Write))
            {
                xlsWorkbook.Write(fs);
            }

            Console.WriteLine("\n Terminada a Chamada de Geração do Sheets - Services \n");
        }
        public void gerarPlanilha(string nomePlanilha, List<Dictionary<string, object>> despesas, List<Dictionary<string, object>> receitas)
        {
            bool result = criarPlanilha(nomePlanilha);

            if (result)
            {
                gerarCaixaDespesas(nomePlanilha, despesas);
                gerarCaixaReceitas(nomePlanilha, receitas);
            }
        }

        public bool criarPlanilha(string nomePlanilha)
        {
            Console.WriteLine("\n Iniciando Chamada de criarPlanilha - Services \n");

            string nomeCExt = nomePlanilha + ".xlsx";

            // Criação de um novo arquivo Excel
            IWorkbook xlsWorkbook = new XSSFWorkbook();
            ISheet xlsSheet = xlsWorkbook.CreateSheet(nomePlanilha);

            XSSFFont myFont = (XSSFFont)xlsWorkbook.CreateFont(); 
            myFont.FontName = "Calibri"; 
            myFont.FontHeightInPoints = 12; 

            // Caminho do arquivo
            string caminhoArquivo = Path.Combine(rootPath, nomeCExt);

            using (var fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                xlsWorkbook.Write(fs);
            }

            Console.WriteLine("\n Resultado criarPlanilha: " + caminhoArquivo);

            return File.Exists(caminhoArquivo);
        }


        public void gerarCaixaDespesas(string nomePlanilha, List<Dictionary<string, object>> despesas)
        {
            string nomeCExt = nomePlanilha + ".xlsx";
            string caminhoArquivo = Path.Combine(rootPath, nomeCExt);

            using (var fs = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                IWorkbook xlsWorkbook = new XSSFWorkbook(fs);
                ISheet xlsSheet = xlsWorkbook.GetSheetAt(0);

                XSSFCellStyle cellTop = (XSSFCellStyle)xlsWorkbook.CreateCellStyle();

                cellTop.FillForegroundColor = HSSFColor.Black.Index;
                cellTop.FillPattern = FillPattern.SolidForeground;
                cellTop.Alignment = HorizontalAlignment.Center;
                cellTop.VerticalAlignment = VerticalAlignment.Center;

                XSSFFont font = (XSSFFont)xlsWorkbook.CreateFont();
                font.Color = HSSFColor.White.Index;

                cellTop.SetFont(font);

                // Cabeçalhos
                IRow topRow = xlsSheet.GetRow(0) ?? xlsSheet.CreateRow(0);

                topRow.Height = 600;

                ICell cellTop0 = topRow.CreateCell(0);
                cellTop0.SetCellValue("DESPESAS (SAÍDAS)");
                cellTop0.CellStyle = cellTop;
                xlsSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7)); 


                ICell cellTop1 = topRow.CreateCell(1);
                cellTop1.SetCellValue("");
                cellTop1.CellStyle = cellTop;

                ICell cellTop2 = topRow.CreateCell(2);
                cellTop2.SetCellValue("DESPESAS (SAÍDAS)");
                cellTop2.CellStyle = cellTop;

                ICell cellTop3 = topRow.CreateCell(3);
                cellTop3.SetCellValue("");
                cellTop3.CellStyle = cellTop;

                ICell cellTop4 = topRow.CreateCell(4);
                cellTop4.SetCellValue("");
                cellTop4.CellStyle = cellTop;

                ICell cellTop5 = topRow.CreateCell(5);
                cellTop5.SetCellValue("");
                cellTop5.CellStyle = cellTop;

                ICell cellTop6 = topRow.CreateCell(6);
                cellTop6.SetCellValue("");
                cellTop6.CellStyle = cellTop;

                ICell cellTop7 = topRow.CreateCell(7);
                cellTop7.SetCellValue("");
                cellTop7.CellStyle = cellTop;


                XSSFCellStyle cellHeader = (XSSFCellStyle)xlsWorkbook.CreateCellStyle();

                cellHeader.FillForegroundColor = HSSFColor.DarkYellow.Index;
                cellHeader.FillPattern = FillPattern.SolidForeground;

                IRow headerRow = xlsSheet.CreateRow(1);

                headerRow.Height = 400;

                ICell cell0 = headerRow.CreateCell(0);
                cell0.SetCellValue("Descrição");
                cell0.CellStyle = cellHeader;

                ICell cell1 = headerRow.CreateCell(1);
                cell1.SetCellValue("Tipo de Despesa");
                cell1.CellStyle = cellHeader;

                ICell cell2 = headerRow.CreateCell(2);
                cell2.SetCellValue("Meio de Pagamento");
                cell2.CellStyle = cellHeader;

                ICell cell3 = headerRow.CreateCell(3);
                cell3.SetCellValue("Forma de Pagamento");
                cell3.CellStyle = cellHeader;

                ICell cell4 = headerRow.CreateCell(4);
                cell4.SetCellValue("Valor Previsto");
                cell4.CellStyle = cellHeader;

                ICell cell5 = headerRow.CreateCell(5);
                cell5.SetCellValue("Valor Gasto");
                cell5.CellStyle = cellHeader;

                ICell cell6 = headerRow.CreateCell(6);
                cell6.SetCellValue("Data de Vencimento");
                cell6.CellStyle = cellHeader;

                ICell cell7 = headerRow.CreateCell(7);
                cell7.SetCellValue("Data do Pagamento");
                cell7.CellStyle = cellHeader;

                // Preenchendo as despesas
                int rowIndex = 2;
                foreach (var despesa in despesas)
                {
                    IRow row = xlsSheet.CreateRow(rowIndex);

                    row.Height = 350;

                    row.CreateCell(0).SetCellValue(despesa["str_nome"].ToString());
                    row.CreateCell(1).SetCellValue(despesa["str_tipo_despesa"].ToString());
                    row.CreateCell(2).SetCellValue("Ainda não retornado");
                    row.CreateCell(3).SetCellValue("Ainda não retornado");
                    row.CreateCell(4).SetCellValue(Convert.ToDouble(despesa["flt_valor_previsto"]));
                    row.CreateCell(5).SetCellValue(Convert.ToDouble(despesa["flt_valor_gasto"]));
                    row.CreateCell(6).SetCellValue(Convert.ToDateTime(despesa["dat_data_vencimento"]).ToString("dd/MM/yyyy"));
                    row.CreateCell(7).SetCellValue(Convert.ToDateTime(despesa["dat_data_pagamento"]).ToString("dd/MM/yyyy"));

                    rowIndex++;
                }


                for (int i = 0; i <= 7; i++)
                {
                    xlsSheet.AutoSizeColumn(i);
                }

                // Salvando o arquivo Excel
                using (var fsOut = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xlsWorkbook.Write(fsOut);
                }

                xlsWorkbook.Close();
            }

            Console.WriteLine("\n Terminada a Chamada de Geração do Sheets - Services \n");
        }

        public void gerarCaixaReceitas(string nomePlanilha, List<Dictionary<string, object>> receitas)
        {
            string nomeCExt = nomePlanilha + ".xlsx";
            string caminhoArquivo = Path.Combine(rootPath, nomeCExt);

            using (var fs = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                IWorkbook xlsWorkbook = new XSSFWorkbook(fs);
                ISheet xlsSheet = xlsWorkbook.GetSheetAt(0);

                XSSFCellStyle cellTop = (XSSFCellStyle)xlsWorkbook.CreateCellStyle();
                cellTop.FillForegroundColor = HSSFColor.Black.Index;
                cellTop.FillPattern = FillPattern.SolidForeground;
                cellTop.Alignment = HorizontalAlignment.Center;
                cellTop.VerticalAlignment = VerticalAlignment.Center;

                XSSFFont font = (XSSFFont)xlsWorkbook.CreateFont();
                font.Color = HSSFColor.White.Index;
                cellTop.SetFont(font);

                // Cabeçalhos para Receitas
                IRow topRow = xlsSheet.GetRow(0) ?? xlsSheet.CreateRow(0);
                topRow.Height = 600; // Ajuste da altura da linha

                // Definindo as células e aplicando o estilo
                ICell cellTop9 = topRow.CreateCell(9);
                xlsSheet.AddMergedRegion(new CellRangeAddress(0, 0, 9, 13));
                cellTop9.SetCellValue("RECEITAS (ENTRADAS)");
                cellTop9.CellStyle = cellTop;

                ICell cellTop10 = topRow.CreateCell(10);
                cellTop10.SetCellValue("");
                cellTop10.CellStyle = cellTop;

                ICell cellTop11 = topRow.CreateCell(11);
                cellTop11.SetCellValue("");
                cellTop11.CellStyle = cellTop;

                ICell cellTop12 = topRow.CreateCell(12);
                cellTop12.SetCellValue("");
                cellTop12.CellStyle = cellTop;

                ICell cellTop13 = topRow.CreateCell(13);
                cellTop13.SetCellValue("");
                cellTop13.CellStyle = cellTop;


                XSSFCellStyle cellHeader = (XSSFCellStyle)xlsWorkbook.CreateCellStyle();

                cellHeader.FillForegroundColor = HSSFColor.DarkYellow.Index;
                cellHeader.FillPattern = FillPattern.SolidForeground;


                // Cabeçalhos
                IRow headerRow = xlsSheet.GetRow(1);

                headerRow.Height = 400;

                ICell cell9 = headerRow.CreateCell(9);
                cell9.SetCellValue("Descrição");
                cell9.CellStyle = cellHeader;

                ICell cell10 = headerRow.CreateCell(10);
                cell10.SetCellValue("Tipo de Receita");
                cell10.CellStyle = cellHeader;

                ICell cell11 = headerRow.CreateCell(11);
                cell11.SetCellValue("Valor Recebido");
                cell11.CellStyle = cellHeader;

                ICell cell12 = headerRow.CreateCell(12);
                cell12.SetCellValue("Data Prevista de Recebimento");
                cell12.CellStyle = cellHeader;

                ICell cell13 = headerRow.CreateCell(13);
                cell13.SetCellValue("Data Efetiva de Recebimento");
                cell13.CellStyle = cellHeader;


                // Preenchendo as receitas
                int rowIndex = 2;
                foreach (var receita in receitas)
                {
                    IRow row = xlsSheet.GetRow(rowIndex) ?? xlsSheet.CreateRow(rowIndex);

                    row.Height = 350;

                    row.CreateCell(9).SetCellValue(receita["str_nome"].ToString());
                    row.CreateCell(10).SetCellValue("Ainda não retornado");
                    row.CreateCell(11).SetCellValue(Convert.ToDouble(receita["flt_valor_recebido"]));
                    row.CreateCell(12).SetCellValue(Convert.ToDateTime(receita["dat_data_prevista"]).ToString("dd/MM/yyyy"));
                    row.CreateCell(13).SetCellValue(Convert.ToDateTime(receita["dat_data_recebimento"]).ToString("dd/MM/yyyy"));

                    rowIndex++;
                }

                for (int i = 9; i <= 13; i++) 
                {
                    xlsSheet.AutoSizeColumn(i); 
                }

                // Salvando o arquivo Excel
                using (var fsOut = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xlsWorkbook.Write(fsOut);
                }

                xlsWorkbook.Close();
            }

            Console.WriteLine("\n Terminada a Chamada de Geração do Sheets - Services \n");
        }

    }
}
