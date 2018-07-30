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
using LovelyMother.Uwp.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LovelyMother.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListTask : Page
    {
        public ListTask()
        {
            DataContext = ViewModelLocator.Instance.TaskViewModel;
            Messenger.Default.Send<UpdateTaskCollectionMessage>(new UpdateTaskCollectionMessage() { selection = 4 });
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<UpdateTaskCollectionMessage>(new UpdateTaskCollectionMessage() { selection = 1 });
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<UpdateTaskCollectionMessage>(new UpdateTaskCollectionMessage() { selection = 4 });
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItems.Count != 0)
            {
                int i;
                var selected_items = new List<Motherlibrary.MyDatabaseContext.Task>();
                for (i = 0; i < TaskListView.SelectedItems.Count; i++)
                {
                    selected_items.Add((Motherlibrary.MyDatabaseContext.Task)TaskListView.SelectedItems[i]);
                }
                Messenger.Default.Send<UpdateTaskCollectionMessage>(new UpdateTaskCollectionMessage() { selection = 2, taskList = selected_items });
            }
        }
    }
}
