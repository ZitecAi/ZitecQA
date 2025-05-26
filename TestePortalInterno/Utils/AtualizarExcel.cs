using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;

namespace TestePortalInterno.Utils
{
    public class AtualizarExcel
    {
        public static bool AtualizarExcelComIds(string caminhoExcel, List<int> ids)
        {
            if (!File.Exists(caminhoExcel))
            {
                Console.WriteLine("Arquivo Excel não encontrado.");
                return false;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                //using (var package = new ExcelPackage(new FileInfo(caminhoExcel)))
                //{
                //    var worksheet = package.Workbook.Worksheets[0];


                //    int startRow = 2;

                //    for (int i = 0; i < ids.Count; i++)
                //    {

                //        if (startRow + i <= 5)
                //        {
                //            worksheet.Cells[startRow + i, 1].Value = ids[i];
                //        }
                //        else
                //        {
                //            break;
                //        }
                //        //package.Save();
                //    }

                //}
                //using (var excelWorkbook = new XLWorkbook(caminhoExcel))
                //{
                //    var rows = excelWorkbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1);
                //    int i = 0;
                //    foreach (var row in rows)
                //    {
                //        row.Cell(1).Value = ids[i];
                //    }

                //    excelWorkbook.Save();
                //}
                Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
                workbook.LoadFromFile(caminhoExcel);
                Spire.Xls.Worksheet sheet = workbook.Worksheets[0];
                try
                {
                    for (int i = 2; i < ids.Count + 2; i++)
                    {
                        sheet.Range[$"A{i}"].Text = ids[i - 2].ToString();
                    }
                }
                catch (System.Exception e)
                {
                }
                workbook.SaveToFile(caminhoExcel);
                Console.WriteLine("IDs inseridos com sucesso no Excel.");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar Excel: {e.Message}");
                return false;
            }
        }
    }
}
