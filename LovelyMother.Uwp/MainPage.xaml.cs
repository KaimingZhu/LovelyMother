using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        /// <summary>
        /// 倒计时组件
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_LoadingAsync(FrameworkElement sender, object args)
        {
            DiagnosticAccessStatus diagnosticAccessStatus = await AppDiagnosticInfo.RequestAccessAsync();
        }

        /**
        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (E.Time <= S.Time)
            {
                await new MessageDialog("时间设置错误！！").ShowAsync();//弹窗。
            }
            else
            {
                //Messenger.Default.Send(new RefreshMessage() { refreshMessage = "Begin" });
                int i = 0;
                var setTime = E.Time - S.Time;

                int TimeCount = setTime.Hours * 3600 + setTime.Minutes * 60;//倒计时秒数

                //传入的参数。


                //await new MessageDialog().ShowAsync();
                //int TimeCount = 10;
                timer.Tick += new EventHandler<object>(async (sende, ei) =>
                {
                    await Dispatcher.TryRunAsync
                    (CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                    {
                        i += 1;
                        double temp = (740 * Math.PI) * i / TimeCount / 15;

                        MyEllipse.StrokeDashArray = new DoubleCollection() { temp, 1000 };

                        txt.Text = ((TimeCount - i) / 3600).ToString("00") + ":"//文本显示。
                        + (((TimeCount - i) % 3600) / 60).ToString("00") + ":"
                        + (((TimeCount - i) % 3600) % 60).ToString("00");

                        if (i == TimeCount)
                        {
                            timer.Stop();
                            //Messenger.Default.Send(new ListenMessage() { listenMessage = "End" });
                        }
                    }));

                });
                timer.Start();
            }
        }
    **/
    }
}
