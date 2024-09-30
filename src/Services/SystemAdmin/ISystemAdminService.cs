using static FusionTech.src.DTO.CustomerDTO;
using static FusionTech.src.DTO.SystemAdminDTO;

namespace FusionTech.Service.SystemAdmin
{
    public interface ISystemAdminService
    {
        Task<SystemAdminReadDto> SignUpAdmin(SystemAdminCreateDto createDto);
        Task<CustomerReadDto> SignInAsCustomer(SystemAdminCreateDto readDto);

        // We need to add GameReadDto here
        Task<bool> AddGame();
        Task<bool> DeleteGame();
    }
}