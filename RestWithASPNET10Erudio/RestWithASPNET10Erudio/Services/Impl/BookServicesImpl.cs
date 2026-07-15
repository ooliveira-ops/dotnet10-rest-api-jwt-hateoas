using Mapster;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Repositories;
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Services.Impl
{
	public class BookServicesImpl : IBookServices
	{
		// Antes era "IRepository<Book>" (genérico). Trocado pra "IBookRepository"
		// porque só ele conhece o método FindWithPagedSearch.
		// Como IBookRepository herda de IRepository<Book>, nada do resto quebra.
		private IBookRepository _repository;

		public BookServicesImpl(IBookRepository repository)
		{
			_repository = repository;
		}

		public List<BookDTO> FindAll()
		{
			return _repository.FindALL().Adapt<List<BookDTO>>();
		}

		public BookDTO FindByID(long id)
		{
			return _repository.FindByID(id).Adapt<BookDTO>();
		}

		public BookDTO Create(BookDTO book)
		{
			var entity = book.Adapt<Book>();
			entity = _repository.Create(entity);
			return entity.Adapt<BookDTO>();
		}

		public BookDTO Update(BookDTO book)
		{
			var entity = book.Adapt<Book>();
			entity = _repository.Update(entity);
			return entity.Adapt<BookDTO>();
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}

		// Novo método: chama o repositório, que devolve PagedSearch<Book> (entidade),
		// e converte a lista interna pra PagedSearch<BookDTO> (o que o Controller expõe pra fora)
		public PagedSearch<BookDTO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page)
		{
			var result = _repository.FindWithPagedSearch(title, sortDirection, pageSize, page);

			return new PagedSearch<BookDTO>
			{
				CurrentPage = result.CurrentPage,
				List = result.List.Adapt<List<BookDTO>>(), // converte List<Book> -> List<BookDTO>
				PageSize = result.PageSize,
				SortDirections = result.SortDirections,
				TotalResults = result.TotalResults
			};
		}
	}
}