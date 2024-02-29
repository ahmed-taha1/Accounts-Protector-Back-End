using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoAccount
    {
        [Required(ErrorMessage = "Account name is required")]
        public String? AccountName { get; set; }
        [Required(ErrorMessage = "Platform id is required")]
        public int? PlatformId { get; set; }
        public int? AccountId { get; set; }
        public Dictionary<String, String>? AccountFields { get; set; }
    }


    public class DtoCreateAccountRequest
    {
        [Required(ErrorMessage = "Account name is required")]
        public String? AccountName { get; set; }
        [Required(ErrorMessage = "Platform id is required")]
        public int? PlatformId { get; set; }
        public Dictionary<String, String>? AccountFields { get; set; }
    }

    public class DtoCreateAccountResponse
    {
        public int? AccountId { get; set; }
    }

    public class DtoGetAccountsByPlatformIdRequest
    {
        [Required(ErrorMessage = "Platform id is required")]
        public int? PlatformId { get; set; }
    }

    public class DtoGetAccountsByPlatformIdResponse
    {
        public List<DtoAccount>? Accounts { get; set; }
    }

}
