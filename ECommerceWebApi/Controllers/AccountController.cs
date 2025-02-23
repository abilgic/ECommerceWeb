using ECommerce.Core;
using ECommerce.Core.Models;
using ECommerceWebApi.DataAccess;
using ECommerceWebApi.Entities;
using ECommerceWebApi.myServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static ECommerce.Core.AccountController;
using static ECommerceWebApi.Controllers.AccountController;

namespace ECommerceWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class AccountController : ControllerBase
    {
        //Application logic: Satıcı Başvurusu
        //Register: Üye kaydı
        //Authenticate: Kimlik doğrulama

        private DatabaseContext _context;
        private IConfiguration _configuration;
        public AccountController(DatabaseContext context, IConfiguration configuration)
        {
            this._context = context;
            _configuration = configuration;

        }
        [HttpPost("merchant/application")]
        [ProducesResponseType(200,Type=typeof(Resp<ApplicationAccountResponseModel>))]
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

        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(Resp<RegisterResponseModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<RegisterResponseModel>))]
        public IActionResult Register([FromBody] RegisterRequestModel model)
        {
            Resp<RegisterResponseModel> response = new Resp<RegisterResponseModel>();
            model.Username = model.Username.Trim().ToLower();
            if(_context.Accounts.Any(x=>x.Username.ToLower()==model.Username))
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
                    isBlocked = false,
                    isApplyment = false,
                    CompanyName ="TestCompany",
                    ContactName ="Ahmet",
                    ContactEmail="a@b.c",
                    Type = AccountType.Member

                };
                _context.Accounts.Add(account);
                _context.SaveChanges();
                RegisterResponseModel data = new RegisterResponseModel
                {
                    Id = account.Id,
                    Username = account.Username
                };
                response.Data = data;
                return Ok(response);
            }

        }

        [HttpPost("authenticate")]
        [ProducesResponseType(200, Type = typeof(Resp<AuthenticateResponseModel>))]
        [ProducesResponseType(400, Type = typeof(Resp<AuthenticateResponseModel>))]
        public IActionResult Authenticate([FromBody]AuthenticateRequestModel model)
        {
            Resp<AuthenticateResponseModel> response = new Resp<AuthenticateResponseModel>();
            model.Username = model.Username.Trim().ToLower();
            Account account = _context.Accounts.SingleOrDefault(x => x.Username == model.Username.ToLower() && x.Password == model.Password.ToLower());
            if (account != null)
            {
                if (account.isApplyment)
                {
                    response.AddError("*", "Başvuru henüz tamamlanmamıştır.");
                    return BadRequest(response);
                }
                else
                {
                    string key = _configuration["JwtOptions:Key"];
                    List<Claim> claims = new List<Claim>
                                    {
                                        new Claim("id", account.Id.ToString()),
                                        new Claim(ClaimTypes.Name, account.Username),
                                        new Claim("type", account.Type.ToString()),
                                        new Claim(ClaimTypes.Role, account.Type.ToString())
                                    };
                    string tokenString = TokenService.GenerateToken(key, DateTime.Now.AddDays(30), claims);
                    response.Data = new AuthenticateResponseModel
                    {
                        Token = tokenString
                    };
                    return Ok(response);
                    //Token üretilecek
                    //Response'ta token dönülecek
                }

            }
            else
            {
                response.AddError("*", "Kullanıcı adı veya şifre hatalı.");
                return BadRequest(response);
            }
            return Ok();
        }

        
    }
}
