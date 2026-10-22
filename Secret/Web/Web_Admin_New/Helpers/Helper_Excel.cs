using OfficeOpenXml;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Helpers
{
    /// <summary>
    /// Помощник работы с Excel
    /// </summary>
    public static class Helper_Excel
    {

        /// <summary>
        /// Создание Excel с клиентами
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="clients"></param>
        public static byte[] CreateExcelFromClients(List<CClient> List_Clients)
        {

            ExcelPackage.License.SetNonCommercialPersonal(AppConfig.Excel_CommercialPersonal);

            using ExcelPackage package = new();

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

            //Заголовки
            worksheet.Cells[1, 1].Value = "Клиенты";
            worksheet.Cells[2, 1].Value = "Код";
            worksheet.Cells[2, 2].Value = "Имя";
            worksheet.Cells[2, 3].Value = "Телефон";
            worksheet.Cells[2, 4].Value = "Дата рождения";


            worksheet.Cells[1, 1, 1, 4].Merge = true;
            worksheet.Cells[1, 1, 1, 4].Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            //Заполнение
            int row = 3;
            foreach (CClient client in List_Clients)
            {
                worksheet.Cells[row, 1].Value = client.ID;
                worksheet.Cells[row, 2].Value = client.Name;
                worksheet.Cells[row, 3].Value = client.Phone;
                if (client.Date_Birth.HasValue)
                {
                    worksheet.Cells[row, 4].Value = client.Date_Birth.Value.ToShortDateString();
                }
                else
                {
                    worksheet.Cells[row, 4].Value = null;
                }

                row++;
            }

            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();

        }
    }
}
