using ClientWPF.DTO;
using ClientWPF.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientWPF.ViewModels
{
    public class UserViewModel : ViewModel
    {
        private readonly string url = "https://bv012.novakvova.com";
        private List<UserDTO> _users;
        public List<UserDTO> Users { get => _users; set { _users = value; OnPropertyChanged(); } }
        public UserViewModel()
        {
            UpdateUsers();
        }
        private void UpdateUsers()
        {
            WebRequest request = WebRequest.Create($"{url}/api/account/users");
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                var response = request.GetResponse();
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string data = stream.ReadToEnd();
                    Users = null;
                    var users = JsonConvert.DeserializeObject<List<UserDTO>>(data);
                    foreach (var user in users)
                    {
                        user.Photo = ConvertImageURLToBase64($"{url}{user.Photo}");
                    }
                    Users = users;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = this.GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }
        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }
    }
}
