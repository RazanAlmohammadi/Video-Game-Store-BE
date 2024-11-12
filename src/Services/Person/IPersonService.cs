namespace FusionTech.src.Services.Person
{
    public interface IPersonService
    {
        Task<PersonReadDto> GetByIdAsync(int id);
        Task<string> SignInAsync(PersonSignInDTO personSignInDTO);
        Task<bool> UpdateNameAsync(string email, string name);

        Task<bool> EditProfilePicture(string email, string picturePath);
        Task<bool> EditPassword(string email, string oldPassword, string newPassword);
        Task<bool> EditPhone(string email, string newPhone);
        Task<int> GetIdByEmailAsync(string email);
        
    }
}
