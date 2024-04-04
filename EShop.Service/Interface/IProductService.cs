using EShop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IProductService
    {
        public List<Product> GetProducts();
        public Product GetProductById(Guid? id);
        public Product CreateNewProduct(string userId, Product product);
        public Product UpdateProduct(Product product);
        public Product DeleteProduct(Guid id);
    }
}
