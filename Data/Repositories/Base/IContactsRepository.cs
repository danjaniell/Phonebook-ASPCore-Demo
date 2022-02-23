using System.Collections.Generic;
using Phonebook.Models;

namespace Phonebook.Data.Repositories.Base
{
    public interface IContactsRepository
    {
        IEnumerable<Contact> GetAllContacts(bool trackChanges);
        void CreateContact(Contact contact);
    }
}