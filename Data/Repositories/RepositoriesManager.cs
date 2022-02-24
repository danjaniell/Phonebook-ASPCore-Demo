using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Phonebook.Data.Repositories.Base;
using Phonebook.Models;

namespace Phonebook.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private IContactsRepository _contactsRepository;

        public RepositoryManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public IContactsRepository Contacts
        {
            get
            {
                return _contactsRepository ??= new ContactsRepository(_context);
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}