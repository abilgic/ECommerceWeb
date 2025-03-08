using ECommerce.Core.Models;
using ECommerceWebApi.DataAccess;
using ECommerceWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static ECommerce.Core.AccountController;

namespace ECommerceWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private DatabaseContext _context;
        private IConfiguration _configuration;
        public ProductController(DatabaseContext context, IConfiguration configuration)
        {
            this._context = context;
            _configuration = configuration;

        }
        [HttpGet("debug-claims")]
        [AllowAnonymous] // Allow access without authentication
        public IActionResult DebugClaims()
        {
            var claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(Resp<List<ProductModel>>))]
        public IActionResult List()
        {
            Resp<List<ProductModel>> response = new Resp<List<ProductModel>>();
            //int accountId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value);
            List<ProductModel> list = _context.Products
                .Include(x=>x.Category)
                .Include(x=>x.Account)
                //.Where(x=>x.AccountId==accountId)
                .Select(x => new ProductModel 
                { 
                    Id = x.Id, 
                    Description = x.Description, 
                    Name = x.Name,
                    UnitPrice = x.UnitPrice,
                    Discontinued = x.Discontinued,
                    CategoryId = x.CategoryId,
                    AccountId = x.AccountId,
                    CategoryName = x.Category.Name,
                    AccountCompanyName = x.Account.CompanyName
                }).ToList();
            response.Data = list;
            return Ok(response);

        }

        [HttpGet("list/{accountId}")]
        [ProducesResponseType(200, Type = typeof(Resp<List<ProductModel>>))]
        public IActionResult ListByAccountId([FromRoute] int accountId)
        {
            Resp<List<ProductModel>> response = new Resp<List<ProductModel>>();
            //int accountId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value);
            List<ProductModel> list = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Account)
                //.Where(x=>x.AccountId==accountId)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    UnitPrice = x.UnitPrice,
                    Discontinued = x.Discontinued,
                    CategoryId = x.CategoryId,
                    AccountId = x.AccountId,
                    CategoryName = x.Category.Name,
                    AccountCompanyName = x.Account.CompanyName
                }).ToList();
            response.Data = list;
            return Ok(response);

        }

        [HttpGet("get/{productId}")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<CategoryModel>))]
        public IActionResult GetById([FromRoute] int productId)
        {
            Resp<ProductModel> response = new Resp<ProductModel>();            

            Product product = _context.Products
                .Include (x => x.Category)
                .Include(x => x.Account)
                .SingleOrDefault(
                x => x.Id == productId);

            if (product == null)
            {
                return NotFound();
            }
            ProductModel data = new ProductModel 
            { 
                Id = product.Id, 
                Description = product.Description, 
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                Discontinued = product.Discontinued,
                CategoryId = product.CategoryId,
                AccountId = product.AccountId,
                CategoryName = product.Category.Name,
                AccountCompanyName = product.Account.CompanyName
            };
            
            response.Data = data;
            return Ok(response);

        }
       

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<CategoryModel>))]
        public IActionResult Create(ProductCreateModel model)
        {
            Resp<ProductModel> response = new Resp<ProductModel>();
            // Ensure CategoryId exists
            bool categoryExists = _context.Categories.Any(c => c.Id == model.CategoryId);
            if (!categoryExists)
            {
                response.AddError(nameof(model.CategoryId), "Invalid CategoryId. Category does not exist.");
                return BadRequest(response);
            }
            string productName = model.Name?.Trim().ToLower();
            var categories = _context.Categories.ToList();
            if (_context.Products.Any(x => x.Name.ToLower() == productName))
            {
                response.AddError(nameof(model.Name), "Bu ürün adı zaten mevcuttur.");
                return BadRequest(response);

            }
            else
            {
                int accountId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c =>
    c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

                //int accountId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c=>c.Type=="id").Value);

                Product product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    UnitPrice = model.UnitPrice,
                    Discontinued = model.Discontinued,
                    CategoryId = model.CategoryId,
                    AccountId = accountId
                };
                _context.Products.Add(product);
                _context.SaveChanges();

               product= _context.Products.Include(x => x.Category).Include(x => x.Account).SingleOrDefault(x=>x.Id == product.Id);

                ProductModel data = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    Discontinued = product.Discontinued,
                    CategoryId = product.CategoryId,
                    AccountId = product.AccountId,
                    CategoryName = product.Category.Name,
                    AccountCompanyName = product.Account.CompanyName
                };

                response.Data = data;
                return Ok(response);

            }

        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<CategoryModel>))]
        public IActionResult Uptade([FromRoute] int id, [FromBody] CategoryUpdateModel model)
        {
            Resp<CategoryModel> response = new Resp<CategoryModel>();
            Category category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound(response);
            }

            string categoryName = model.Name?.Trim().ToLower();
            if (_context.Categories.Any(x => x.Name.ToLower() == categoryName && x.Id != id))
            {
                response.AddError(nameof(model.Name), "Bu kategory adı zaten mevcuttur.");
                return BadRequest(response);
            }
            category.Name = model.Name;
            category.Description = model.Description;
            _context.SaveChanges();

            CategoryModel data = new CategoryModel { Id = category.Id, Description = category.Description, Name = category.Name };

            response.Data = data;
            return Ok(response);

        }
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<CategoryModel>))]
        public IActionResult Delete([FromRoute] int id)
        {
            Resp<object> response = new Resp<object>();
            Category category = _context.Categories.Find(id);
            if (category == null)
                return NotFound(response);

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok(response);


        }
    }
}
