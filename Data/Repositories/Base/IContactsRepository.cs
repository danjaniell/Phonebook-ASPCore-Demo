using System.Collections.Generic;
using System.Threading.Tasks;
using Phonebook.Models;

namespace Phonebook.Data.Repositories.Base
{
    public interface IContactsRepository
    {
        Task<IEnumerable<Contact>> GetAllContacts(bool trackChanges);
        Task<Contact> GetById(int id, bool trackChanges);
        Task CreateContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
    }
}