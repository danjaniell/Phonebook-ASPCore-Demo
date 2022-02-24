using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.Data.Repositories.Base;
using Phonebook.Models;
using Phonebook.Services.Base;

namespace Phonebook.Data.Repositories
{
    public class ContactsRepository : RepositoryBase<Contact>, IContactsRepository
    {
        private readonly ICacheService<Contact> _contactsCacheService;

        public ContactsRepository(ApplicationDbContext context, ICacheService<Contact> contactsCacheService) : base(context)
        {
            _contactsCacheService = contactsCacheService ?? throw new ArgumentNullException(nameof(contactsCacheService));
        }

        public async Task CreateContact(Contact contact) => await Create(contact);

        public void DeleteContact(Contact contact) => Delete(contact);

        public async Task<IEnumerable<Contact>> GetAllContacts(bool trackChanges) => await FindAll(trackChanges).OrderBy(x => x.LastName).ToListAsync();

        public async Task<Contact> GetById(int id, bool trackChanges) => await FindByCondition((x) => x.Id == id, trackChanges).SingleOrDefaultAsync();

        public void UpdateContact(Contact contact) => Update(contact);
    }
}