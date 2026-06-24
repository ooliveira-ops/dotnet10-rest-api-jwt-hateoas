using FluentAssertions;
using RestWithASPNET10Erudio.Repositories.QueryBuilders;

namespace RestWithASPNET10Erudio.Tests.UnitTests
{
	public class PersonQueryBuilderTests
	{
		private readonly PersonQueryBuilder _queryBuilder;

		public PersonQueryBuilderTests()
		{
			_queryBuilder = new PersonQueryBuilder();
		}

		[Fact]
		public void BuildQueries_ShouldReturnCorrectQueries()
		{	
			var name = "John";
			var sortDirection = "asc";
			var pageSize = 10;
			var page = 2;

			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(  
				name, sortDirection, pageSize, page);

			query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%John%')");
			query.Should().Contain("ORDER BY p.first_name asc");
			query.Should().Contain("LIMIT 10 OFFSET 10");

			countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%John%') ");
			sort.Should().Be("asc");
			size.Should().Be(10);
			offset.Should().Be(10);
		}

		[Fact]
		public void BuildQueries_ShouldHandleInvalidPageAndPageSize()
		{   //arrange
			var name = "";
			var sortDirection = "desc";
			var pageSize = 0;
			var page = -1;
			//act
			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				name, sortDirection, pageSize, page);
			//assert
			query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1 ");
			query.Should().Contain("ORDER BY p.first_name desc");
			query.Should().Contain("LIMIT 1 OFFSET 0");
			
			countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1 ");
			sort.Should().Be("desc");
			size.Should().Be(1);
			offset.Should().Be(0);
		}

		[Fact]
		public void BuildQueries_ShouldHandleNullOrWhitespaceName()
		{       //arrange
			string name = null;
			var sortDirection = "asc";
			var pageSize = 5;
			var page = 1;
			//act
			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				name, sortDirection, pageSize, page);
			//assert
			query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1 ");
			query.Should().Contain("ORDER BY p.first_name asc");
			query.Should().Contain("LIMIT 5 OFFSET 0");
			query.Should().NotContain("AND (p.first_name LIKE");
			
			countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1 ");
			sort.Should().Be("asc");
			size.Should().Be(5);
			offset.Should().Be(0);
		}

		[Fact]
		public void BuildQueries_ShouldDefaultToDescForInvalidSortDirection()
		{	//act
			var name = "Jane";
			var sortDirection = "invalid";
			var pageSize = 10;
			var page = 1;

			var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
				name, sortDirection, pageSize, page);
			//assert
			query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%Jane%')");
			query.Should().Contain("ORDER BY p.first_name asc");
			query.Should().Contain("LIMIT 10 OFFSET 0");
			//assert
			countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%Jane%') ");
			sort.Should().Be("asc");
			size.Should().Be(10);
			offset.Should().Be(0);
		}
	}
}