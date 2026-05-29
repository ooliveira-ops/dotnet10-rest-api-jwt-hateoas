using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RestWithASPNET10Erudio.Model.Base;

namespace RestWithASPNET10Erudio.Repositories
{                                                               //só posso add repositórios genéricos(IRepository) para as classes que estendam 'BaseEntity' 
	public interface IRepository<T> where T : BaseEntity           //vai ser de um tipo dinamico; tipo 'T' : condição = 'qaunto T'= for do tipo 'BaseEntity' (para que implemente a interface corretam.)
	{
		List<T> FindALL();

		T FindByID(long id);                        //long = nao terá problema no banco de dados se estiver com muitas pessoas cadastradas. Agora se for o Int(no lugar do 'long'), pode ser que tenha problemas no banco de dados

		T Create(T item);                           //método chama 'Create' e a assinatura 'T'.

		T Update(T item);

		void Delete(long id);

		bool Exists(long id);

		List<T> FindWithPagedSearch(string query);
		int GetCount(string query);
		List<T> FindByName(string firstName, string lastName); 
	}
}
