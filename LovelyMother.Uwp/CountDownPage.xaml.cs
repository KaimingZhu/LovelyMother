using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.ViewModels;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CountDownPage : Page
    {

        //倒计时进程判断符
        private static bool ifTimePickerRun = false;

        //倒计时进程声明
        private DispatcherTimer timer;

        //预设时间传值
        public double _defaultTime { get; private set; }

        public CountDownPage()
        {

            this.DataContext = ViewModelLocator.Instance.CountDownViewModel;
            this.InitializeComponent();

            Messenger.Default.Register<PunishWindowMessage>(this, (message) =>
            {
                //TODO : How to Solve

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    PunishWindowAsync();
                });

            });

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _defaultTime = (double) e.Parameter;
        }

        private void RunTimePicker()
        {
            int i = (int)_defaultTime * 60;
            if (ifTimePickerRun == false)
            {
                ifTimePickerRun = true;
                timer.Tick += new EventHandler<object>(async (sende, ei) =>
                {
                    i--;
                    await Dispatcher.TryRunAsync
                        (CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                        {
                            txt.Text = (i / 3600).ToString("00") + ":"//文本显示。
                             + ((i % 3600) / 60).ToString("00") + ":"
                             + ((i % 3600) % 60).ToString("00");
                            if (i <= 0)
                            {
                                stopService();
                            }
                        }));
                });
                timer.Start();
                Messenger.Default.Send(new BeginListenMessage() { DefaultTime = 233 });
            }
        }

        private async void PunishWindowAsync()
        {
            var currentAV = ApplicationView.GetForCurrentView();
            var newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            async () =>
                            {
                                var newWindow = Window.Current;
                                var newAppView = ApplicationView.GetForCurrentView();
                                newAppView.Title = "你怎么回事弟弟？";

                                var frame = new Frame();
                                frame.Navigate(typeof(PunishPage), null);
                                newWindow.Content = frame;
                                newWindow.Activate();

                                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                                newAppView.Id,
                                ViewSizePreference.UseMinimum,
                                currentAV.Id,
                                ViewSizePreference.UseMinimum);
                            });
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //写入失败信息
            stopService();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            //写入成功信息
            stopService();
        }

        //暂停倒计时状态的进程，并进行页面跳转
        private void stopService()
        {
            Messenger.Default.Send<StopListenMessage>(new StopListenMessage() { stopListenMessage = "你怎么回事弟弟" });
            ifTimePickerRun = false;
            timer.Stop();
            Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            RunTimePicker();
        }
    }
}
