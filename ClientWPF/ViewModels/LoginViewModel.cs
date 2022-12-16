using ClientWPF.Commands;
using ClientWPF.DTO;
using ClientWPF.ViewModels.Base;
using ClientWPF.Views;
using ClientWPF.Views.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private string? _email;
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        private SecureString _password;
        public SecureString Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string? _message;
        public string? Message { get => _message != null ? "* " + _message : ""; set { _message = value; OnPropertyChanged(); } }
        private LambdaCommand _login;
        public LambdaCommand Login
        {
            get => _login??new LambdaCommand((w) =>
            {
                NetworkCredential network = new NetworkCredential(Email, Password);
                if(LoginUser(new LoginUserDTO()
                {
                    Email = network.UserName,
                    Password = network.Password
                }))
                {
                    Message = null;
                    (w as Window).Hide();

                    var usersWindow = new UsersWindow(new UserViewModel());
                    usersWindow.Show();
                    usersWindow.Closed += (sender, args) => { (w as Window).Show(); };
                }
            }, (o) =>
            {
                NetworkCredential network = new NetworkCredential(Email, Password);
                return !string.IsNullOrWhiteSpace(network.UserName) || !string.IsNullOrWhiteSpace(network.Password);
            });
        }
        private LambdaCommand _register;
        public LambdaCommand Register
        {
            get => _register ?? new LambdaCommand((w) =>
            {
                Message = null;
                (w as Window).Hide();

                var registerWindow = new RegisterWindow(new RegisterViewModel());
                registerWindow.Show();
                registerWindow.Closed += (sender, args) => { (w as Window).Show(); };
            }, (o) => true);
        }
        private bool LoginUser(LoginUserDTO registerUser)
        {
            string url = "https://bv012.novakvova.com";
            string json = JsonConvert.SerializeObject(registerUser);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            WebRequest request = WebRequest.Create($"{url}/api/account/login");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            try
            {
                var response = request.GetResponse();
                return true;
            }
            catch
            {
                Message = "invalid password or email!";
            }
            return false;
        }
    }
}
