namespace FusionTech.src.DTO
{
    public class PersonDTO
    {
        public class PersonSignUpDTO
        {
            [Required(ErrorMessage = "Person name is required.")]
            [StringLength(100, ErrorMessage = "Person name cannot exceed 100 characters.")]
            public required string PersonName { get; set; }

            [Required(ErrorMessage = "Person email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public required string PersonEmail { get; set; }

            [Required(ErrorMessage = "Person password is required.")]
            //[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and cannot exceed 100 characters.")]
            [PasswordComplexity]
            public required string PersonPassword { get; set; }

            [Phone(ErrorMessage = "Invalid phone number format.")]
            public string? PersonPhoneNumber { get; set; }
        }

        public class PersonSignInDTO
        {
            [Required(ErrorMessage = "Person email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public required string PersonEmail { get; set; }

            [Required(ErrorMessage = "Person password is required.")]
            public required string PersonPassword { get; set; }
        }

        public class PersonReadDto
        {
            public int PersonId { get; set; }

            public required string PersonName { get; set; }

            public required string PersonEmail { get; set; }

            public string? PersonPhoneNumber { get; set; }

            public string? ProfilePicturePath { get; set; }
        }

        public class PersonListDto
        {
            public int TotalCount { get; set; }
        }
        public class UpdatePasswordDTO
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        
    }
}
