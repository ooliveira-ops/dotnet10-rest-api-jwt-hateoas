using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET10Erudio.Model.Base
{
	public class BaseEntity
	{
		[Key]                                   // O ID é automático, mas precisamos dele na classe
		[Column("id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
	
	}
}
