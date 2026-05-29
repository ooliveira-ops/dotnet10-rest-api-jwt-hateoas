using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.FIles.Exporters.Contract;
using RestWithASPNET10Erudio.FIles.Exporters.Factory;

namespace RestWithASPNET10Erudio.FIles.Exporters.Impl
{
	internal class XlsxExporter : IFileExporter
	{
		public FileContentResult ExportFile(List<PersonDTO> people)
		{
			using var workbook = new XLWorkbook();						//cria um 'workbook'; RESUMO: como se abrisse o Excel, criasse uma planilha e salvasse 
			var worksheet = workbook.Worksheets.Add("People");			// "worksheet" = uma aba na planilha, chamada de 'people'

			// Adding Headers
			worksheet.Cell(1, 1).Value = "Id";						//linha 1, coluna 1 de valor "Id"
			worksheet.Cell(1, 2).Value = "First Name";
			worksheet.Cell(1, 3).Value = "Last Name";               //linha 1, coluna 2 de valor "First Name".... até a coluna 6
			worksheet.Cell(1, 4).Value = "Address";
			worksheet.Cell(1, 5).Value = "Gender";
			worksheet.Cell(1, 6).Value = "Enabled";
			
			var headerRange = worksheet.Range(1, 1, 1, 6);				//vai começar na 1ªlinha, 1ª célula e na 1ª coluna e o "6" é ate onde vai
			headerRange.Style.Font.Bold = true;							//fonte negrito
			headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;    //alinhar ao centro a célula

				
			int row = 2;						//2ª linha da planilha
			foreach (var person in people)
			{
				worksheet.Cell(row, 1).Value = person.Id;
				worksheet.Cell(row, 2).Value = person.FirstName;
				worksheet.Cell(row, 3).Value = person.LastName;
				worksheet.Cell(row, 4).Value = person.Address;
				worksheet.Cell(row, 5).Value = person.Gender;
				worksheet.Cell(row, 6).Value = person.Enabled == true ? "Yes" : "No";   // "se o 'Enabled' for igual a 'true', ele vai retornar 'Yes', se nao, ele vai retornar 'No'

				row++;
			}

			worksheet.Columns().AdjustToContents();						//ajustar o tamanho das colunas

			using var stream = new MemoryStream();
			workbook.SaveAs(stream);								//salvar a planilha
			
			var fileBytes = stream.ToArray();						//converter para um array de bytes

			return new FileContentResult(fileBytes, 
				MediaTypes.ApplicationXlsx) { FileDownloadName = $"people_exported_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx" };
		}
	}
}