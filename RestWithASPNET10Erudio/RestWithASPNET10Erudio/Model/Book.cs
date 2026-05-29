using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestWithASPNET10Erudio.Model.Base;

namespace RestWithASPNET10Erudio.Model
{
	// O nome da tabela no banco é 'books'
	[Table("books")]
	public class Book : BaseEntity
	{
		[Required]
		[Column("title", TypeName = "varchar(MAX)")]
		public string Title { get; set; }

		[Required]
		[Column("author", TypeName = "varchar(MAX)")]
		public string Author { get; set; }

		[Required]
		[Column("price", TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
		
		[Required]
		[Column("launch_date")]
		public DateTime LaunchDate { get; set; }



	}
}