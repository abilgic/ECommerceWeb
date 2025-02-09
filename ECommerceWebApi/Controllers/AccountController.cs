using ECommerceWebApi.DataAccess;
using ECommerceWebApi.Entities;
using ECommerceWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerceWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        //Application logic: Satıcı Başvurusu
        //Register: Üye kaydı
        //Authenticate: Kimlik doğrulama

        private DatabaseContext _context;
        public AccountController(DatabaseContext context)
        {
            this._context = context;

        }
        [HttpPost("merchant/application")]
        [ProducesResponseType(200,Type=typeof(ApplicationAccountResponseModel))]
        //[ProducesResponseType(400, Type = typeof(List<string>))]
        public IActionResult Application([FromBody] ApplicationAccountRequestModel model)
        {
            //if (ModelState.IsValid)
            //{
                model.Username = model.Username.Trim().ToLower();
                if (_context.Accounts.Any(x => x.Username.ToLower() == model.Username)) 
                {
                    ModelState.AddModelError(nameof(model.Username), "Kullanıcı adı kullanılmaktadır.");
                    return BadRequest(ModelState);
                }
                else
                {
                    Account account = new Account
                    {
                        Username = model.Username,
                        Password = model.Password,
                        CompanyName = model.CompanyName,
                        ContactName = model.ContactName,
                        ContactEmail = model.ContactEmail,
                        isBlocked = false,
                        isApplyment = true,
                        Type = AccountType.Merchant
                        
                    };
                    _context.Accounts.Add(account);
                    _context.SaveChanges();

                    ApplicationAccountResponseModel response = new ApplicationAccountResponseModel
                    {
                        Id = account.Id,
                        Username = account.Username,
                        ContactName = account.ContactName,
                        CompanyName = account.CompanyName,
                        ContactEmail =account.ContactEmail,
                    };
                    account.Password = null;
                    return Ok(response);

                }
            
            //}
            //List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
            //return BadRequest(errors);
        }
    }
}
