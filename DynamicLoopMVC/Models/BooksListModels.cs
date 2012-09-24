using System.Collections.Generic;

namespace DynamicLoopMVC.Models
{
    public class BooksListModel
    {
        public List<BookListItemModel> Books { get; set; }
        public string SuccessMessage { get; set; }
    }
}
