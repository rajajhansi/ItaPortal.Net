using ItaPortal.Net.Models.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItaPortal.Net.Data
{
    public class SeedData
    {
        private readonly int MaxUserNameCollisionsAllowed = 10;
        private readonly string DefaultPassword = "P@ssw0rd1";

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Seed()
        {
            //if(_context.Database.EnsureCreated())
            {
                SeedUsers().Wait();
                SeedSecurityQuestions().Wait();
            }
        }

        private string ComputeUserName(string firstName, string lastName, string email)
        {
            int uniqueUserNameSuffix = 1;
            var userName = string.Concat(firstName.Substring(0, 1), lastName);
            var newUserName = userName;
            ApplicationUser foundUser = null;
            if (((foundUser = _userManager.FindByNameAsync(newUserName).Result) != null) &&
                (foundUser.Email != email))
            {
                do
                {
                    newUserName = string.Concat(userName, Convert.ToString(uniqueUserNameSuffix));
                    ++uniqueUserNameSuffix;
                } while (((foundUser = _userManager.FindByNameAsync(newUserName).Result) != null) &&
                          (foundUser.Email != email) && 
                          (uniqueUserNameSuffix <= MaxUserNameCollisionsAllowed));
            }
            return (uniqueUserNameSuffix <= MaxUserNameCollisionsAllowed) ? newUserName : string.Empty;
        }
        private async Task SeedUsers()
        {
            // Add some default users
            var users = new List<ApplicationUser>
                        {
                            new ApplicationUser { FirstName = "Raja", LastName = "Mani", UserName = "rmani", Email = "myraja@hotmail.com", EmailConfirmed = true, Level = 1, JoinDate = new DateTime(2012, 12, 1)},
                            new ApplicationUser { FirstName = "Jhansirani", LastName = "Sarvamohan", Email = "jrani@hotmail.com", EmailConfirmed = true, Level = 1, JoinDate = new DateTime(2012, 12, 10)}
                        };
            users.ForEach(async user => {
                var foundUser = await _userManager.FindByEmailAsync(user.Email);
                if (foundUser == null)
                {
                    user.UserName = ComputeUserName(user.FirstName, user.LastName, user.Email);
                    user.NormalizedUserName = user.UserName.ToUpperInvariant();
                    user.NormalizedEmail = user.Email.ToUpperInvariant();                    
                    await _userManager.CreateAsync(user, DefaultPassword);
                    await _userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
                }
            });
            await Task.FromResult(true);
        }

        private async Task SeedSecurityQuestions()
        {
            var securityQuestions = new List<SecurityQuestion>
            {
                new SecurityQuestion { Name = "In what city or town was your first job?" },
                new SecurityQuestion { Name = "In what city was your father born? (enter full name of city only)" },
                new SecurityQuestion { Name = "What color was your first car?" },
                new SecurityQuestion { Name = "What is the first name of your favorite teacher?" },
                new SecurityQuestion { Name = "What is the last name of the best man from your wedding?" },
                new SecurityQuestion { Name = "What is the name of the first school you attended?" },
                new SecurityQuestion { Name = "What is the name of the hospital where your first child was born?" },
                new SecurityQuestion { Name = "What is the name of your Father's Father? (enter first name only)" },
                new SecurityQuestion { Name = "What is the name of your favorite childhood friend?" },
                new SecurityQuestion { Name = "What is your oldest sibling's middle name?" },
                new SecurityQuestion { Name = "What is your paternal grandmother's first name?" },
                new SecurityQuestion { Name = "What is your spouse's/significant other's father's first name?" },
                new SecurityQuestion { Name = "What is/was your mother's last occupation?" },
                new SecurityQuestion { Name = "What was the name of your favorite pet?" },
                new SecurityQuestion { Name = "What was the name/location of your favorite vacation spot?" },
                new SecurityQuestion { Name = "What was your dream job as a child?" },
                new SecurityQuestion { Name = "What was your favorite childhood television show?" },
                new SecurityQuestion { Name = "What year did you graduate from High School?" },
                new SecurityQuestion { Name = "Where did you and your spouse/significant other go on your honeymoon?"},
                new SecurityQuestion { Name = "Who was your childhood hero?" }
            };

            await _context.SecurityQuestions.AddAsync(securityQuestions.ToArray());
            await _context.SaveChangesAsync();           
        }
    }
}