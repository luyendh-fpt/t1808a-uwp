using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using T1808AHelloUWP.Entity;
using T1808AHelloUWP.Service;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1808AHelloUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListSongPage : Page
    {
        ObservableCollection<Song> ListSong;
        private ISongService _songService;
        public ListSongPage()
        {
            this.ListSong = new ObservableCollection<Song>();
            this.ListSong.Add(new Song()
            {
                name = "Chưa bao giờ",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://file.tinnhac.com/resize/600x-/music/2017/07/04/19554480101556946929-b89c.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/ChuaBaoGioSEESINGSHARE2-HaAnhTuan-5111026.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Tình thôi xót xa",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/XyjhXzsVdiI/maxresdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/TinhThoiXotXaSEESINGSHARE1-HaAnhTuan-4652191.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Tháng tư là tháng nói dối của em",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://sky.vn/wp-content/uploads/2018/05/0-30.jpg",
                link = "https://od.lk/s/NjFfMjM4MzQ1OThf/ThangTuLaLoiNoiDoiCuaEm-HaAnhTuan-4609544.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Nơi ấy bình yên",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/A8u_fOetSQc/hqdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui946/NoiAyBinhYenSeeSingShare2-HaAnhTuan-5085337.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Giấc mơ chỉ là giấc mơ",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/J_VuNwxSEi0/maxresdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui945/GiacMoChiLaGiacMoSeeSingShare2-HaAnhTuan-5082049.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Người tình mùa đông",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/EXAmxBxpZEM/maxresdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/NguoiTinhMuaDongSEESINGSHARE2-HaAnhTuan-5104816.mp3"
            }); ;
            this.InitializeComponent();
        }

        private void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var playIcon = sender as SymbolIcon;
            var currentSong = playIcon.Tag as Song;
            Debug.WriteLine(currentSong.name);
            MyMediaElement.Source  = new Uri(currentSong.link);
            MyMediaElement.Play();
        }
    }
}
