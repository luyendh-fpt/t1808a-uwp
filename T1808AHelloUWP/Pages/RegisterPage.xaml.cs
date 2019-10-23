using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using T1808AHelloUWP.Entity;
using StorageFile = Windows.Storage.StorageFile;

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
        private const string GET_UPLOAD_URL = "https://2-dot-backup-server-003.appspot.com/get-upload-token";
        private StorageFile photo;
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //CaptureImage();
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
                avatar = AvatarUrl.Text,
                phone = this.Phone.Text,
                address = this.Address.Text,
                introduction = this.Introduction.Text,
                email = this.Email.Text,
                gender = _gender.Equals("Male") ? 1 : (_gender.Equals("Female") ? 0 : 2),
                birthday = birthday,
                password = this.Password.Password
            };
            // tạo đối tượng httpclient giúp gửi dữ liệu đi. (hoặc lấy dữ liệu về)
            var httpClient = new HttpClient();
            // chuyển kiểu dữ liệu c# thành kiểu dữ liệu json.
            var dataToSend = JsonConvert.SerializeObject(member);
            // gói gém, gắn mác cho dữ liệu gửi đi, xác định kiểu dữ liệu là json, encode là utf8.
            var content = new StringContent(dataToSend, Encoding.UTF8, "application/json");
            // thực hiện gửi dữ liệu với phương thức post.
            var response = httpClient.PostAsync(REGISTER_URL, content).GetAwaiter().GetResult();
            // lấy kết quả trả về từ server.
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            // ép kiểu kết quả từ dữ liệu json sang dữ liệu của C#
            var responseMember = JsonConvert.DeserializeObject<Member>(jsonContent);
            // in ra id của member trả về.
            Debug.WriteLine("Register success with id: " + responseMember.id);
        }

        private async void ProcessCaptureImage()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            this.photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (this.photo == null)
            {
                // User cancelled photo capture
                return;
            }

            string uploadUrl = GetUploadUrl();
            HttpUploadFile(uploadUrl, "myFile", "image/jpeg");
            //StorageFolder destinationFolder =
            //    await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder",
            //        CreationCollisionOption.OpenIfExists);

            //await photo.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
            //await photo.DeleteAsync();

        }

        public async void HttpUploadFile(string url, string paramName, string contentType)        {            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);            wr.ContentType = "multipart/form-data; boundary=" + boundary;            wr.Method = "POST";            Stream rs = await wr.GetRequestStreamAsync();            rs.Write(boundarybytes, 0, boundarybytes.Length);            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await this.photo.OpenStreamForReadAsync();            byte[] buffer = new byte[4096];            int bytesRead = 0;            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)            {                rs.Write(buffer, 0, bytesRead);            }            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");            rs.Write(trailer, 0, trailer.Length);            WebResponse wresp = null;            try            {                wresp = await wr.GetResponseAsync();                Stream stream2 = wresp.GetResponseStream();                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //string imgUrl = reader2.ReadToEnd();
                //Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                //Debug.WriteLine(u.AbsoluteUri);
                //ImageUrl.Text = u.AbsoluteUri;
                //MyAvatar.Source = new BitmapImage(u);
                //Debug.WriteLine(reader2.ReadToEnd());
                string imageUrl = reader2.ReadToEnd();                Debug.WriteLine("Image url:" + imageUrl);                Avatar.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));                AvatarUrl.Text = imageUrl;            }            catch (Exception ex)            {                Debug.WriteLine("Error uploading file", ex.StackTrace);                Debug.WriteLine("Error uploading file", ex.InnerException);                if (wresp != null)                {                    wresp = null;                }            }            finally            {                wr = null;            }        }

        private string GetUploadUrl()
        {
            var httpClient = new HttpClient();
            // thực hiện gửi dữ liệu với phương thức post.
            var response = httpClient.GetAsync(GET_UPLOAD_URL).GetAwaiter().GetResult();
            // lấy kết quả trả về từ server.
            var uploadUrl = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("Upload url: " + uploadUrl);
            return uploadUrl;
        }


        private void RadionButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton checkedRadio)
            {
                this._gender = checkedRadio.Tag.ToString();
            }
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void CapturePhoto(object sender, RoutedEventArgs e)
        {
            ProcessCaptureImage();
        }
    }
}
