﻿namespace Talabat.Core.Entities.Identity
{
	public class Address:BaseEntity
	{

		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;
        public string Counrty { get; set; } = null!;

		public ApplicationUser User { get; set; }= null!; // Navigational Propery [ONE]

        public string ApplicationUserId { get; set; } // Forgein Key

    }
}