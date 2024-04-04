using EShop.Domain.Domain;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IRepository<Product> productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }


        public Product CreateNewProduct(string userId, Product product)
        {
            var createdBy = _userRepository.Get(userId);
            product.CreatedBy = createdBy;
            return _productRepository.Insert(product);
        }

        public Product DeleteProduct(Guid id)
        {
            var product_to_delete = this.GetProductById(id);
            return _productRepository.Delete(product_to_delete);
        }

        public Product GetProductById(Guid? id)
        {
            return _productRepository.Get(id);
        }

        public List<Product> GetProducts()
        {
            return _productRepository.GetAll().ToList();
        }

        public Product UpdateProduct(Product product)
        {
           return _productRepository.Update(product);
        }
    }
}
