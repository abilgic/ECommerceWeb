using ECommerce.Core.Models;
using ECommerceWebApi.DataAccess;
using ECommerceWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ECommerce.Core.AccountController;

namespace ECommerceWebApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Merchant")]
    public class CategoryController : ControllerBase
    {
        private DatabaseContext _context;
        private IConfiguration _configuration;
        public CategoryController(DatabaseContext context, IConfiguration configuration)
        {
            this._context = context;
            _configuration = configuration;

        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(Resp<List<CategoryModel>>))]
        public IActionResult List()
        { Resp<List<CategoryModel>> response = new Resp<List<CategoryModel>>();

            List<CategoryModel> list =  _context.Categories.Select(
                x=>new CategoryModel { Id = x.Id, Description =x.Description, Name = x.Name}).ToList();
            response.Data = list;
            return Ok(response);

        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<CategoryModel>))]
        public IActionResult GetById([FromRoute] int id)
        {
            Resp<CategoryModel> response = new Resp<CategoryModel>();
            CategoryModel data = null;           

            Category category = _context.Categories.SingleOrDefault(
                x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            if (category != null)
            {
                data = new CategoryModel { Id = category.Id, Description = category.Description, Name = category.Name };
            }
            response.Data = data;
            return Ok(response);

        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<CategoryModel>))]
        public IActionResult Create(CategoryCreateModel model)
        {
            Resp<CategoryModel> response = new Resp<CategoryModel>();
            string categoryName = model.Name?.Trim().ToLower();
            var categories = _context.Categories.ToList();
            if (_context.Categories.Any(x=>x.Name.ToLower()==categoryName))
            {
                response.AddError(nameof(model.Name), "Bu category adı zaten mevcuttur.");
                return BadRequest(response);

            }
            else
            {
                Category category = new Category
                {
                    Name = model.Name,
                    Description = model.Description
                };
                _context.Categories.Add(category);
                _context.SaveChanges();

                CategoryModel data = new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                response.Data = data;
                return Ok(response);

            }

        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(200, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<CategoryModel>))]
        [ProducesResponseType(404, Type = typeof(Resp<CategoryModel>))]
        public IActionResult Uptade([FromRoute] int id, [FromBody]CategoryUpdateModel model)
        {
            Resp<CategoryModel> response = new Resp<CategoryModel>();
            Category category = _context.Categories.Find(id);
            
            if (category == null)
            {
                return NotFound(response);
            }

            string categoryName = model.Name?.Trim().ToLower();
            if(_context.Categories.Any(x=>x.Name.ToLower()==categoryName && x.Id != id))
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
