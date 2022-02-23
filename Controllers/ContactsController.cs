using Microsoft.AspNetCore.Mvc;
using Phonebook.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Phonebook.Models;
using System;
using System.Linq;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(contact);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");

            return View(contact);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var exist = await _context.Contacts.Where(x => x.Id == id).FirstOrDefaultAsync();

            return View(exist);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Contact request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var exist = await _context.Contacts.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                    if (exist is not null)
                    {
                        exist.FirstName = request.FirstName;
                        exist.LastName = request.LastName;
                        exist.Mobile = request.Mobile;
                        exist.Email = request.Email;

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");

            return View(request);
        }
    }
}
