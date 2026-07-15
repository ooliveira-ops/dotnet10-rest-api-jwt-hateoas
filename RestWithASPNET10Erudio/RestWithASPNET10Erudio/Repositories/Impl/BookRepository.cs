using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Model.Context;
using RestWithASPNET10Erudio.Repositories.QueryBuilders;

namespace RestWithASPNET10Erudio.Repositories.Impl
{
	// Herda do GenericRepository<Book> (ganha de graça o FindALL, Create, Update, Delete, etc.)
	// e implementa a interface específica IBookRepository.
	public class BookRepository(MSSQLContext context) : GenericRepository<Book>(context), IBookRepository
	{
		public PagedSearch<Book> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page)
		{
			// Usa o QueryBuilder pra montar as duas strings SQL (busca e contagem)
			var queryBuilder = new BookQueryBuilder();
			var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueries(title, sortDirection, pageSize, page);

			// "base." chama o método que já existe no GenericRepository (herdado)
			var books = base.FindWithPagedSearch(query);
			var totalResult = base.GetCount(countQuery);

			// Empacota tudo num objeto só, que vai ser usado depois pelo front-end
			// pra montar os botões de "próxima página", "página anterior", etc.
			return new PagedSearch<Book>()
			{
				CurrentPage = page,
				List = books,
				PageSize = size,
				SortDirections = sort,
				TotalResults = totalResult
			};
		}
	}
}