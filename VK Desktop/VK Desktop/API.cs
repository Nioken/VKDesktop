using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace VK_Desktop
{
    public class API
    {
        public string last_response;
        public string token;
        public string first_name;
        public string last_name;
        public string user_id;
        public string avatar_url;
        private bool isAuth = false;
        public void OAuth(string Login, string Password)
        {
            try
            {
                using (var request = new HttpRequest())
                {
                    last_response = request.Get("https://oauth.vk.com/token?grant_type=password&client_id=2274003&client_secret=hHbZxrka2uZ6jB1inYsH&username=" + Login + "&password=" + Password).ToString();
                    dynamic json = JObject.Parse(last_response);
                    this.token = (string)json.access_token;
                    last_response = request.Get("https://api.vk.com/method/users.get?&fields=photo_max&access_token=" + token + "&v=5.120").ToString();
                    json = JObject.Parse(last_response);
                    this.first_name = json.response[0].first_name;
                    this.last_name = json.response[0].last_name;
                    this.user_id = (string)json.response[0].id;
                    this.avatar_url = json.response[0].photo_max;
                    isAuth = true;
                }
            }
            catch (HttpException ex)
            {
                last_response = ex.Message;
            }
        }
        public void OAuth(string Token)
        {
            try
            {
                using (var request = new HttpRequest())
                {
                    this.token = Token;
                    last_response = request.Get("https://api.vk.com/method/users.get?&fields=photo_max&access_token=" + token + "&v=5.120").ToString();
                    dynamic json = JObject.Parse(last_response);
                    this.first_name = json.response[0].first_name;
                    this.last_name = json.response[0].last_name;
                    this.user_id = "ID: " + (string)json.response[0].id;
                    this.avatar_url = json.response[0].photo_max;
                    isAuth = true;
                }
            }
            catch (HttpException ex)
            {
                last_response = ex.Message;
            }
        }
    }
}
