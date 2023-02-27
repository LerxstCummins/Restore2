using System.Text.Json;
using API.Data;
using API.Entities;
using API.Extensions;
using API.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts([FromQuery] ProductParams productParams)
        {
            var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            int count = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(count / (double)productParams.PageSize);
            var products = await query.Skip((productParams.PageNumber - 1) * productParams.PageSize).Take(productParams.PageSize).ToListAsync();
            //var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            MetaData md = new MetaData
            {
                CurrentPage = productParams.PageNumber,
                TotalPages = totalPages,
                PageSize = productParams.PageSize,
                TotalCount = count
            };

            Response.AddPaginationHeader(md);
            //Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }
    }
}