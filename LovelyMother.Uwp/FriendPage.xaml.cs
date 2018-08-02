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
    public sealed partial class FriendPage
    {
        public FriendPage()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.FriendViewModel;

        }
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           (this.DataContext as FriendViewModel).refresh();
        }


        private void FriendListView_OnItemClick(object sender, ItemClickEventArgs e)
        {



            var selectItem = e.ClickedItem as FriendList;
            (this.DataContext as FriendViewModel).SelectFriend.FriendID = selectItem.FriendID;
            (this.DataContext as FriendViewModel).SelectFriend.FriendUserName = selectItem.FriendUserName;

           
        }
    }
}
