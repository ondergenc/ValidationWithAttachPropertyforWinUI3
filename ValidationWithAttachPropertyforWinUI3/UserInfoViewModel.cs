using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ValidationWithAttachPropertyforWinUI3.Attributes;

namespace ValidationWithAttachPropertyforWinUI3
{
    public class UserInfoViewModel : ObservableValidator
    {
        private string _name = string.Empty;
        private string _email = string.Empty;
        public UserInfoViewModel()
        {
        }

        //[Required(ErrorMessage = "Name is Required")]
        //[MinLength(4, ErrorMessage = "Name should be longer than one character")]
        [NameValidation(5)]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, true);
            }
        }

        //[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value, true);
            }
        }
    }
}
