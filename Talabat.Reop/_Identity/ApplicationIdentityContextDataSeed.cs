﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Reop._Identity
{
	public static class ApplicationIdentityContextDataSeed
	{
		public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Youssef Raof",
					Email = "youssefroaf93@gmail.com",
					UserName = "youssef.raof",
					PhoneNumber = "01006103731",

				};

				await userManager.CreateAsync(user, "P@ssw0rd"); 
			}
		}

	}
}
