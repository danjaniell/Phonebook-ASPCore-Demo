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
            _repository = repository;
        }

        public IActionResult Index()
        {
            var contacts = _repository.Contacts.GetAllContacts(trackChanges: false);
            return View(contacts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Contacts.CreateContact(contact);
                    _repository.Save();

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
        public IActionResult Edit(int id)
        {
            var exist = _repository.Contacts.GetById(id, trackChanges: false);

            return View(exist);
        }

        [HttpPost]
        public IActionResult Edit(Contact request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var exist = _repository.Contacts.GetById(request.Id, trackChanges: false);

                    if (exist is not null)
                    {
                        _repository.Contacts.UpdateContact(request);
                        _repository.Save();
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
        public IActionResult Delete(int id)
        {
            var exist = _repository.Contacts.GetById(id, trackChanges: false);

            return View(exist);
        }

        [HttpPost]
        public IActionResult Delete(Contact request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var exist = _repository.Contacts.GetById(request.Id, trackChanges: false);

                    if (exist != null)
                    {
                        _repository.Contacts.DeleteContact(request);
                        _repository.Save();

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