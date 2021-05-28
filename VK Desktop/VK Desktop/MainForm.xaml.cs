using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using xNet;

namespace VK_Desktop
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        API api;
        public MainForm(API api)
        {
            InitializeComponent();
            this.api = api;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = api.first_name + " " + api.last_name;
            this.Icon = new BitmapImage(new Uri(api.avatar_url));
            try
            {
                using (var request = new HttpRequest())
                {
                    api.last_response = request.Get("https://api.vk.com/method/users.get?&fields=photo_max,status,followers_count,bdate,country&access_token=" + api.token + "&v=5.120").ToString();
                    dynamic json = JObject.Parse(api.last_response);
                    NicknameBlock.Text = json.response[0].first_name + " " + json.response[0].last_name;
                    api.avatar_url = json.response[0].photo_max;
                    Avatar.Source = new BitmapImage(new Uri(api.avatar_url));
                    StatusBlock.Text = json.response[0].status;
                    FollowersCountText.Text = json.response[0].followers_count;
                    //DateTime bdate = DateTime.Parse(json.response[0].bdate);
                    Birthday.Text = "Дата рождения: " + json.response[0].bdate;
                    Country.Text = "Страна: " + json.response[0].country.title;
                    api.last_response = request.Get("https://api.vk.com/method/messages.getConversations?&offset=0&count=0&access_token=" + api.token + "&v=5.120").ToString();
                    json = JObject.Parse(api.last_response);
                    if(Convert.ToInt32(json.response.unread_count) > 0)
                    {
                        NewMessagesIndicator.Visibility = Visibility.Visible;
                        NewMessagesIndicator.Content = json.response.unread_count;
                    }
                    api.last_response = request.Get("https://api.vk.com/method/friends.get?&access_token=" + api.token + "&v=5.120").ToString();
                    json = JObject.Parse(api.last_response);
                    FriendsCountText.Text = json.response.count;

                }
                MyPageGrid.Visibility = Visibility.Visible;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            feedGrid.Visibility = Visibility.Hidden;
            MyPageGrid.Visibility = Visibility.Visible;
            try
            {
                using (var request = new HttpRequest())
                {
                    api.last_response = request.Get("https://api.vk.com/method/users.get?&fields=photo_max,status,followers_count,bdate,country&access_token=" + api.token + "&v=5.120").ToString();
                    dynamic json = JObject.Parse(api.last_response);
                    NicknameBlock.Text = json.response[0].first_name + " " + json.response[0].last_name;
                    api.avatar_url = json.response[0].photo_max;
                    Avatar.Source = new BitmapImage(new Uri(api.avatar_url));
                    StatusBlock.Text = json.response[0].status;
                    FollowersCountText.Text = json.response[0].followers_count;
                    //DateTime bdate = DateTime.Parse(json.response[0].bdate);
                    Birthday.Text = "Дата рождения: " + json.response[0].bdate;
                    Country.Text = "Страна: " + json.response[0].country.title;
                    api.last_response = request.Get("https://api.vk.com/method/messages.getConversations?&offset=0&count=0&access_token=" + api.token + "&v=5.120").ToString();
                    json = JObject.Parse(api.last_response);
                    if (Convert.ToInt32(json.response.unread_count) > 0)
                    {
                        NewMessagesIndicator.Visibility = Visibility.Visible;
                        NewMessagesIndicator.Content = json.response.unread_count;
                    }
                    api.last_response = request.Get("https://api.vk.com/method/friends.get?&access_token=" + api.token + "&v=5.120").ToString();
                    json = JObject.Parse(api.last_response);
                    FriendsCountText.Text = json.response.count;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            feedGrid.Visibility = Visibility.Visible;
            MyPageGrid.Visibility = Visibility.Hidden;
            //newsfeed.get
            try
            {
                using (var request = new HttpRequest())
                {
                    api.last_response = request.Get("https://api.vk.com/method/newsfeed.get?&filters=post&count=40&access_token=" + api.token + "&v=5.120").ToString();
                    dynamic json = JObject.Parse(api.last_response);
                    for (int k = 0; k < json.response.items.Count; k++)
                    {
                        string source = json.response.items[k].source_id;
                        if (source[0] == '-')
                        {
                            source = source.Replace("-", "");
                            int i = 0;
                            while (i >= 0)
                            {
                                if (json.response.groups[i].id == source)
                                {
                                    DateTime date = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(json.response.items[k].date));
                                    NewsClass news = new NewsClass(json.response.groups[i].name.ToString(), date.ToLongDateString(), json.response.groups[i].photo_50.ToString(), json.response.items[k].text.ToString());
                                    int count = json.response.items[k].attachments.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (json.response.items[k].attachments[j].type == "photo")
                                        {
                                            news.Images[j] = json.response.items[k].attachments[j].photo.sizes[json.response.items[k].attachments[j].photo.sizes.Count-1].url;
                                        }
                                    }
                                    NewsList.Items.Add(news);
                                    break;
                                }
                                i++;
                            }
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         }

        private void NewsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NewsClass test = NewsList.SelectedItem as NewsClass;
            MessageBox.Show(test.TextContent);
        }
    }
}
