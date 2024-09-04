using EShop.Domain.Domain;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Implementation
{
    public class PublishRepository : IPublishRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Publishers> entities;

        public PublishRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Publishers>();
        }
        public List<Publishers> GetAllPublishers()
        {
            return entities.Include(r => r.Book).ToList();
        }

        public Publishers GetDetailsForRent(Guid? id)
        {
            return entities.Include(r => r.Author).Include(r => r.Book).
                Where(r => r.Id == id).FirstOrDefault();
        }

        List<Publishers> IPublishRepository.GetAllPublishers()
        {
            throw new NotImplementedException();
        }

        Publishers IPublishRepository.GetDetailsForRent(Guid? id)
        {
            throw new NotImplementedException();
        }
    }
}
