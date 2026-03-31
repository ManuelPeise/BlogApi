namespace Shared.Models
{
    public class ChangePasswordRequestModel
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string UpdatedPassword { get; set; } = string.Empty;
    }
}
