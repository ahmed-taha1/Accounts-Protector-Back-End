using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoGetPlatformRequest
    {
        [Required(ErrorMessage = "id is required")]
        public int Id { get; set; }
    }

    public class DtoCreatePlatformRequest
    {
        [Required(ErrorMessage = "platform name is required")]
        public string? PlatformName { get; set; }
        [Required(ErrorMessage = "icon color is required")]
        public string? IconColor { get; set; }
    }

    public class DtoCreatePlatformResponse
    {
        public int? PlatformId { get; set; }
    }

    public class DtoDeletePlatformRequest
    {
        [Required(ErrorMessage = "id is required")]
        public int Id { get; set; }
    }

    public class DtoGetAllPlatformsResponse
    {
        public List<DtoPlatform>? Platforms { get; set; }
    }

    public class DtoPlatform
    {
        public int PlatformId { get; set; }
        public string? PlatformName { get; set; }
        public string? IconColor { get; set; }
        public int? NumOfAccounts { get; set; }
    }

    public class DtoGetAllPlatformsWithAccountsResponse
    {
        public List<DtoPlatformWithAccounts>? Platforms { get; set; }
    }

    public class DtoPlatformWithAccounts : DtoPlatform
    {
        public ICollection<DtoAccount?>? Accounts { get; set; }
    }

    public class DtoUpdatePlatformRequest
    {
        [Required(ErrorMessage = "id is required")]
        public int Id { get; set; }
        public string? PlatformName { get; set; }
        public string? IconColor { get; set; }
    }
}