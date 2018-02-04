using System.ComponentModel.DataAnnotations;

namespace WebTorrent.Web.ViewModels
{
    public class UserEditViewModel : UserViewModel
    {
        public string CurrentPassword { get; set; }

        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string NewPassword { get; set; }

        private new bool IsLockedOut { get; } //Hide base member
    }
}