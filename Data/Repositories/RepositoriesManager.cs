using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Phonebook.Data.Repositories.Base;
using Phonebook.Models;
using Phonebook.Services.Base;

namespace Phonebook.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private IContactsRepository _contactsRepository;
        private readonly ICacheService<Contact> _contactsCacheService;

        public RepositoryManager(ApplicationDbContext context, ICacheService<Contact> contactsCacheService)
        {
            _context = context;
            _contactsCacheService = contactsCacheService;
        }

        public IContactsRepository Contacts
        {
            get
            {
                return _contactsRepository ??= new ContactsRepository(_context, _contactsCacheService);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}