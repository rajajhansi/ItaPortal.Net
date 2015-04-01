using ItaPortal.Net.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ItaPortal.Net.Data
{
    public class LookupRepository : ILookupRepository
    {
        private ApplicationDbContext _context;

        public LookupRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SecurityQuestion>> GetSecurityQuestions()
        {
            return await (from securityQuestion in _context.SecurityQuestions
                                    select securityQuestion).ToListAsync<SecurityQuestion>();            
        }
    }
}