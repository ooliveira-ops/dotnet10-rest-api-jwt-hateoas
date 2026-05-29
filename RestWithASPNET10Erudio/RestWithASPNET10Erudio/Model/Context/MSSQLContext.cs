 using Microsoft.EntityFrameworkCore;
using RestWithASPNET10Erudio.Repositories.Impl;
using RestWithASPNET10Erudio.Model;
public static class DatabaseConfig;

namespace RestWithASPNET10Erudio.Model.Context
{
	public class MSSQLContext(DbContextOptions<MSSQLContext> options) : DbContext(options) 
	{

		//cada entidade que cria, add aqui:

		public DbSet<Person> Persons { get; set;  }                             //tebelas do banco, voce mapeia; cada tabela do banco, vc cria uma entidade.
		public DbSet<Book> Books { get; set; }

		public DbSet<User> Users { get; set; }

		
		}
	}

