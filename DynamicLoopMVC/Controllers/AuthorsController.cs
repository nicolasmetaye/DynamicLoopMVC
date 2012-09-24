using System.Linq;
using System.Web.Mvc;
using DynamicLoopMVC.Data;
using DynamicLoopMVC.Data.Entities;
using DynamicLoopMVC.Data.Repositories;
using DynamicLoopMVC.Extensions;
using DynamicLoopMVC.Models;

namespace DynamicLoopMVC.Controllers
{
    public class AuthorsController : Controller
    {
        public ActionResult Add()
        {
            return View("Edit", new AuthorModel
                                    {
                                        IsEditMode = false
                                    });
        }

        public ActionResult Edit(int id)
        {
            var author = new AuthorRepository().GetById(id);
            if (author == null)
                return RedirectToAction("Index");
            return View(new AuthorModel
                            {
                                FirstName = author.FirstName,
                                LastName = author.LastName,
                                Id = author.Id,
                                IsEditMode = true
                            });
        }

        public ActionResult Index(AuthorsListSuccessMessage message = AuthorsListSuccessMessage.None)
        {
            var authors = new AuthorRepository().GetAll();
            var model = new AuthorsListModel
                            {
                                Authors = authors.Select(author => new AuthorListItemModel
                                                                       {
                                                                           Id = author.Id,
                                                                           FullName = author.FirstName + " " + author.LastName
                                                                       }).ToList()
                            };
            if (message != AuthorsListSuccessMessage.None)
                model.SuccessMessage = message.GetDescriptionAttributeValue();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AuthorModel model)
        {
            if (ModelState.IsValid)
            {
                new AuthorRepository().Insert(new Author
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    });
                return RedirectToAction("Index", new { message = (int)AuthorsListSuccessMessage.AuthorAddedSuccesfully });
            }
            model.IsEditMode = false;
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit(AuthorModel model)
        {
            if (ModelState.IsValid)
            {
                new AuthorRepository().Save(new Author
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                });
                return RedirectToAction("Index", new { message = (int)AuthorsListSuccessMessage.AuthorEditedSuccesfully });
            }
            model.IsEditMode = true;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (new BookRepository().Any(book => book.AuthorId == id))
                return RedirectToAction("Index", new { message = (int)AuthorsListSuccessMessage.AuthorNotDeletedSuccesfully });
            new AuthorRepository().Delete(id);
            return RedirectToAction("Index", new { message = (int)AuthorsListSuccessMessage.AuthorDeletedSuccesfully });
        }
    }
}
