using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User.Management.API.Models;
using User.Management.API.Models.Authentication.SignUp;

namespace User.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager; // Manages users
        private readonly RoleManager<IdentityRole> _roleManager; // Manages roles
		private readonly IConfiguration _configuration;

        // Constructor to initialize all instance variables
        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Registering a new user on the page
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
			// registerUser value is taken from the body of the request on the page

			// Check user exists
			var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)     // There is already a user is the database with this email
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists." });
            }

			// If no user exists, create a new user object
			IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName
            };

            // Check that the role entered in the text field is one of the existing roles in the database
            if (await _roleManager.RoleExistsAsync(role))
            {
				// Add the user object to the database with the password entered
				var result = await _userManager.CreateAsync(user, registerUser.Password);

                // Check if an error occurred
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "User failed to create." });
                }

                // Add user to the role
                await _userManager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "User created successfully." });
            }
            else
            {
				// The role entered in the text field isn't one of the existing roles
				return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "This role doesn't exist." });
            }
        }

    }
}
