using System.Threading.Tasks;

namespace Phonebook.Data.Repositories.Base
{
    public interface IRepositoryManager
    {
        IContactsRepository Contacts { get; }
        Task SaveAsync();
    }
}