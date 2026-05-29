using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestWithASPNET10Erudio.Model.Base;

namespace RestWithASPNET10Erudio.Model
{                                                    // Abre Namespace

	 [Table("person")]
	public class Person  : BaseEntity                         //classe 'Person' irá ter: ID, Nome, Segundo Nome, Endereço e Gênero.
	{                                                       // Abre Class

		[Required]
		[Column("first_name", TypeName = "varchar(80)")]								//varchar =  tipo de dados; uso p texto
		[MaxLength(80)]																	//maxlenght:tamanho do maximo
		public string FirstName { get; set; }

		[Required]
		[Column("last_name", TypeName = "varchar(80)")]
		[MaxLength(80)]
		public string LastName { get; set; }

		[Required]
		[Column("address", TypeName = "varchar(100)")]
		[MaxLength(100)]
		public string Address { get; set; }

		[Required]
		[Column("gender", TypeName = "varchar(6)")]
		public string Gender { get; set; }

		//[Column("birth_day", TypeName = "datetime")]
		// public DateTime? BirthDay { get; set; }

		[Column("enabled")]
		public bool Enabled { get; set; }


		//[NotMapped]
		//public DateTime? BirthDay { get; set; } 
	}																// Fecha Classe
}																 // Fecha Namespace