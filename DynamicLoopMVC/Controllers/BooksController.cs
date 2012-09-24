using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using DynamicLoopMVC.Data;
using DynamicLoopMVC.Data.Entities;
using DynamicLoopMVC.Data.Repositories;
using DynamicLoopMVC.Models;

namespace DynamicLoopMVC.Controllers
{
    public class BooksController : Controller
    {
        public ActionResult Add()
        {
            return View("Edit", new BookModel
            {
                Authors = GetAuthorsList(),
                IsEditMode = false
            });
        }

        public ActionResult Edit(int id)
        {
            var book = new BookRepository().GetById(id);
            if (book == null)
                return RedirectToAction("Index", "Home");
            return View(new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                AuthorId = book.AuthorId,
                Authors = GetAuthorsList(),
                IsEditMode = true
            });
        }

        [HttpPost]
        public ActionResult Add(BookModel model)
        {
            if (ModelState.IsValid)
            {
                var isbn = ISBNFilter.Filter(model.ISBN);
                new BookRepository().Insert(new Book
                {
                    ISBN = isbn,
                    Title = model.Title,
                    AuthorId = model.AuthorId,
                });
                return RedirectToAction("Index", "Home", new { message = (int)BooksListSuccessMessage.BookAddedSuccesfully });
            }
            model.IsEditMode = false;
            model.Authors = GetAuthorsList();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(BookModel model)
        {
            if (ModelState.IsValid)
            {
                var isbn = ISBNFilter.Filter(model.ISBN);
                new BookRepository().Save(new Book
                {
                    Id = model.Id,
                    ISBN = isbn,
                    Title = model.Title,
                    AuthorId = model.AuthorId
                });
                return RedirectToAction("Index", "Home", new { message = (int)BooksListSuccessMessage.BookEditedSuccesfully });
            }
            model.IsEditMode = true;
            model.Authors = GetAuthorsList();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            new BookRepository().Delete(id);
            return RedirectToAction("Index", "Home", new { message = (int)BooksListSuccessMessage.BookDeletedSuccesfully });
        }

        private List<AuthorListItemModel> GetAuthorsList()
        {
            return
                new AuthorRepository()
                .GetAll()
                .Select(author => new AuthorListItemModel {Id = author.Id, FullName = author.FirstName + " " + author.LastName})
                .ToList();
        }
    }
}
