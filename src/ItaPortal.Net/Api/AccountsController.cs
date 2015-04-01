using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using ItaPortal.Net.Models.Identity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using ItaPortal.Net.Services;
using ItaPortal.Net.Data;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ItaPortal.Net.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : BaseController
    {
        public IMessageService MessageService { get; set; }
        public IAuthRepository AuthRepository { get; set; }
        public AccountsController(IAuthRepository authRepository, IMessageService messageService, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            AuthRepository = authRepository;
            MessageService = messageService;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<ApplicationUser> GetUsers()
        {
            return this.UserManager.Users.ToList();

            //return (this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            return new ObjectResult(user);
        }

        private IActionResult CheckIfModelIsInvalid()
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            return new EmptyResult();
        }
        [AllowAnonymous]
        [HttpPost(Name ="Register")]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody]UserModel userModel)
        {
            var result = CheckIfModelIsInvalid();
            if (result is EmptyResult)
            {
                IdentityResult identityResult = await AuthRepository.RegisterUser(userModel);

                IActionResult errorResult = GetErrorResult(identityResult);

                if (errorResult != null)
                {
                    return errorResult;
                }

                Context.Response.StatusCode = 200;
                return new ObjectResult(userModel);
            }
            return result;
        }

        [HttpPost(Name ="InviteUser")]
        [ActionName("inviteuser")]
        [Route("[action]")]
        public async Task<IActionResult> InviteUser([FromBody]InviteUserBindingModel inviteUserModel)
        {
            Context.Response.StatusCode = 400;
            var result = new ObjectResult(new { Error = "User model is invalid" });
            if (!ModelState.IsValid)
            {
                return result;
            }
            var user = await UserManager.FindByEmailAsync(inviteUserModel.Email);
            if(user != null)
            {                
                var isInvitationSent = await InviteUserHelper(user);
                if(isInvitationSent)
                {
                    Context.Response.StatusCode = 200;
                    return new ObjectResult(user);
                }
            }
            return result;            
        }

        private async Task<bool> InviteUserHelper(ApplicationUser user)
        {
           string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
           var invitationConfirmationEmailLink = Configuration.Get("Application:BaseUrl") + "?code=" + code;

            var invitationMessage = new InvitationMessage
            {
                Destination = user.Email,
                Body = string.Format(@"Dear {0} {1}, <br/>
<br/>
You are invited to register with ItaPortal.Net. Please click <a href= ""{2}"">here</a> to register.
", user.FirstName, user.LastName, invitationConfirmationEmailLink),
                Subject = "New User Invitation"
            };
           
            await MessageService.SendAsync(invitationMessage);

            return true;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserBindingModel createUserModel)
        {
            Context.Response.StatusCode = 400;
            var result = new ObjectResult(new  { Error = "User model is invalid" });
            if (!ModelState.IsValid)
            {
                return result;
            }

            var newUser = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                SecurityQuestion = createUserModel.SecurityQuestion,
                SecurityAnswer = createUserModel.SecurityAnswer,
                Level = 3,
                JoinDate = DateTime.Now.Date,
            };

            IdentityResult addUserResult = await this.UserManager.CreateAsync(newUser, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            string url = Url.RouteUrl("GetUserById", new { userId = newUser.Id }, Request.Scheme, Request.Host.ToUriComponent());
            return new CreatedResult(url, newUser);                           
        }
    }
}
