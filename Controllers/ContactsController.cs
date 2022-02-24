using Microsoft.AspNetCore.Mvc;
using Phonebook.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Phonebook.Models;
using System;
using System.Linq;
using Phonebook.Data.Repositories.Base;
using Phonebook.Common;

namespace Phonebook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IRepositoryManager _repository;

        public ContactsController(IRepositoryManager repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNum)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FNameSortParm"] = sortOrder == "fname_asc" ? "fname_desc" : "fname_asc";
            ViewData["LNameSortParm"] = sortOrder == "lname_asc" ? "lname_desc" : "lname_asc";
            ViewData["EmailSortParm"] = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewData["MobileSortParm"] = sortOrder == "mobile_asc" ? "mobile_desc" : "mobile_asc";

            if (searchString is null) { pageNum = 1; }
            searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var contacts = await _repository.Contacts.GetAllContacts(trackChanges: false);

            if (!string.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(s => s.LastName.Contains(searchString) ||
                                               s.FirstName.Contains(searchString));
            }

            contacts = sortOrder switch
            {
                "fname_asc" => contacts.OrderBy(s => s.FirstName),
                "fname_desc" => contacts.OrderByDescending(s => s.FirstName),
                "lname_asc" => contacts.OrderBy(s => s.LastName),
                "lname_desc" => contacts.OrderByDescending(s => s.LastName),
                "email_asc" => contacts.OrderBy(s => s.Email),
                "email_desc" => contacts.OrderByDescending(s => s.Email),
                "mobile_asc" => contacts.OrderBy(s => s.Mobile),
                "mobile_desc" => contacts.OrderByDescending(s => s.Mobile),
                _ => contacts.OrderBy(s => s.Id),
            };

            const int pageSize = 25;

            return View(PaginatedList<Contact>.Create(contacts, pageNum ?? 1, pageSize));
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
                    await _repository.Contacts.CreateContact(contact);
                    await _repository.SaveAsync();

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
            var exist = await _repository.Contacts.GetById(id, trackChanges: false);

            return View(exist);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Contact request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var exist = await _repository.Contacts.GetById(request.Id, trackChanges: false);

                    if (exist is not null)
                    {
                        _repository.Contacts.UpdateContact(request);
                        await _repository.SaveAsync();
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _repository.Contacts.GetById(id, trackChanges: false);

            return View(exist);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Contact request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var exist = await _repository.Contacts.GetById(request.Id, trackChanges: false);

                    if (exist != null)
                    {
                        _repository.Contacts.DeleteContact(request);
                        await _repository.SaveAsync();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");

            return View();
        }
    }
}