using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class AddProgress : Page
    {
        public AddProgress()
        {
            this.InitializeComponent();
        }
        int i = 0;//控制功能变量。
        private async void NewProgress_Click(object sender, RoutedEventArgs e)
        {
            if(i==0)
            {
                await new MessageDialog("请打开程序后摁下按钮~").ShowAsync();//弹窗。
                i++;

            }
            else if(i==1)
            {
                await new MessageDialog("请关闭程序后摁下按钮~").ShowAsync();//弹窗。
                i++;
            }
            else
            {
                await new MessageDialog("请选择要添加的程序后摁下按钮~").ShowAsync();//弹窗。
                i = 0;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewProgress));
        }
    }
}
