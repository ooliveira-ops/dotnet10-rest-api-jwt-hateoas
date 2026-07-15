using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Services
{
	public interface IBookServices
	{

		BookDTO Create(BookDTO book);
		BookDTO FindByID(long id);
		List<BookDTO> FindAll();
		BookDTO Update(BookDTO book);
		void Delete(long id);

		// Novo método: busca paginada por título, com ordenação e tamanho de página configuráveis
		PagedSearch<BookDTO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);
	}
}