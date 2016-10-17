using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NotificationHubForDQD {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        private int RetryNumber = 0;

        public MainPage() {
            this.InitializeComponent();
            Title.Text = "DQD UWP 新版本通知";
            Content.Text = "最近版本：1.1004.2204已提交应用商店，修复部分问题，增加改变新闻字体大小和节省流量的功能，请注意更新";
        }

        private async void Submit_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(Title.Text) || string.IsNullOrEmpty(Content.Text))
                return;
            await SendToastNotificationAsync(Title.Text, Content.Text);
        }

        private void CheckToggle_Toggled(object sender, RoutedEventArgs e) {
            Submit.IsEnabled = (sender as ToggleSwitch).IsOn;
        }

        public async Task SendToastNotificationAsync(string title, string content) {
            using (HttpClient client = new HttpClient()) {
                string url = "http://notificationhubforuwp.azurewebsites.net/Home/SendToastForDQD?title={0}&toastContent={1}";
                await client.GetAsync(string.Format(url,title,content));
                Debug.WriteLine("{0} > Sending message: {1} +\n {2}", DateTime.Now, title, content);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(Password.Password)) { ErrorText.Text = "Error! Null Password is Invalid!"; return; }
            if (Password.Password.Equals("BOSS.2w3e4r5t"))
                PassBorder.Visibility = Visibility.Collapsed;
            else {
                if (RetryNumber < 3) {
                    ErrorText.Text = string.Format("Error! Wrong Password. Still {0} times to retry!", (3-RetryNumber).ToString());
                    RetryNumber++;
                } else 
                    Application.Current.Exit();
            }
        }
    }
}
