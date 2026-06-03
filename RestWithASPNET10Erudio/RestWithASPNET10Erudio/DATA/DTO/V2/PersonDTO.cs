using System.Text.Json.Serialization;
using System.Xml.Serialization;
using RestWithASPNET10Erudio.JsonSerializers;

namespace RestWithASPNET10Erudio.Data.DTO.V2
{
	public class PersonDTO
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }

		[JsonConverter(typeof(GenderSerializer))]
		public string Gender { get; set; }

		//[JsonConverter(typeof(DateSerializer))]
		//[XmlIgnore]
		//public DateTime? BirthDay { get; set; }

		
	}
}