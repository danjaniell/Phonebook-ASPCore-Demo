using System;
using System.Collections.Generic;
using System.Linq;
using Phonebook.Data.Repositories.Base;
using Phonebook.Models;

namespace Phonebook.Data.Repositories
{
    public class ContactsRepository : RepositoryBase<Contact>, IContactsRepository
    {
        public ContactsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CreateContact(Contact contact) => Create(contact);

        public void DeleteContact(Contact contact) => Delete(contact);

        public IEnumerable<Contact> GetAllContacts(bool trackChanges) => FindAll(trackChanges).OrderBy(x => x.LastName).ToList();

        public Contact GetById(int id, bool trackChanges) => FindByCondition((x) => x.Id == id, trackChanges).SingleOrDefault();

        public void UpdateContact(Contact contact) => Update(contact);
    }
}