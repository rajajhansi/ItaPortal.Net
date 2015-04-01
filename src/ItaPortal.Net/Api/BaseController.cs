using Microsoft.AspNet.Mvc;
using ItaPortal.Net.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.WebUtilities;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ItaPortal.Net.Api.Controllers
{
    public class BaseController : Controller
    {       
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private IConfiguration _configuration = null;
        protected IConfiguration Configuration
        {
            get
            {
                if(_configuration == null)
                {
                    _configuration = new Configuration()
                        .AddJsonFile("config.json");
                }
                return _configuration;
            }
        }
        protected UserManager<ApplicationUser> UserManager { get; private set; }
        protected ApplicationUserManager AppUserManager
            {
                get
                {
                    return _AppUserManager ?? (ApplicationUserManager) Request.HttpContext.ApplicationServices.GetService(typeof(ApplicationUserManager));
                }
            }

            public BaseController(UserManager<ApplicationUser> userManager)
            {
                UserManager = userManager;
            }

            protected ModelFactory TheModelFactory
            {
                get
                {
                    if (_modelFactory == null)
                    {
                        _modelFactory = new ModelFactory(this.AppUserManager);
                    }
                    return _modelFactory;
                }
            }

            protected IActionResult HttpOk()
            {
            return new HttpStatusCodeResult(StatusCodes.Status200OK);
            }
            
            protected IActionResult GetErrorResult(IdentityResult result)
            {
                if (result == null)
                {
                    return HttpNotFound();
                }

                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        // No ModelState errors are available to send, so just return an empty BadRequest.
                        return HttpBadRequest();
                    }

                    return HttpBadRequest(ModelState);
                }

                return null;
            }
        }
    
}
