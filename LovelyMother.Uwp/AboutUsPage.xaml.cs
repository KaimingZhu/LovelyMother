using System.Reflection;
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
using Windows.UI.ViewManagement;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutUsPage : Page
    {
        public AboutUsPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(800, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void project_Click(object sender, RoutedEventArgs e)
        {
            Project.Visibility = Visibility.Visible;
            Member.Visibility = Visibility.Collapsed;
        }

        private void member_Click(object sender, RoutedEventArgs e)
        {
            Project.Visibility = Visibility.Collapsed;
            Member.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            project_Click(sender, e);
        }
    }
}
