using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.dao;
using API.Data;
using API.Dto;
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
            GenericSpecification<Product> specification = new GenericSpecification<Product>();
            specification.AddIncludes(x => x.ProductType);
            specification.AddIncludes(x => x.ProductBrand);

            List<Product> products = await _productRepository.GetEntityListWithSpec(specification);
            return Ok(products.Select(product => new ReturnProduct
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnProduct>> GetProduct(int id) {
            GenericSpecification<Product> specification = new GenericSpecification<Product>(criteria => criteria.Id == id);
            specification.AddIncludes(x => x.ProductType);
            specification.AddIncludes(x => x.ProductBrand);

            Product product = await _productRepository.GetEntityWithSpec(specification);
            return Ok(new ReturnProduct 
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            });
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