using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LovelyMother.Uwp.Models;
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

        public ObservableCollection<TaskBindingModel> bindingCollection
        {
            get;
            private set;
        }

        public ObservableCollection<Motherlibrary.MyDatabaseContext.Task> taskCollection
        {
            get;
            private set;
        }

        //本地日程读取服务
        private readonly ILocalTaskService _localTaskService;

        //服务器日程读取服务
        private readonly IWebTaskService _webTaskService;

        private readonly IIdentityService _identityService;

        //更新Task
        public async void RefreshTaskCollection()
        {
            //更新Collection
            taskCollection.Clear();
            bindingCollection.Clear();

            //读取本地
            var localTask = await _localTaskService.ListTaskAsync();

            foreach (var temp in localTask)
            {
                taskCollection.Add(temp);
            }

            foreach (var temp in localTask)
            {
                temp.Date = temp.Date.Insert(4,"-");
                temp.Date = temp.Date.Insert(7,"-");
                temp.Begin = temp.Begin.Insert(2, ":");
                temp.Begin = temp.Begin.Insert(5, ":");
                taskCollection.Add(temp);

                if(temp.FinishFlag == 0)
                {
                    bindingCollection.Add(new TaskBindingModel() { uri = "Assets/Images/Success.jpg", theTask = temp , condition = "成功" }); 
                }
                else
                {
                    bindingCollection.Add(new TaskBindingModel() { uri = "Assets/Images/Fail.jpg", theTask = temp , condition="失败"});
                }
            }
            
            if(_identityService.GetCurrentUserAsync().ID != 0)
            {
                //读取服务器
                var webTask = await _webTaskService.ListWebTaskAsync();
                foreach (var temp in webTask)
                {
                    taskCollection.Add(_localTaskService.WebTaskToLocal(temp));
                }
            }

            //排序（?）
            taskCollection.OrderByDescending(m => m.Date);
        }

        public TaskViewModel(ILocalTaskService localTaskService, IWebTaskService webTaskService, IIdentityService identityService)
        {
            //所需的Service
            _localTaskService = localTaskService;
            _webTaskService = webTaskService;
            _identityService = identityService;

            taskCollection = new ObservableCollection<Motherlibrary.MyDatabaseContext.Task>();
            bindingCollection = new ObservableCollection<TaskBindingModel>();

            Messenger.Default.Register<UpdateTaskCollectionMessage>(this, async (message) =>
            {
            switch (message.selection)
            {
                case 2:
                    {   
                        //删除任务
                        //分为两项：

                        var webTaskList = new List<WebTask>();
                        var localTaskList = new List<Motherlibrary.MyDatabaseContext.Task>();

                        foreach (var task in message.taskList)
                        {
                            if( task.UserID == -1 )
                            {
                                localTaskList.Add(task);
                            }
                            else
                            {
                                webTaskList.Add(_webTaskService.LocalTaskToWeb_NoneUser(task));
                            }
                        }
                            
                        //删除本地
                        await _localTaskService.DeleteTaskAsync(localTaskList);
                        
                        //删除服务器
                        foreach (var task in webTaskList)
                        {
                            await _webTaskService.DeleteWebTaskAsync(task.ID);
                        }

                        RefreshTaskCollection();
                        break;
                    }
                case 3:
                    {
                        //改变
                        if(message.taskList[0].UserID == -1)
                        {
                            await _localTaskService.UpdateTaskIntroductionAsync(message.taskList[0]);
                        }
                        else
                        {
                                //TODO : 此处的值需要修改 -》 生成只变更introduction的接口
                            await _webTaskService.UpdateWebTaskAsync(message.taskList[0].ID, message.taskList[0].FinishFlag, 
                                    message.taskList[0].FinishTime, message.taskList[0].Introduction);
                        }
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
