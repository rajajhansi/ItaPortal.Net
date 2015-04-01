using ItaPortal.Net.Models.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaPortal.Net.Data
{
    public interface ILookupRepository
    {
        Task<IEnumerable<SecurityQuestion>> GetSecurityQuestions();
    }
}