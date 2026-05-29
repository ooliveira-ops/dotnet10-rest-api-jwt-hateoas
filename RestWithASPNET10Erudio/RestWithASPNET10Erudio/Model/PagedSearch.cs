using System.Xml.Serialization;
using RestWithASPNET10Erudio.Hypermedia.Abstract;


namespace RestWithASPNET10Erudio.Model
{
	// "<T>" = genérico, servindo p qlqr tipo de obj... "desde que T suporte Hypermedia"
	public class PagedSearch<T> 
	{
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public string SortDirections { get; set; } = "asc";
		public int TotalResults { get; set; }
		public List<T> List { get; set; }
	}
}
