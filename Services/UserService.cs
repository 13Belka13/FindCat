using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindKočka.Data;
using FindKočka.Controllers;
using System.Security.Claims;



namespace FindKočka.Services
{
    public class UserService : IUserService
    {
        private readonly FindKočkaContext _context;


        public UserService(FindKočkaContext context)
        {
            _context = context;
        }

        public int? GetUserId(ClaimsPrincipal User)
        {
            if (User.Identity.Name != null)
            {
                return _context.Owners.FirstOrDefault(u => u.Number == User.Identity.Name | u.Email == User.Identity.Name).Id;
            }

            else
            {
                return null;
            }
        }
    }

}
