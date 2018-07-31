using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.ViewModels;
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
        //辅助判断
        private int i;

        public AddProgress()
        {
            DataContext = ViewModelLocator.Instance.AddProgressViewModel;
            i = 0;
            this.InitializeComponent();
        }
        private async void NewProgress_Click(object sender, RoutedEventArgs e)
        {
            if(i == 0)
            {
                Messenger.Default.Send<AddProgressMessage>(new AddProgressMessage() { choice = 1, ifSelectToAdd = true });
                await new MessageDialog("请关闭程序后摁下按钮~").ShowAsync();//弹窗。
                NewProgress.Label = "已确认关闭";
                i++;
            }
            else if(i == 1)
            {
                Messenger.Default.Send<AddProgressMessage>(new AddProgressMessage() { choice = 2, ifSelectToAdd = true });
                if(ProgressListView.Items.Count() == 0)
                {
                    await new MessageDialog("未发现新程序！").ShowAsync();//弹窗。
                    i = 0;
                    Frame root = Window.Current.Content as Frame;
                    Frame.Navigate(typeof(ViewProgress));
                }
                else
                {
                    await new MessageDialog("请选择要添加的程序后摁下按钮~").ShowAsync();//弹窗。
                    ProgressListView.Visibility = Visibility.Visible;
                    theBlock.Visibility = Visibility.Visible;
                    ResetName.Visibility = Visibility.Visible;
                    NewProgress.Label = "添加";
                    i++;
                }
            }
            else
            {
                if(ProgressListView.SelectedItems.Count() != 0)
                {
                    Messenger.Default.Send<AddProgressMessage>(new AddProgressMessage() { choice = 3, ifSelectToAdd = true, parameter = ProgressListView.SelectedItem as Process, newName = ResetName.Text});
                    i = 0;
                    Frame root = Window.Current.Content as Frame;
                    Frame.Navigate(typeof(ViewProgress));
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            i = 0;
            Frame.Navigate(typeof(ViewProgress));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            theBlock.Visibility = Visibility.Collapsed;
            ResetName.Visibility = Visibility.Collapsed;
            ProgressListView.Visibility = Visibility.Collapsed;
            i = 0;
            await new MessageDialog("请打开程序，在程序开启后摁下按钮~").ShowAsync();//弹窗。
            NewProgress.Label = "已确认打开";
            
        }
    }
}
