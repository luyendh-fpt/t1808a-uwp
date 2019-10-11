using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using T1808AHelloUWP.Entity;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1808AHelloUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private string _gender = "Gender";
        private const string REGISTER_URL = "https://2-dot-backup-server-003.appspot.com/_api/v2/members";
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var birthdaySelectedDate = this.Birthday.SelectedDate;
            if (birthdaySelectedDate == null)
            {
                birthdaySelectedDate = DateTime.Now;
            }
            var birthday = birthdaySelectedDate.Value.ToString("yyyy-MM-dd");

            var member = new Member
            {
                firstName = this.FirstName.Text,
                lastName = this.LastName.Text,
                avatar = this.Avatar.Text,
                phone = this.Phone.Text,
                address = this.Address.Text,
                introduction = this.Introduction.Text,
                email = this.Email.Text,
                gender = _gender.Equals("Male") ? 1 : (_gender.Equals("Female") ? 0 : 2),
                birthday = birthday,
                password = this.Password.Password
            };
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(member), Encoding.UTF8, "application/json");
            var response =  httpClient.PostAsync(REGISTER_URL, content).Result.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(response);
        }

        private void RadionButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton checkedRadio)
            {
                this._gender = checkedRadio.Tag.ToString();
            }
        }
    }
}
