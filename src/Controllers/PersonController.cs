namespace FusionTech.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonController : ControllerBase
    {
        protected readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        // Endpoint for SignIn
        [HttpPost("signIn")]
        public async Task<ActionResult<string>> SignInPerson(
            [FromBody] PersonSignInDTO personSignInDTO
        )
        {
            var token = await _personService.SignInAsync(personSignInDTO);
            return Ok(token); // 200
        }

        [HttpPut("updateName")]
        public async Task<IActionResult> UpdateName([FromQuery] string name)
        {
            string? userEmail = User.FindFirstValue(ClaimTypes.Email);
            bool result = await _personService.UpdateNameAsync(userEmail, name);
            if (result)
                return Ok(new { message = "Name updated successfully." });
            return BadRequest(new { message = "Failed to update name." });
        }

        [HttpPut("updatePhone")]
        public async Task<IActionResult> UpdatePhone([FromQuery] string newPhone)
        {
            string? userEmail = User.FindFirstValue(ClaimTypes.Email);
            bool result = await _personService.EditPhone(userEmail, newPhone);
            if (result)
                return Ok(new { message = "Phone number updated successfully." });
            return BadRequest(new { message = "Failed to update phone number." });
        }

        [HttpPut("updateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromQuery] string picturePath)
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            bool result = await _personService.EditProfilePicture(userEmail, picturePath);
            if (result)
                return Ok(new { message = "Profile picture updated successfully." });
            return BadRequest(new { message = "Failed to update profile picture." });
        }

        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword(
            [FromBody] UpdatePasswordDTO updatePasswordDto
        )
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            bool result = await _personService.EditPassword(
                userEmail,
                updatePasswordDto.OldPassword,
                updatePasswordDto.NewPassword
            );
            if (result)
                return Ok(new { message = "Password updated successfully." });
            return BadRequest(new { message = "Failed to update password." });
        }
        
    }
}
