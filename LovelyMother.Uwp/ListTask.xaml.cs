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
using Windows.UI.Popups;

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
            Frame root = Window.Current.Content as Frame;
            Frame.Navigate(typeof(MainPage));
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

        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TaskListView.SelectedItems.Count != 0)
            {
                //如果选择的的是唯一一个，而且flag!=0 , 更新与重新挑战为true
                if (TaskListView.SelectedItems.Count == 1)
                {
                    var temp = TaskListView.SelectedItem as Motherlibrary.MyDatabaseContext.Task;
                    if(temp != null)
                    {
                        if(temp.FinishFlag != 0)
                        {
                            Chanllenge.IsEnabled = true;
                        }
                        else
                        {
                            Chanllenge.IsEnabled = false;
                        }
                    }
                    else
                    {
                        Chanllenge.IsEnabled = false;
                    }
                    UpdateButton.IsEnabled = true;
                }
                else
                {
                    
                    UpdateButton.IsEnabled = false;
                    Chanllenge.IsEnabled = false;
                }
                RemoveSelected.IsEnabled = true;

            }
            else
            {
                Chanllenge.IsEnabled = false;
                UpdateButton.IsEnabled = false;
                RemoveSelected.IsEnabled = false;
            }
        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            TaskListView.SelectedItems.Clear();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TaskListView.SelectedItems.Clear();
            Chanllenge.IsEnabled = false;
            RemoveSelected.IsEnabled = false;
            UpdateButton.IsEnabled = false;
        }

        private void Chanllenge_Click(object sender, RoutedEventArgs e)
        {
            //删除对应项
            var selected_items = new List<Motherlibrary.MyDatabaseContext.Task>();
            var deleteItem = TaskListView.SelectedItem as Motherlibrary.MyDatabaseContext.Task;
            selected_items.Add(deleteItem);
            Messenger.Default.Send<UpdateTaskCollectionMessage>(new UpdateTaskCollectionMessage() { selection = 2, taskList = selected_items });

            //开始跳转
            Frame.Navigate(typeof(CountDownPage), (double)(deleteItem.DefaultTime));
        }

        /*
         * private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var templist = new List<Motherlibrary.MyDatabaseContext.Task>();
            templist.Add(TaskListView.SelectedItem as Motherlibrary.MyDatabaseContext.Task);
            templist[0].Introduction = Introduction.Text;
            templist[0].FinishTime = int.Parse(FinishTime.Text);
            templist[0].DefaultTime = int.Parse(DefaultTime.Text);
            Messenger.Default.Send<UpdateTaskCollectionMessage>(new UpdateTaskCollectionMessage() { selection = 3, taskList = templist });
        }
        */
    }
}
