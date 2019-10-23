using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using T1808AHelloUWP.Entity;
using T1808AHelloUWP.Pages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1808AHelloUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Windows.UI.Xaml.Controls.Page
    {

        private const string REGISTER_URL =
            "https://2-dot-backup-server-003.appspot.com/_api/v2/members/authentication";

        public LoginPage()
        {
            this.InitializeComponent();
        }



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var memberLogin = new MemberLogin
            {
                email = Email.Text,
                password = Password.Password
            };
            var httpClient = new HttpClient();
            var dataToSend = JsonConvert.SerializeObject(memberLogin);
            var content = new StringContent(dataToSend,Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(REGISTER_URL, content).GetAwaiter().GetResult();
            var memberCredential = JsonConvert.DeserializeObject<MemberCredential>(response.Content.ReadAsStringAsync().Result);
            //Debug.WriteLine(memberCredential.token);
            SaveTokenToFile(memberCredential);
        }

        private async void SaveTokenToFile(MemberCredential memberCredential)
        {
            var storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("SavedFile",
                  CreationCollisionOption.OpenIfExists);
            var storageFile =
                await storageFolder.CreateFileAsync("token.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(storageFile, JsonConvert.SerializeObject(memberCredential));
        }
    }
}