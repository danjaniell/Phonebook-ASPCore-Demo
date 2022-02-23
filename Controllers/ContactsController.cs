using Microsoft.AspNetCore.Mvc;
using Phonebook.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Phonebook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.ToListAsync();
            return View(contacts);
        }
    }
}
