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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

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
        int i=-1;
        BitmapImage bitimage = new BitmapImage();


        public MainPage()
        {
            this.InitializeComponent();
        }
        //获取音乐信息
        private async void Get_Music(object sender, RoutedEventArgs e)
        {
            var apiUrl_GetJson = string.Format("http://api.itwusun.com/music/search/qq/1?format=json&sign=a5cc0a8797539d3a1a4f7aeca5b695b9&keyword={0}", this.tbox_musicName.Text);
            //开始准备访问api服务器 实例化client
            try
            {
                HttpClient client = new HttpClient();
                //把自己本地整理好的数据发送给api服务器
                var json = await client.GetStringAsync(new Uri(apiUrl_GetJson));
                //判断是否有json返回
                if (json == null)
                {
                    return;
                }
                obj = FromJsonTo<List<music>>(json);//耳朵提供的方法
                                                    //如果瞎按，还是按获取那就重置i，回到第一条查询信息
                i = 0;//如果用户是正常按的 就重置为0
                show_result(i);
            }
            catch
            {
                MessageDialog ms = new MessageDialog("小兄弟，你好像没联网吧。要不就是出了点问题，重启一下试试？","多喝热水，重启试试，不行刷机");
            }           
        }
       
        public void show_result(int i)
        {
            tb_songName.Text = obj[i].SongName;
            tb_songArtist.Text = obj[i].Artist;
            tb_size.Text = obj[i].Size;
            tb_length.Text = obj[i].Length;
            tb_album.Text = obj[i].Album;
            MusicUrl.Text = obj[i].HqUrl;
            tb_SQ.Text = obj[i].SqUrl;
            tb_Flac.Text = obj[i].FlacUrl;
            show_image.Width = bitimage.DecodePixelWidth = 160;
            bitimage.UriSource = new Uri(show_image.BaseUri, obj[i].PicUrl);
            show_image.Source = bitimage;
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

        private void NextResult_Click(object sender, RoutedEventArgs e)
        {          
            if(this.tb_HQUrl==null||i>29||i==-1)
            {
                //如果没有获取到信息那么这个按钮没啥用
                //或者查询记录超过了30条
                return;
            }            
            i++;
            show_result(i);
        }
    }

    

}
