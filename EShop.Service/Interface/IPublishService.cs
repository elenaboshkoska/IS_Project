using EShop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IPublishService
    {
        public List<Publishers> GetPublish();
        public Publishers GetPublisherById(Guid id);
        public void CreateNewPublish(Publishers r, string customerId);
        public Publishers DeletePublish(Guid? id);

        public Publishers UpdatePublish(Publishers rent);
    }
}
