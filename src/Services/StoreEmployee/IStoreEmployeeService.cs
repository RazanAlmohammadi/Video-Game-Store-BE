using static FusionTech.src.DTO.CustomerDTO;
using static FusionTech.src.DTO.StoreEmployeeDTO;

namespace FusionTech.Service.StoreEmployee
{
    public interface IStoreEmployeeService
    {
        Task<StoreEmployeeReadDto> SignUpEmployee(StoreEmployeeCreateDto createDto);
        Task<CustomerReadDto> SignInAsCustomer(StoreEmployeeReadDto readDto);
        Task ViewSalary(StoreEmployeeReadDto readDto);
    }
}