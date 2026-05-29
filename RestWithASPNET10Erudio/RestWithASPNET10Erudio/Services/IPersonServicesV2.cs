 using RestWithASPNET10Erudio.Data.DTO.V2;

namespace RestWithASPNET10Erudio.Services
{
	public interface IPersonServicesV2
	{
		List<PersonDTO> FindALL();
		PersonDTO FindByID(long id);
		PersonDTO Create(PersonDTO person);
		PersonDTO Update(PersonDTO person);
		void Delete(long id);
	}
}