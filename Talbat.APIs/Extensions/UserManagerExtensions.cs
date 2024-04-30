using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
	public static class UserManagerExtensions
	{
		public static async Task<ApplicationUser?> FindUserWithAddress(this UserManager<ApplicationUser> userManager , ClaimsPrincipal User)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(U => U.Addrees).FirstOrDefaultAsync(U => U.NormalizedEmail == email.ToUpper());
			return user;
		}
	}
}
