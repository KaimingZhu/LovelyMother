using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class CutDown : Page
    {
        public CutDown()
        {
            Timers();
            this.InitializeComponent();
        }

        private void Timers()
        {
            int i = 360;
            DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += new EventHandler<object>(async (sende, ei) =>
            {

                i--;

                await Dispatcher.TryRunAsync
                    (CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                    {
                        txt.Text = (i / 3600).ToString("00") + ":"//文本显示。
                         + ((i % 3600) / 60).ToString("00") + ":"
                         + ((i % 3600) % 60).ToString("00");
                        if (i == 0)
                        {
                            timer.Stop();
                        }
                    }));

            });
            timer.Start();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }


    }
}
