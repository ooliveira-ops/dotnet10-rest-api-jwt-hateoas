using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.FIles.Importers.Contract;

namespace RestWithASPNET10Erudio.FIles.Importers.Factory
{
	internal class CsvImporter : IFileImporter
	{
		//ClassMap para traduzir snake_case → PascalCase
		private sealed class PersonDTOMap : ClassMap<PersonDTO>
		{
			public PersonDTOMap()
			{
				Map(m => m.FirstName).Name("first_name");
				Map(m => m.LastName).Name("last_name");
				Map(m => m.Address).Name("address");
				Map(m => m.Gender).Name("gender");
				Map(m => m.Enabled).Ignore();  // não vem do CSV
				Map(m => m.Id).Ignore();        // não vem do CSV
			}
		}

		public async Task<List<PersonDTO>> ImportFileAsync(Stream fileStream)
		{
			using var reader = new StreamReader(fileStream);
			using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				TrimOptions = TrimOptions.Trim,
				IgnoreBlankLines = true,
				HeaderValidated = null,
				MissingFieldFound = null,
			});

			csv.Context.RegisterClassMap<PersonDTOMap>(); 

			var people = new List<PersonDTO>();

			await foreach (var record in csv.GetRecordsAsync<PersonDTO>())
			{
				record.Enabled = true;
				people.Add(record);
			}

			return people;
		}
	}
}