using Graduation_Project.Shared.Models;
using Graduation_Project.Shared.Models.Authentication;
using Graduation_Project.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol;
using M = Graduation_Project.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.API.Data;

namespace Graduation_Project.API.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
		private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly ApplicationDatabaseContext _context;
        private readonly EmailService emailService;


        [ActivatorUtilitiesConstructor]
		public AuthController(IAuthService authService , UserManager<User> userManager , EmailService emailService, UserService userService , ApplicationDatabaseContext context)
        {
            _authService = authService;
			_userManager = userManager;
			_userService = userService;
            _context = context;
            this.emailService = emailService;
        }







        //[HttpGet("getUserByname")]  //this action didn't exist in Services
        //public async Task<IActionResult> GetUserByUsername(string username)
        //{
        //	// Check if the user exists
        //	var user = await _userManager.FindByNameAsync(username);

        //	if (user == null)
        //	{
        //		return NotFound("User not found");
        //	}

        //	// You may want to customize the data you return based on your requirements
        //	var userData = new
        //	{
        //		UserId = user.Id,
        //		UserName = user.UserName,
        //		Email = user.Email,
        //		Token = user.ToJToken()
        //		// Add any other properties you want to expose
        //	};

        //	return Ok(userData);
        //}


        [HttpPost("login")]  //to get the token 
		public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.GetTokenAsync(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);
		}




        //[HttpPost("addrole")]
        //public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        //{
        //	if (!ModelState.IsValid)
        //		return BadRequest(ModelState);

        //	var result = await _authService.AddRoleAsync(model);

        //	if (!string.IsNullOrEmpty(result))
        //		return BadRequest(result);

        //	return Ok(model);
        //}








        //POST : /api/Auth/forgetPassword
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordEmail([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.EmailAddress);

            if (user != null)
            {
                const int otpLength = 6;
                const string allowedChars = "0123456789";

                Random random = new Random();
                char[] otp = new char[otpLength];

                for (int i = 0; i < otpLength; i++)
                {
                    otp[i] = allowedChars[random.Next(0, allowedChars.Length)];
                }
                string otpCode = new string(otp);

                var deleteUserOtp = _context.Users.FirstOrDefault(x => x.Id == user.Id);

                if (deleteUserOtp != null)
                {
                    // Clear the OTP and expiry date
                    deleteUserOtp.Otp = null;
                    deleteUserOtp.Expire = null;

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }

                await SaveOTPInDatabase(user.Id, otpCode);

                var email = new ForgetPasswdEmail()
                {
                    Subject = "Reset Your Password",
                    To = forgetPasswordDto.EmailAddress,
                    Body = $"Your OTP to reset the password is: {otpCode}"
                };

                // Use the SendEmail method with plain text message
                emailService.SendEmail(email.To, email.Subject, email.Body);

                return Ok("OTP sent successfully");
            }

            return BadRequest("Email is incorrect");
        }


        //POST : /api/Auth/verifyOTP
        [HttpPost]
        [Route("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] OTPVerificationDto verificationDto)
        {
            // Assuming verificationDto contains UserId and EnteredOTP
            string enteredOTP = verificationDto.EnteredOTP;

            var user = _context.Users.FirstOrDefault(u => u.Otp == enteredOTP);

            if (user != null && !string.IsNullOrEmpty(user.Otp) && user.Expire.HasValue && user.Expire > DateTime.Now)
            {
                // Check if the entered OTP matches the stored OTP
                if (enteredOTP.Equals(user.Otp, StringComparison.OrdinalIgnoreCase))
                {
                    return Ok("OTP verification successful");
                }
            }

            return BadRequest("Invalid or expired OTP");
        }


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {

            // Ensure model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input. Please check the provided data.");
            }




            var user = await _userManager.FindByEmailAsync(resetPasswordDto.EmailAddress);
            if (user == null)
            {
                return NotFound("User not found.");
            }



            bool isOTPValid = await VerifyOTP(resetPasswordDto.OTP);

            if (isOTPValid)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Reset the user's password
                var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    return Ok("Password reset successfully.");
                }
                else
                {
                    // If the password reset failed, return a generic message to avoid leaking information
                    return BadRequest("Password reset failed. Please try again.");
                }
            }

            return BadRequest("Invalid or expired OTP");

            // Generate a password reset token




        }



        private async Task SaveOTPInDatabase(Guid userId, string otpCode)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (existingUser != null)
                {
                    // Update the existing OTP record
                    existingUser.Otp = otpCode;
                    existingUser.Expire = DateTime.Now.AddMinutes(15); // Adjust expiry time as needed
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Log the issue
                    Console.WriteLine($"User not found for UserId: {userId}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for further analysis
                Console.WriteLine($"An error occurred while saving OTP: {ex.Message}");
            }
        }


        [NonAction] // Exclude this method from being treated as an action by Swagger
        public async Task<bool> VerifyOTP(string enteredOTP)
        {
            // Retrieve the user's profile

            var user = _context.Users.FirstOrDefault(u => u.Otp == enteredOTP);
            if (user != null)
            {
                // Check if the user has an OTP, and it's not expired
                if (!string.IsNullOrEmpty(user.Otp) && user.Expire.HasValue && user.Expire > DateTime.Now)
                {
                    // Check if the entered OTP matches the stored OTP
                    if (enteredOTP.Equals(user.Otp, StringComparison.OrdinalIgnoreCase))
                    {
                        // Clear the OTP and expiry date after successful verification
                        user.Otp = null;
                        user.Expire = null;

                        await _context.SaveChangesAsync();

                        return true; // OTP is valid
                    }
                }
            }

            return false; // OTP is invalid or expired, or the user/profile does not exist
        }







    }
}

   