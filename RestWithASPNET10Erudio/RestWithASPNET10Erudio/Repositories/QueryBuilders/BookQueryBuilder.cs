namespace RestWithASPNET10Erudio.Repositories.QueryBuilders
{
	// Essa classe só tem uma responsabilidade: montar as strings de SQL cru
	// (query de busca + query de contagem) que serão executadas depois no repositório.
	// É basicamente o "PersonQueryBuilder", só que apontando pra tabela 'books'.
	public class BookQueryBuilder
	{
		public (
			string query,
			string countQuery,
			string sort,
			int size,
			int offset)
			BuildQueries(
			string title,
			string sortDirection,
			int pageSize,
			int page)
		{
			// Garante que a página nunca seja menor que 1 (não existe página 0 ou negativa)
			page = Math.Max(1, page);

			// Calcula quantos registros "pular" antes de começar a trazer resultados.
			// Ex: página 2, tamanho 10 -> offset = 10 (pula os 10 primeiros)
			var offset = (page - 1) * pageSize;

			// Se ninguém mandar um tamanho válido, usa no mínimo 1 (evita erro de LIMIT 0 ou negativo)
			var size = pageSize < 1 ? 1 : pageSize;

			// Se sortDirection vier vazio ou for "desc", usa "desc". Qualquer outra coisa vira "asc".
			var sort = !string.IsNullOrWhiteSpace(sortDirection)
				&& !sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
				? "asc" : "desc";

			// "WHERE 1 = 1" é um truque pra sempre poder concatenar "AND ..." depois,
			// sem se preocupar se é o primeiro filtro ou não.
			var whereClause = $"FROM books p WHERE 1 = 1 ";

			// Só filtra por título se o usuário realmente mandou um título pra buscar.
			if (!string.IsNullOrWhiteSpace(title))
				whereClause += $" AND (p.title LIKE '%{title}%') ";

			// Monta a query final: seleciona tudo, aplica o filtro, ordena por título
			// e usa LIMIT/OFFSET (sintaxe MySQL, diferente do OFFSET...FETCH do SQL Server)
			var query = $@"
			 SELECT *  {whereClause} 
			 ORDER BY p.title {sort} 
			 LIMIT {size} OFFSET {offset}";

			// Query separada só pra contar o total de registros (sem LIMIT),
			// necessária pra calcular quantas páginas existem no total.
			var countQuery = $"SELECT COUNT(*)  {whereClause}";

			return (query, countQuery, sort, size, offset);
		}
	}
}