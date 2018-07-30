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
using LovelyMother.Uwp.Models;
using LovelyMother.Uwp.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class YuhaoTest3 : Page
    {
        public YuhaoTest3()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.WebTaskViewModel;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {

            var clickedTask = e.ClickedItem as WebTask;

            (DataContext as WebTaskViewModel).SelectTask.ID = clickedTask.ID;
            (DataContext as WebTaskViewModel).SelectTask.Date = clickedTask.Date;
            (DataContext as WebTaskViewModel).SelectTask.Begin = clickedTask.Begin;
            (DataContext as WebTaskViewModel).SelectTask.DefaultTime = clickedTask.DefaultTime;
            (DataContext as WebTaskViewModel).SelectTask.FinishTime = clickedTask.FinishTime;
            (DataContext as WebTaskViewModel).SelectTask.FinishFlag = clickedTask.FinishFlag;
            (DataContext as WebTaskViewModel).SelectTask.Introduction = clickedTask.Introduction;
            (DataContext as WebTaskViewModel).SelectTask.UserID = clickedTask.UserID;


        }
    }
}
