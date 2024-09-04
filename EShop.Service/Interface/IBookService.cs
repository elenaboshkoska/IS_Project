using EShop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IBookService
    {
        public List<Book> GetBooks();
        public Book GetBookById(Guid? id);
        public void CreateNewBook(Book c);
        public Book DeleteBook(Guid id);

        public Book UpdateBook(Book id);

        public bool AvailableCheck(Guid? id);

        public void SetBookAsUnavailable(Guid? id);
        public void SetBookAsAvailable(Guid? id);
    }
}
