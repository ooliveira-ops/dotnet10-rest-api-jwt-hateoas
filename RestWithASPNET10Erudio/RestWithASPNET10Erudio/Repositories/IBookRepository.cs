using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Repositories
{
	// Herda tudo que já existe no repositório genérico (FindALL, FindByID, Create, Update, Delete...)
	// e adiciona só o método que é específico do Book: a busca paginada.
	public interface IBookRepository : IRepository<Book>
	{
		PagedSearch<Book> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);
	}
}