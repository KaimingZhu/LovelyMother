using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ViewProgress : Page
    {

        public ViewProgress()
        {
            DataContext = ViewModelLocator.Instance.AddProgressViewModel;
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            Frame.Navigate(typeof(MainPage));
        }

        private void AddProgress_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            Frame.Navigate(typeof(AddProgress));
        }

        private void BlackListListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Delete.IsEnabled = CanDeleteJudge();
        }

        private bool CanDeleteJudge()
        {
            if(BlackListListView.SelectedItems.Count() == 0)
            {
                return false;
            }
            else
            {
                for(int i = 0; i < BlackListListView.SelectedItems.Count(); i++)
                {
                    var temp = BlackListListView.SelectedItems[i] as Motherlibrary.MyDatabaseContext.BlackListProgress;
                    if(( temp.Type == 0 ) || ( temp.Type == 1 ))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (BlackListListView.SelectedItems.Count != 0)
            {
                int i;
                var selected_items = new List<Motherlibrary.MyDatabaseContext.BlackListProgress>();
                for (i = 0; i < BlackListListView.SelectedItems.Count; i++)
                {
                    selected_items.Add((Motherlibrary.MyDatabaseContext.BlackListProgress)BlackListListView.SelectedItems[i]);
                }
                Messenger.Default.Send<AddProgressMessage>(new AddProgressMessage() { choice = 1, ifSelectToAdd = false, deleteList = selected_items });
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<AddProgressMessage>(new AddProgressMessage() { choice = 3, ifSelectToAdd = false });
        }
    }
}
