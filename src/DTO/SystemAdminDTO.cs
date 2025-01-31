namespace FusionTech.src.DTO
{
    public class SystemAdminDTO : PersonDTO
    {
        public class SystemAdminSignUpDTO : PersonSignUpDTO
        {
            public required bool ManageStores { get; set; }
            public required bool ManageEmployees { get; set; }
            public required bool ManageGames { get; set; }
            public required bool ManageCustomers { get; set; }
            public required bool ManageSystemAdmins { get; set; }
        }

        public class SystemAdminReadDto : PersonReadDto
        {
            public bool ManageStores { get; private set; }
            public bool ManageEmployees { get; private set; }
            public bool ManageGames { get; private set; }
            public bool ManageCustomers { get; private set; }
            public required bool ManageSystemAdmins { get; set; }
        }

        public class SystemAdminListDto : PersonListDto
        {
            public List<SystemAdminReadDto> SystemAdmins { get; set; }
        }
    }
}
