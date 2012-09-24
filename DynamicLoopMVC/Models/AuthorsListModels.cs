using System.Collections.Generic;

namespace DynamicLoopMVC.Models
{
    public class AuthorsListModel
    {
        public List<AuthorListItemModel> Authors { get; set; }
        public string SuccessMessage { get; set; }
    }
}
