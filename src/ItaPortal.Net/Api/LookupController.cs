using ItaPortal.Net.Data;
using ItaPortal.Net.Models.Identity;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ItaPortal.Api.Controllers
{

    [Route("api/[controller]")]    
    public class LookupController : Controller
    {
        private ILookupRepository _repository = null;

        public LookupController(ILookupRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [Route("securityquestions")]
        public async Task<IEnumerable<SecurityQuestion>> GetSecurityQuestions()
        {
            return await _repository.GetSecurityQuestions();

        }
    }
}
