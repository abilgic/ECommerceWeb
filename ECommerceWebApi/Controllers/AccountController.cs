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
        [ProducesResponseType(400, Type = typeof(Resp<ApplicationAccountResponseModel>))]
        public IActionResult Application([FromBody] ApplicationAccountRequestModel model)
        {
            Resp<ApplicationAccountResponseModel> response = new Resp<ApplicationAccountResponseModel>();
            //if (ModelState.IsValid)
            //{
            model.Username = model.Username.Trim().ToLower();
                if (_context.Accounts.Any(x => x.Username.ToLower() == model.Username)) 
                {
                    response.AddError(nameof(model.Username), "Kullanıcı adı kullanılmaktadır.");
                    return BadRequest(response);
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

                    ApplicationAccountResponseModel applicationAccountResponseModel = new ApplicationAccountResponseModel
                    {
                        Id = account.Id,
                        Username = account.Username,
                        ContactName = account.ContactName,
                        CompanyName = account.CompanyName,
                        ContactEmail =account.ContactEmail,
                    };

                
                response.Data = applicationAccountResponseModel;
                    return Ok(response);

                }
            
            //}
            //List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
            //return BadRequest(errors);
        }

        public class Resp<T>
        {
            public Dictionary<string, string[]> Errors { get; private set; }
            public T Data { get; set; }

            public void AddError(string key, params string[] errors)
            {
                if (Errors == null)
                    Errors = new Dictionary<string, string[]>();

                Errors.Add(key, errors);
            }

        }
    }
}
