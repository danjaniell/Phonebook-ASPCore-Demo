using Microsoft.AspNetCore.Mvc;
using Phonebook.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Phonebook.Models;
using System;
using System.Linq;
using Phonebook.Data.Repositories.Base;

namespace Phonebook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IRepositoryManager _repository;

        public ContactsController(IRepositoryManager repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _repository.Contacts.GetAllContacts(trackChanges: false);
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