using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.dao;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IGenericRepository<Product> _productRepository;
        private IGenericRepository<ProductBrand> _productBrandRepository;
        private IGenericRepository<ProductType> _productTypeRepository;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts() {
            List<Product> products = await _productRepository.GetAllAsync();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) {
            Product product = await _productRepository.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands() {
            return await _productBrandRepository.GetAllAsync();
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes() {
            return await _productTypeRepository.GetAllAsync();
        }
        
    }
}