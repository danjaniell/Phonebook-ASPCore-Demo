using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.Data.Repositories.Base;
using Phonebook.Models;

namespace Phonebook.Data.Repositories
{
    public class ContactsRepository : RepositoryBase<Contact>, IContactsRepository
    {
        public ContactsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task CreateContact(Contact contact) => await Create(contact);

        public void DeleteContact(Contact contact) => Delete(contact);

        public async Task<IEnumerable<Contact>> GetAllContacts(bool trackChanges) => await FindAll(trackChanges).OrderBy(x => x.LastName).ToListAsync();

        public async Task<Contact> GetById(int id, bool trackChanges) => await FindByCondition((x) => x.Id == id, trackChanges).SingleOrDefaultAsync();

        public void UpdateContact(Contact contact) => Update(contact);
    }
}