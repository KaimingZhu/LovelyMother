using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models.Messages;
using LovelyMother.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovelyMother.Uwp.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {

        public ObservableCollection<Motherlibrary.MyDatabaseContext.Task> taskCollection
        {
            get;
            private set;
        }

        //本地日程读取服务
        private ILocalTaskService _localTaskService;

        //服务器日程读取服务

        //规格化：首部加0
        private string NormalLize(string temp,int time)
        {
            if(time > 0)
            {
                string template = "0";
                for (int i = time - 1; i > 0; i++)
                {
                    template += "0";
                }
                template += temp;
                return template;
            }
            return temp;
        }

        private Motherlibrary.MyDatabaseContext.Task getTaskWithNowTime()
        {
            DateTime dateTime = DateTime.Now;

            string year = dateTime.Year.ToString();

            string month = dateTime.Month.ToString();
            month = NormalLize(month,2 - month.Count());

            string day = dateTime.Day.ToString();
            day = NormalLize(day, 2 - day.Count());

            string hour = dateTime.Hour.ToString();
            hour = NormalLize(hour, 2 - hour.Count());

            string minute = dateTime.Minute.ToString();
            minute = NormalLize(minute, 2 - minute.Count());

            string second = dateTime.Second.ToString();
            second = NormalLize(second, 2 - second.Count());

            return _localTaskService.GetTask(year + month + day, hour + minute + second, 5, 5, "test", 0, -1);
        }

        public async void RefreshTaskCollection()
        {
            //更新Collection
            taskCollection.Clear();
            var temp = await _localTaskService.ListTaskAsync();
            foreach (var template in temp)
            {
                taskCollection.Add(template);
            }
        }

        public TaskViewModel(ILocalTaskService localTaskService)
        {
            _localTaskService = localTaskService;
            taskCollection = new ObservableCollection<Motherlibrary.MyDatabaseContext.Task>();
            Messenger.Default.Register<UpdateTaskCollectionMessage>(this, async (message) =>
            {
            switch (message.selection)
            {
                case 1:
                    {
                        //新增任务
                        var temp = await _localTaskService.AddTaskAsync(getTaskWithNowTime());
                        RefreshTaskCollection();
                        break;
                    }
                case 2:
                    {
                        //删除任务
                        await _localTaskService.DeleteTaskAsync(message.taskList);
                        RefreshTaskCollection();
                        break;
                    }
                case 3:
                    {
                        //改变
                        await _localTaskService.UpdateTaskAsync(message.taskList[0]);
                        RefreshTaskCollection();
                        break;
                    }
                case 4:
                    {
                        //更新Collection
                        RefreshTaskCollection();
                        break;
                    }
                }
            }
            );
        }
    }
}
