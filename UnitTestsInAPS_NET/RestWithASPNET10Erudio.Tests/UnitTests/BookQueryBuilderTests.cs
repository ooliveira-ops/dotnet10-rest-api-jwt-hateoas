using FluentAssertions;
using RestWithASPNET10Erudio.Repositories.QueryBuilders;

namespace RestWithASPNET10Erudio.Tests.UnitTests
{
	public class BookQueryBuilderTests
	{
		private readonly BookQueryBuilder _queryBuilder;

		public BookQueryBuilderTests()
		{
			_queryBuilder = new BookQueryBuilder();
		}

		[Fact]
		public void BuildQueries_ShouldReturnCorrectQueries()
		{
			//arrange
			var title = "Docker";
			var sortDirection = "asc";
			var pageSize = 10;
			var page = 2;

			//act
			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				title, sortDirection, pageSize, page);

			//assert
			query.Should().Contain("SELECT *  FROM books p WHERE 1 = 1  AND (p.title LIKE '%Docker%')");
			query.Should().Contain("ORDER BY p.title asc");
			query.Should().Contain("LIMIT 10 OFFSET 10");

			countQuery.Should().Be("SELECT COUNT(*)  FROM books p WHERE 1 = 1  AND (p.title LIKE '%Docker%') ");
			sort.Should().Be("asc");
			size.Should().Be(10);
			offset.Should().Be(10);
		}

		[Fact]                      //handle = "lidar"
		public void BuildQueries_ShouldHandleInvalidPageAndPageSize()
		{
			//arrange
			var title = "";
			var sortDirection = "desc";
			var pageSize = 0;
			var page = -1;

			//act
			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				title, sortDirection, pageSize, page);

			//assert
			query.Should().Contain("SELECT *  FROM books p WHERE 1 = 1 ");
			query.Should().Contain("ORDER BY p.title desc");
			query.Should().Contain("LIMIT 1 OFFSET 0");

			countQuery.Should().Be("SELECT COUNT(*)  FROM books p WHERE 1 = 1 ");
			sort.Should().Be("desc");
			size.Should().Be(1);
			offset.Should().Be(0);
		} 

		[Fact]                    //handle = "lidar"
		public void BuildQueries_ShouldHandleNullOrWhitespaceTitle()
		{
			//arrange
			string title = null;
			var sortDirection = "asc";
			var pageSize = 5;
			var page = 1;

			//act
			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				title, sortDirection, pageSize, page);

			//assert
			query.Should().Contain("SELECT *  FROM books p WHERE 1 = 1 ");
			query.Should().Contain("ORDER BY p.title asc");
			query.Should().Contain("LIMIT 5 OFFSET 0");
			query.Should().NotContain("AND (p.title LIKE");

			countQuery.Should().Be("SELECT COUNT(*)  FROM books p WHERE 1 = 1 ");
			sort.Should().Be("asc");
			size.Should().Be(5);
			offset.Should().Be(0);
		}

		[Fact]
		public void BuildQueries_ShouldDefaultToAscForInvalidSortDirection()
		{
			//arrange
			var title = "Kubernetes";
			var sortDirection = "invalid";
			var pageSize = 10;
			var page = 1;

			//act
			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				title, sortDirection, pageSize, page);

			//assert
			query.Should().Contain("SELECT *  FROM books p WHERE 1 = 1  AND (p.title LIKE '%Kubernetes%')");
			query.Should().Contain("ORDER BY p.title asc");
			query.Should().Contain("LIMIT 10 OFFSET 0");

			countQuery.Should().Be("SELECT COUNT(*)  FROM books p WHERE 1 = 1  AND (p.title LIKE '%Kubernetes%') ");
			sort.Should().Be("asc");
			size.Should().Be(10);
			offset.Should().Be(0);
		}
	}
}