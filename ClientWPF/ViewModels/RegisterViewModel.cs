using ClientWPF.Commands;
using ClientWPF.DTO;
using ClientWPF.ViewModels.Base;
using Microsoft.Win32;
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

namespace ClientWPF.ViewModels
{
    public class RegisterViewModel : ViewModel
    {
        private string? _firstName;
        public string? FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        private string? _secondName;
        public string? SecondName { get => _secondName; set { _secondName = value; OnPropertyChanged(); } }
        private string? _email;
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        private string? _phone;
        public string? Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        private string? _photo;
        public string? Photo { get => _photo; set { _photo = value; OnPropertyChanged(); } }
        private SecureString _password;
        public SecureString Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private SecureString _confirmPassword;
        public SecureString ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); } }
        private string? _message;
        public string? Message { get => _message != null ? "* " + _message : ""; set { _message = value; OnPropertyChanged(); } }
        private LambdaCommand _register;
        public LambdaCommand Register
        {
            get => _register ?? new LambdaCommand((w) =>
            {
                NetworkCredential network1 = new NetworkCredential(Email, Password);
                NetworkCredential network2 = new NetworkCredential(Email, ConfirmPassword);
                if(RegisterUser(new RegisterUserDTO()
                {
                    FirstName = FirstName,
                    SecondName = SecondName,
                    ConfirmPassword = network2.Password,
                    Password = network1.Password,
                    Email = Email,
                    Phone = Phone,
                    Photo = ImageToBase64(Photo)
                }))
                {
                    (w as Window).Close();
                }

            }, (o) => {
                NetworkCredential network1 = new NetworkCredential(Email, Password);
                NetworkCredential network2 = new NetworkCredential(Email, ConfirmPassword);
                return !string.IsNullOrWhiteSpace(network1.UserName) ||
                       !string.IsNullOrWhiteSpace(network1.Password) ||
                       !string.IsNullOrWhiteSpace(network2.Password) &&
                       string.IsNullOrWhiteSpace(network2.Password) == string.IsNullOrWhiteSpace(network1.Password) ||
                       !string.IsNullOrWhiteSpace(FirstName) ||
                       !string.IsNullOrWhiteSpace(SecondName) ||
                       !string.IsNullOrWhiteSpace(Photo) ||
                       !string.IsNullOrWhiteSpace(Phone);
            });
        }
        private LambdaCommand _loadPhoto;
        public LambdaCommand LoadPhoto
        {
            get => _loadPhoto ?? new LambdaCommand((w) =>
            {
                OpenFileDialog res = new OpenFileDialog();
                res.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

                if (res.ShowDialog() != false)
                    Photo = res.FileName;
            });
        }
        private LambdaCommand _cancel;
        public LambdaCommand Cancel
        {
            get => _cancel ?? new LambdaCommand((w) =>
            {
                (w as Window).Close();
            }, (o) => true);
        }

        private bool RegisterUser(RegisterUserDTO registerUser)
        {
            string url = "https://bv012.novakvova.com";
            string json = JsonConvert.SerializeObject(registerUser);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            WebRequest request = WebRequest.Create($"{url}/api/account/register");
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
            catch (Exception ex) { Message = "something went wrong!"; }
            return false;
        }
        private static string ImageToBase64(string path)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    img.Save(m, img.RawFormat);
                    byte[] bytes = m.ToArray();
                    return Convert.ToBase64String(bytes);
                }
            }
        }
    }
}
