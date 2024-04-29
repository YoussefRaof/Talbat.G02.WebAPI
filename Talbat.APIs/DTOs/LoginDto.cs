using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
        public string Email { get; set; } = null!;

		//[DataType(DataType.Password)] // Dont Need To Use It Because I Dont Have View
		[Required]
        public string Password { get; set; } = null!;
    }
}
