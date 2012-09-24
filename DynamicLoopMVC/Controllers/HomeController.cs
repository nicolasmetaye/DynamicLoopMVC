using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DynamicLoopMVC.Data;
using DynamicLoopMVC.Data.Entities;
using DynamicLoopMVC.Data.Repositories;
using DynamicLoopMVC.Extensions;
using DynamicLoopMVC.Models;

namespace DynamicLoopMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(BooksListSuccessMessage message = BooksListSuccessMessage.None)
        {
            var books = new BookRepository().GetAll();
            var model = new BooksListModel
            {
                Books = GetBooksListModels(books)
            };
            if (message != BooksListSuccessMessage.None)
                model.SuccessMessage = message.GetDescriptionAttributeValue();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            var isbnFiltered = ISBNFilter.Filter(search);
            var books = new BookRepository().SearchFor(book => book.ISBN.Contains(isbnFiltered) || book.Title.Contains(search)).ToList();
            var model = new BooksListModel
            {
                Books = GetBooksListModels(books)
            };
            return View(model);
        }

        private List<BookListItemModel> GetBooksListModels(IEnumerable<Book> books)
        {
            return books.Select(book => new BookListItemModel
                                            {
                                                Id = book.Id,
                                                Title = book.Title,
                                                ISBN = book.ISBN,
                                                AuthorFullName = book.Author.FirstName + " " + book.Author.LastName
                                            }).ToList();
        }
    }
}
