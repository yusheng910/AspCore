using prjToDoList.Models;

namespace prjToDoList.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel() { User = new tUser(); oldPassword = string.Empty; }
        public tUser User { get; set; }
        public int fUserId
        {
            get { return this.User.fUserId; }
            set { this.User.fUserId = value; }
        }

        public string fUserName
        {
            get { return this.User.fUserName; }
            set { this.User.fUserName = value; }
        }

        public string oldPassword { get; set; }

        public string fPassword
        {
            get { return this.User.fPassword; }
            set { this.User.fPassword = value; }
        }

        public string fEmail
        {
            get { return this.User.fEmail; }
            set { this.User.fEmail = value; }
        }

        public string? fMobile
        {
            get { return this.User.fMobile; }
            set { this.User.fMobile = value; }
        }
    }
}
