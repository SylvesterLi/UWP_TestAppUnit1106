using System;
using System.Collections.Generic;
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
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using static UWP_TestAppUnit1106.Music0;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_TestAppUnit1106
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //定义泛型集合（音乐信息）
        List<music> obj;
        public MainPage()
        {
            this.InitializeComponent();
        }
        //获取音乐信息
        private async void Get_Music(object sender, RoutedEventArgs e)
        {
            var apiUrl_GetJson = string.Format("http://api.itwusun.com/music/search/qq/1?format=json&sign=a5cc0a8797539d3a1a4f7aeca5b695b9&keyword={0}", this.tbox_musicName.Text);
            //开始准备访问api服务器 实例化client
            HttpClient client = new HttpClient();
            //把自己本地整理好的数据发送给api服务器
            var json = await client.GetStringAsync(new Uri(apiUrl_GetJson));
            //判断是否有json返回
            if (json == null)
            {
                return;
            }
            obj = FromJsonTo<List<music>>(json);//耳朵提供的方法
            tb_Paste();
        }
        public void tb_Paste()
        {
            tb_songName.Text = obj[0].SongName;
            tb_songArtist.Text = obj[0].Artist;
            MusicUrl.Text = obj[0].HqUrl;
        }
        //Json转泛型 方法
        public static T FromJsonTo<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T jsonObject = (T)ser.ReadObject(ms);
            ms.Dispose();
            return jsonObject;
        }

        
    }

    

}
