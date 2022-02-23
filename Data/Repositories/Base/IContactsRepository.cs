using System.Collections.Generic;
using Phonebook.Models;

namespace Phonebook.Data.Repositories.Base
{
    public interface IContactsRepository
    {
        IEnumerable<Contact> GetAllContacts(bool trackChanges);
        Contact GetById(int id, bool trackChanges);
        void CreateContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
    }
}