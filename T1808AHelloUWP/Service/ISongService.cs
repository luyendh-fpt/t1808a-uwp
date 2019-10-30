using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1808AHelloUWP.Entity;

namespace T1808AHelloUWP.Service
{
    interface ISongService
    {
        Song CreateSong(Song song);
        List<Song> GetAllSong();
        List<Song> GetMineSongs();
    }
}
