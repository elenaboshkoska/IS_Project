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
    public class PublishService : IPublishService
    {
        private readonly IRepository<Publishers> publishRepository;
        private readonly IPublishRepository _publishRepository;
        private readonly IBookService bookService;
        public PublishService(IRepository<Publishers> publishRepository, IBookService bookService, IPublishRepository _publishRepository)
        {
            this.publishRepository = publishRepository;
            this.bookService = bookService;
            this._publishRepository = _publishRepository;
        }
        public void CreateNewPublish(Publishers r, string authorId)
        {
            if (bookService.AvailableCheck(r.BookId))
            {
                bookService.SetBookAsUnavailable(r.BookId);
                r.AuthorsId = authorId;
                publishRepository.Insert(r);
            }
            //else return exception
        }

        /// <summary>
        /// Deletes a rent and sets the associated car as available.This is an atomic operation, 
        /// so changes to the Car and Rent are made within a try/catch block to ensure consistency.
        /// </summary>
        /// <param name="id">The unique identifier of the rent to be deleted.</param>
        /// <returns>Deleted rent</returns>
        public Publishers DeletePublish(Guid? id)
        {
            Publishers publish = _publishRepository.GetDetailsForRent(id);
            try
            {
                publishRepository.Delete(publish);
                bookService.SetBookAsAvailable(publish.BookId);
            }
            catch
            {
                //No rent exception
            }
            return publish;
        }

        public Publishers GetPublisherById(Guid id)
        {
            return _publishRepository.GetDetailsForRent(id);
        }

        public List<Publishers> GetPublish()
        {
            return _publishRepository.GetAllPublishers();
        }

        public Publishers UpdatePublish(Publishers publish)
        {
            return publishRepository.Update(publish);
        }
    }
}
