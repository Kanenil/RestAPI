using ClientWPF.Commands;
using ClientWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private string? _email;
        public string? Email { get => _email; set => _email = value; }
        private string? _password;
        public string? Password { get => _password; set => _password = value; }
    }
}
