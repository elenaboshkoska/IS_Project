using EShop.Domain.Domain;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public bool AvailableCheck(Guid? id)
        {
            return bookRepository.Get(id).IsAvailable == true;
        }

        public void CreateNewBook(Book c)
        {
            c.IsAvailable = true;
            //Authors a = new Authors();
            //a.FirstName = "Elena";
            //c.author = a;
            bookRepository.Insert(c);
        }

        public Book DeleteBook(Guid id)
        {
            Book car = bookRepository.Get(id);
            bookRepository.Delete(car);
            return car;
        }

        public Book GetBookById(Guid? id)
        {
            return bookRepository.Get(id);
        }

        public List<Book> GetBooks()
        {
            return bookRepository.GetAll().ToList();
        }

        public void SetBookAsAvailable(Guid? id)
        {
            Book car = bookRepository.Get(id);
            car.IsAvailable = true;
            bookRepository.Update(car);
        }

        public void SetBookAsUnavailable(Guid? id)
        {
            Book car = bookRepository.Get(id);
            car.IsAvailable = false;
            bookRepository.Update(car);
        }

        public Book UpdateBook(Book id)
        {
            return bookRepository.Update(id);
        }
    }
}
