using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.ViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XamlAnimatedGif;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CountDownPage : Page
    {

        //倒计时进程声明
        private DispatcherTimer timer;

        //预设时间传值
        public double _defaultTime { get; private set; }

        public CountDownPage()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

            //置顶测试
            ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);

            this.DataContext = ViewModelLocator.Instance.CountDownViewModel;
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _defaultTime = ( double )e.Parameter;
        }

        private void RunTimePicker()
        {
            int start = (int)_defaultTime * 60;
            int i = (int)_defaultTime * 60;

            //添加数据库项
            timer.Tick += new EventHandler<object>(async (sende, ei) =>
            {
               if(start == i)
               {
                   //下层对上层透明，对服务器/数据库读写runBackGround
                   Messenger.Default.Send<AddTask>(new AddTask() { message = "Init", parameter = start / 60 });
               }
               i--;
               if(((start - i)%60 == 0) && (i > 0))
               {
                    Messenger.Default.Send<AddTask>(new AddTask(){ message="Refresh" });    
               }
               await Dispatcher.TryRunAsync
                   (CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                   {
                       txt.Text = (i / 3600).ToString("00") + ":"//文本显示。
                        + ((i % 3600) / 60).ToString("00") + ":"
                        + ((i % 3600) % 60).ToString("00");
                       if (i <= 0)
                       {
                           StopService(1);
                       }
                   }));
            });
            timer.Start();
            Messenger.Default.Send(new BeginListenMessage() { DefaultTime = 233 });
        }
    


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //写入失败信息
            StopService(3);
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            //写入成功信息
            StopService(2);
        }

        //暂停倒计时状态的进程，并进行页面跳转
        private void StopService(int type)
        {
            //type = 1(成功) / 2 (提前完成) / 3 (失败) 
            Messenger.Default.Send<StopListenMessage>(new StopListenMessage() { stopListenMessage = "你怎么回事弟弟" });
            switch (type)
            {
                case 1:
                    {
                        Messenger.Default.Send<AddTask>(new AddTask() { message = "Finish" });
                        Frame.Navigate(typeof(MainPage),1);
                        break;
                    }
                case 2:
                    {
                        Messenger.Default.Send<AddTask>(new AddTask() { message = "ForeFinish" });
                        Frame.Navigate(typeof(MainPage),2);
                        break;
                    }
                case 3:
                    {
                        Messenger.Default.Send<AddTask>(new AddTask() { message = "Fail" });
                        Frame.Navigate(typeof(MainPage),3);
                        break;
                    }
            }
            timer.Stop();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RunTimePicker();   
        }
    }
}
